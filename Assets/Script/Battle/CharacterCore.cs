using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// キャラクターのステータス管理、移動、攻撃、死亡、ノックバック、回復、被ダメージ、強化の処理を行うメソッド
/// </summary>
public class CharacterCore : MonoBehaviour, IDamage, IRecovery, ITemporaryEnhance
{
    [SerializeField]
    [Tooltip("キャラクターID")]
    private CharacterId characterId;

    [SerializeField]
    [Tooltip("味方/敵")]
    public CharacterType characterType = CharacterType.Buddy;

    [SerializeField]
    [Tooltip("攻撃範囲の種類")]
    private AttackType attackType = AttackType.Single;

    [SerializeField]
    [Tooltip("役職")]
    private CharacterRole characterRole = CharacterRole.Attacker;

    [SerializeField]
    [Tooltip("リーダー")]
    private bool isLeader;

    [SerializeField]
    [Header("遠距離攻撃する最小の距離")]
    [Tooltip("トリガー範囲>(遠距離攻撃)>longAttackDistance>(近距離攻撃)>limitMovePosition>(近づけない)")]
    private float longAttackDistance = 600;

    private float minLimitMovePosition = -2700f;

    private float maxLimitMovePosition = 2700f;

    [Tooltip("現在のレベル")]
    [HideInInspector]
    public int level = 0;
    private float maxHp;
    private float defKB;
    private float maxSpeed;
    private float atkKB;
    [HideInInspector]
    public float atkPower;
    private int skillCost;
    private float skillCoolDown;
    private bool hasSkill;
    private float skillRatio;
    private int specialCost;
    private float specialCoolTime;
    private bool hasSpecial;
    private float specialRatio;
    public List<GameObject> targets = new List<GameObject>();
    private MagicPowerController magicPowerController;
    private Animator animator;
    public bool canState = true;
    private bool canSkillCoolTime = true;
    private bool canSpecialCoolTime = true;
    private GameObject characterPanel;
    private AudioSource audioSource;

    //ノックバックの秒数
    private const float knockBackDuration = 2f;
    //ノックバックの距離
    private const float knockBackForce = 150f;
    //ノックバックする時の高さ
    private const float jumpHeight = 50f;

    // グローバル変数として出現時のY座標を保存
    private float originalY;

    [SerializeField]
    private AudioClip normalAttackDamageSounds;
    private Player player;
    private RectTransform rectTransform;
    private Rigidbody2D rb;

    public float hp = 100;
    public float Hp
    {
        get { return hp; }
        set { hp = Mathf.Clamp(value, 0, maxHp); }
    }

    private float speed = 0;
    public float Speed
    {
        get { return speed; }
        set { speed = Mathf.Clamp(value, 0, maxSpeed); }
    }

    private float skillCoolTime;
    public float SkillCoolTime
    {
        get { return skillCoolTime; }
        set { skillCoolTime = Mathf.Clamp(value, 0, skillCoolDown); }
    }

    private void Start()
    {
        InitializeCharacter();
        InitializeComponents();
        StartCoroutine(SkillCoolTimeCount());

        originalY = transform.position.y;
    }

    private void InitializeCharacter()
    {
        if (characterType == CharacterType.Buddy)
        {
            SaveController saveController = new SaveController();
            saveController.characterSave.Load();
            try
            {
                level = saveController.characterSave.list.Find(characterSave => characterSave.id == characterId.ToString()).level;
            }
            catch
            {
                level = 1;
            }
        }

        var characterInfo = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/{characterId}");
        if (characterInfo == null)
        {
            Debug.LogError($"{characterId} : データベースにキャラクターのデータがありません");
            return;
        }

        maxHp = characterInfo.status[level].hp;
        Hp = maxHp;
        maxSpeed = characterInfo.status[level].speed / 20;
        Speed = maxSpeed;
        atkPower = characterInfo.status[level].attack;
        atkKB = characterInfo.status[level].atkKB;
        defKB = characterInfo.status[level].defKB;

        hasSkill = !string.IsNullOrEmpty(characterInfo.skill.name);
        skillCost = characterInfo.skill.cost;
        skillCoolDown = characterInfo.skill.cd;
        skillRatio = characterInfo.skill.Ratio;

        hasSpecial = !string.IsNullOrEmpty(characterInfo.special.name);
        specialCost = characterInfo.skill.cost;
        specialCoolTime = characterInfo.special.cd;
        specialRatio = characterInfo.special.Ratio;
    }

