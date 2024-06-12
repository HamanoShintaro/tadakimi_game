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
    private CharacterType characterType = CharacterType.Buddy;

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

    [SerializeField]
    private AudioClip normalAttackDamageSounds;
    private Player player;
    private RectTransform rectTransform;
    private Rigidbody2D rb; // Rigidbody2Dコンポーネント

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
        skillCost = characterInfo.skill.cost;
        skillCoolDown = characterInfo.skill.cd;
        skillRatio = characterInfo.skill.Ratio;
        specialCost = characterInfo.skill.cost;
        specialCoolTime = characterInfo.special.cd;
        specialRatio = characterInfo.special.Ratio;
        hasSkill = !string.IsNullOrEmpty(characterInfo.skill.name) && characterInfo.skill.name != "9999";
        hasSpecial = !string.IsNullOrEmpty(characterInfo.special.name) && characterInfo.special.name != "9999";
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
            return;
        }
        else
        {
            animator.SetBool("Walk", false);
        }

        if (targets.Count == 0)
        {

        }
        else
        {
            Action();
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
            Debug.Log("足を止める");
            Action();
            Debug.Log("アクション！");
        }
    }

    private void Walk()
    {
        if (isLeader) return;
        animator.SetBool("Walk", true);
        float direction = characterType == CharacterType.Buddy ? 1 : -1;
        transform.position = new Vector2(transform.position.x + speed * direction, transform.position.y);
    }

    private void EndWalk()
    {
        animator.SetBool("Walk", false);
    }

    private void Action()
    {
        if (!canState) return;
        canState = false;
        Debug.Log("アクションしない！");

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
            NormalAction();
            Debug.Log("のーまる");
        }
    }

    private void SpecialAction()
    {
        magicPowerController.magicPower -= specialCost;
        SetCharacterPanelIndex();
        animator.SetBool("Special", true);
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
        Debug.Log("Normal Attack Triggered");
    }

    // スペルミス
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
        /*
        canState = false;
        animator.SetBool("KnockBack", true);
        const float knockBackInterval = 0.1f;
        const int knockBackSteps = 10;
        const float knockBackDistance = 40;
        const float jumpHeight = 80; // ジャンプの高さ
        float originalY = transform.position.y; // 元のY座標

        var wait = new WaitForSeconds(knockBackInterval);
        for (int i = 0; i < knockBackSteps; i++)
        {
            yield return wait;
            float direction = characterType == CharacterType.Buddy ? -1 : 1;
            float x = transform.position.x + knockBackDistance * direction;

            // Y軸方向の変化を追加
            float y = originalY + jumpHeight * Mathf.Sin((i / (float)knockBackSteps) * Mathf.PI);

            transform.position = new Vector2(x, y);
        }
        // ノックバック後に元の高さに戻る
        transform.position = new Vector2(transform.position.x, originalY);
        */
        
        canState = false;
        animator.SetBool("KnockBack", true);
        const float knockBackDuration = 0.5f;
        const float knockBackForce = 800f;
        //const float jumpForce = 400f;
        const float jumpHeight = 100f; // ジャンプの高さ

        float direction = characterType == CharacterType.Buddy ? -1 : 1;
        float originalY = transform.position.y; // 元のY座標を取得

        float elapsedTime = 0f;

        while (elapsedTime < knockBackDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / knockBackDuration;
            float x = transform.position.x + direction * knockBackForce * Time.deltaTime;
            float y = originalY + jumpHeight * Mathf.Sin(t * Mathf.PI); // 山なりの軌道を実現
            transform.position = new Vector2(x, y);
            yield return null;
        }
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
        if (t.isTrigger) return;

        bool isTarget = characterRole == CharacterRole.Attacker
            ? (characterType == CharacterType.Buddy && t.CompareTag("Enemy")) || (characterType == CharacterType.Enemy && t.CompareTag("Buddy"))
            : (characterType == CharacterType.Buddy && t.CompareTag("Buddy")) || (characterType == CharacterType.Enemy && t.CompareTag("Enemy"));

        if (isTarget && !targets.Contains(t.gameObject))
        {
            targets.Add(t.gameObject);
            targets = targets.OrderBy(n => n.GetComponent<RectTransform>().anchoredPosition.x).ToList();
        }
    }

    private void OnTriggerExit2D(Collider2D t)
    {
        if (t.isTrigger) return;

        bool isTarget = characterRole == CharacterRole.Attacker
            ? (characterType == CharacterType.Buddy && t.CompareTag("Enemy")) || (characterType == CharacterType.Enemy && t.CompareTag("Buddy"))
            : (characterType == CharacterType.Buddy && t.CompareTag("Buddy")) || (characterType == CharacterType.Enemy && t.CompareTag("Enemy"));

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
}