    private void InitializeComponents()
    {
        characterPanel = transform.parent.gameObject;
        magicPowerController = GameObject.Find("Canvas_Dynamic/[ControlPanel]/Power").GetComponent<MagicPowerController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        player = GetComponent<Player>();
        rectTransform = GetComponent<RectTransform>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isLeader)
        {
            HandleLeaderActions();
        }
        else
        {
            HandleNonLeaderActions();
        }
    }

    private void HandleLeaderActions()
    {
        if (player.isMove)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
            if (targets.Count != 0)
            {
                Action();
            }
        }
    }

    private void HandleNonLeaderActions()
    {
        if (!canState) return;
        if (targets.Count == 0)
        {
            Walk();
        }
        else
        {
            animator.SetBool("Walk", false);
            Action();
        }
    }

    private void Walk()
    {
        if (isLeader) return;
        animator.SetBool("Walk", true);
        float direction = characterType == CharacterType.Buddy ? 1 : -1;
        float newPositionX = transform.position.x + speed * direction;

        if (IsOutOfBounds(newPositionX)) return;

        transform.position = new Vector2(newPositionX, transform.position.y);
    }

    private bool IsOutOfBounds(float positionX)
    {
        return positionX < minLimitMovePosition || positionX > maxLimitMovePosition;
    }

    private void EndWalk()
    {
        animator.SetBool("Walk", false);
    }

    private void Action()
    {
        if (!canState) return;
        canState = false;
        if (hasSpecial && specialCost <= magicPowerController.maxMagicPower && specialCoolTime == 0)
        {
            SpecialAction();
        }
        else if (hasSkill && skillCost <= magicPowerController.magicPower && SkillCoolTime == 0)
        {
            SkillAction();
        }
        else
        {
            if (characterRole != CharacterRole.Supporter)
            {
                NormalAction();
            }
            else
            {
                canState = true;
            }
        }
    }

    private void SpecialAction()
    {
        magicPowerController.magicPower -= specialCost;
        SetCharacterPanelIndex();
        animator.SetBool("Special", true);
        Debug.Log($"{characterId}はスペシャルのアニメーションを発動した");
    }

    public void EndSpecialAction()
    {
        animator.SetBool("Special", false);
        StartCoroutine(SpecialCoolTimeCount());
        canState = true;
    }

    private void SkillAction()
    {
        magicPowerController.magicPower -= skillCost;
        SetCharacterPanelIndex();
        animator.SetBool("Skill", true);
        Debug.Log($"{characterId}はスキルのアニメーションを発動した");
    }

    public void EndSkillAction()
    {
        animator.SetBool("Skill", false);
        StartCoroutine(SkillCoolTimeCount());
        canState = true;
    }

    private void NormalAction()
    {
        if (isLeader)
        {
            var nearTarget = targets.OrderBy(n => n.GetComponent<RectTransform>().anchoredPosition.x).FirstOrDefault();
            if (nearTarget != null)
            {
                var distance = Vector2.Distance(nearTarget.GetComponent<RectTransform>().anchoredPosition, rectTransform.anchoredPosition);
                if (distance > longAttackDistance)
                {
                    animator.SetBool("Long", true);
                }
            }
        }
        animator.SetBool("Attack", true);
        Debug.Log($"{characterId}は通常攻撃のアニメーションを発動した");
    }

    public void EndNomalAction()
    {
        if (isLeader)
        {
            animator.SetBool("Long", false);
        }
        animator.SetBool("Attack", false);
        canState = true;
    }

    public void InflictDamage(int type = 0)
    {
        float ratio = type switch
        {
            1 when hasSkill => skillRatio,
            2 when hasSpecial => specialRatio,
            _ => 1.0f
        };

        if (isLeader)
        {
            InflictDamageAsLeader(ratio);
        }
        else
        {
            InflictDamageAsNonLeader(ratio);
        }
        ResetTargets();
    }

    private void InflictDamageAsLeader(float ratio)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            var orderedTargets = targets.OrderBy(n => Vector2.Distance(n.GetComponent<RectTransform>().anchoredPosition, rectTransform.anchoredPosition));
            foreach (var target in orderedTargets)
            {
                var distance = Vector2.Distance(target.GetComponent<RectTransform>().anchoredPosition, rectTransform.anchoredPosition);
                if (distance < longAttackDistance)
                {
                    target.GetComponent<IDamage>().Damage(atkPower * ratio, atkKB);
                    if (attackType == AttackType.Single) break;
                }
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("LongAttack"))
        {
            var orderedTargets = targets.OrderBy(n => Vector2.Distance(n.GetComponent<RectTransform>().anchoredPosition, rectTransform.anchoredPosition));
            foreach (var target in orderedTargets)
            {
                target.GetComponent<IDamage>().Damage(atkPower * ratio, atkKB);
                if (attackType == AttackType.Single) break;
            }
        }
    }

    private void InflictDamageAsNonLeader(float ratio)
    {
        foreach (var target in targets)
        {
            target.GetComponent<IDamage>().Damage(atkPower * ratio, atkKB);
            if (attackType == AttackType.Single) break;
        }
    }

    private IEnumerator SkillCoolTimeCount()
    {
        if (!canSkillCoolTime) yield break;
        canSkillCoolTime = false;
        var wait = new WaitForSeconds(1);
        skillCoolTime = skillCoolDown;
        while (skillCoolTime > 0)
        {
            yield return wait;
            skillCoolTime--;
        }
        canSkillCoolTime = true;
    }

    private IEnumerator SpecialCoolTimeCount()
    {
        if (!canSpecialCoolTime) yield break;
        canSpecialCoolTime = false;
        var wait = new WaitForSeconds(1);
        while (specialCoolTime > 0)
        {
            yield return wait;
            specialCoolTime--;
        }
        canSpecialCoolTime = true;
    }

    private IEnumerator KnockBack()
    {
        // ステートを一時的に無効にする
        canState = false;

        // ノックバックアニメーションを再生する
        animator.SetBool("KnockBack", true);

        // キャラクターの種類に応じて、ノックバックの方向を決定する
        float direction = characterType == CharacterType.Buddy ? -1 : 1;

        // 経過時間を初期化
        float elapsedTime = 0f;

        // ノックバックが指定された期間続くまでループする
        while (elapsedTime < knockBackDuration)
        {
            // 経過時間を更新
            elapsedTime += Time.deltaTime;

            // ノックバックの進行度（0から1までの値）を計算
            float t = elapsedTime / knockBackDuration;

            // X座標に対してノックバックの力を加える
            float x = transform.position.x + direction * knockBackForce * Time.deltaTime;

            // Y座標にジャンプの高さを反映（正弦波で上下移動を表現）
            float y = originalY + jumpHeight * Mathf.Sin(t * Mathf.PI);

            // ノックバックが画面外に出ないように制御
            if (!IsOutOfBounds(x))
            {
                transform.position = new Vector2(x, y); // キャラクターの位置を更新
            }

            // 1フレーム待機して次の更新に進む
            yield return null;
        }

        // ノックバックが終わった後、Y座標を元の位置に戻す
        transform.position = new Vector2(transform.position.x, originalY);
    }


    public void EndKnockBack()
    {
        animator.SetBool("KnockBack", false);
        animator.SetBool("Attack", false);
        canState = true;
    }

    private void Death()
    {
        canState = false;
        animator.SetTrigger("Death");
    }

    public void EndDeath()
    {
        gameObject.SetActive(false);
    }

    public void Damage(float atkPower = 0, float atkKB = 0)
    {
        Hp -= atkPower;
        audioSource.PlayOneShot(normalAttackDamageSounds);
        if (Hp <= 0)
        {
            Death();
        }
        else if ((atkKB - defKB) * Random.value > 1 || atkKB.Equals(Mathf.Infinity))
        {
            StartCoroutine(KnockBack());
        }
        Debug.Log($"{characterId}が{atkPower}ダメージを受けた");
    }

    public void Recovery(int heal)
    {
        Hp += heal;
    }

    public IEnumerator TemporaryEnhance(float duration = 0.0f, int addSpeed = 0, int addMaxHp = 0)
    {
        Speed += addSpeed;
        maxHp += addMaxHp;
        yield return new WaitForSeconds(duration);
        Speed = speed;
        maxHp = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/{characterId}").status[level].hp;
    }

    private void ResetTargets()
    {
        targets.Clear();
    }

    private void OnTriggerStay2D(Collider2D t)
    {
        if (IsLongRangeTrigger(t)) return;
        bool isTarget = (characterType.Equals(CharacterType.Buddy) && t.CompareTag("Enemy")) || (characterType.Equals(CharacterType.Enemy) && t.CompareTag("Buddy"));
        if (isTarget && !targets.Contains(t.gameObject))
        {
            targets.Add(t.gameObject);
            targets = targets.OrderBy(n => n.GetComponent<RectTransform>().anchoredPosition.x).ToList();
        }
    }

    private void OnTriggerExit2D(Collider2D t)
    {
        if (IsLongRangeTrigger(t)) return;
        bool isTarget = (characterType.Equals(CharacterType.Buddy) && t.CompareTag("Enemy")) || (characterType.Equals(CharacterType.Enemy) && t.CompareTag("Buddy"));
        if (isTarget && targets.Contains(t.gameObject))
        {
            targets.Remove(t.gameObject);
            targets = targets.OrderBy(n => n.GetComponent<RectTransform>().anchoredPosition.x).ToList();
        }
    }

    private void SetCharacterPanelIndex()
    {
        var index = characterPanel.transform.childCount - 1;
        transform.SetSiblingIndex(index);
    }

    private bool IsLongRangeTrigger(Collider2D t)
    {
        BoxCollider2D[] colliders = t.gameObject.GetComponents<BoxCollider2D>();

        // 2つ目のBoxCollider2D(攻撃範囲のCollider2D)が存在するか確認
        if (colliders.Length > 1 && t == colliders[1])
        {
            return true;
        }
        return false;
    }
}
