using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Battle;
using System;

/// <summary>
/// キャラクターのステータス管理、移動、攻撃、死亡、ノックバック、回復、被ダメージ、強化の処理を行うクラス
/// </summary>
public class CharacterCore : MonoBehaviour, IDamage, IRecovery, ITemporaryEnhance
{
    // ノックバックの定数
    private const float KNOCKBACK_DURATION = 0.5f;         // ノックバックの秒数
    private const float KNOCKBACK_FORCE = 800f;            // ノックバックの距離
    private const float KNOCKBACK_JUMP_HEIGHT = 50f;       // ノックバックする時の高さ

    // クールタイム関連の定数
    private const float COOLTIME_INTERVAL = 1f;            // クールタイム計算の間隔(秒)
    
    // ダメージ計算関連の定数
    private const float KNOCKBACK_BASE_VALUE = 70f;        // ノックバック計算の基本値
    private const float KNOCKBACK_DIVIDER = 50f;           // ノックバック計算の除算値
    
    // アニメーション名
    private const string ANIM_WALK = "Walk";
    private const string ANIM_ATTACK = "Attack";
    private const string ANIM_LONG = "Long";
    private const string ANIM_SKILL = "Skill";
    private const string ANIM_SPECIAL = "Special";
    private const string ANIM_KNOCKBACK = "KnockBack";
    private const string ANIM_DEATH = "Death";
    
    // キャラクター固有識別子
    private const string SARA_ID = "Sara_01";
    
    // ResourcesロードPath
    private const string CHARACTER_INFO_PATH = "DataBase/Data/CharacterInfo/";
    
    // ステート
    private const int STATE_NORMAL = 0;
    private const int STATE_SKILL = 1;
    private const int STATE_SPECIAL = 2;
    
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
    private float longAttackDistance;

    [SerializeField]
    [Header("キャラクターのX座標の移動範囲を制限する左の壁の位置")]
    public float minLimitMovePosition = -1400f;
    [SerializeField]
    [Header("キャラクターのX座標の移動範囲を制限する左の壁の位置")]
    public float maxLimitMovePosition = 2800f;

    //イーラに攻撃力を上げさせられたかどうか。
    public bool hasDoubleAttackPower = false;

    [Tooltip("現在のレベル")]
    [HideInInspector]
    public int level = 0;

    private CharacterInfo characterInfo;

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

    private List<GameObject> confirmedTargets;

    // ノックバック中かどうかを追跡
    private bool isKnockBack = false;

    public float originalY;

    [SerializeField]
    private AudioClip normalAttackDamageSounds;
    private Player player;
    private RectTransform rectTransform;
    private Rigidbody2D rb;

    //ヴォルカスの攻撃判定のための変数。
    [SerializeField, Tooltip("自キャラと敵の進行方向の端どうしの距離")]
    private float frontEdgeDistance;

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

    /// <summary>
    /// 死亡時に呼び出されるコールバック
    /// </summary>
    public Action<CharacterCore> OnDeath;

    [SerializeField]
    //ヴォルカスの近距離攻撃範囲
    [Header("近距離攻撃の範囲")]
    private float meleeAttackRange = 1.5f;  // デフォルト値を1.5に設定

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
                level = 0;
            }
        }

        SetCharacterInfo(level);
    }

    public void SetCharacterInfo(int level)
    {
        characterInfo = Resources.Load<CharacterInfo>($"{CHARACTER_INFO_PATH}{characterId}");
        if (characterInfo == null)
        {
            Debug.LogError($"{characterId} : データベースにキャラクターのデータがありません");
            return;
        }

        // ステータス設定
        maxHp = characterInfo.status[level].hp;
        Hp = maxHp;

        maxSpeed = characterInfo.status[level].speed / 20;
        Speed = maxSpeed;

        atkPower = characterInfo.status[level].attack;
        atkKB = characterInfo.status[level].atkKB;
        defKB = characterInfo.status[level].defKB;

        // スキル設定
        hasSkill = characterInfo.skill != null && !string.IsNullOrEmpty(characterInfo.skill.name);
        if (hasSkill)
        {
            skillCost = characterInfo.skill.cost;
            skillCoolDown = characterInfo.skill.cd;
            skillRatio = characterInfo.skill.Ratio;
        }

        // 奥義（スペシャル）設定
        hasSpecial = characterInfo.special != null && !string.IsNullOrEmpty(characterInfo.special.name);
        if (hasSpecial)
        {
            specialCost = characterInfo.special.cost;
            specialCoolTime = characterInfo.special.cd;
            specialRatio = characterInfo.special.Ratio;
        }
    }

    private void InitializeComponents()
    {
        if (transform.parent != null)
        {
            characterPanel = transform.parent.gameObject;
        }

        var powerObj = GameObject.Find("Canvas_Dynamic/[ControlPanel]/Power");
        if (powerObj != null)
        {
            magicPowerController = powerObj.GetComponent<MagicPowerController>();
        }
        else
        {
            magicPowerController = null;
            Debug.LogWarning($"{characterId}: 魔力UIが見つかりません。召喚キャラなら問題ありません。");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning($"{characterId}: Animatorがアタッチされていません");
        }

        audioSource = GetComponent<AudioSource>();
        player = GetComponent<Player>(); // 召喚キャラにはいない可能性あり
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
            animator.SetBool(ANIM_WALK, true);
        }
        else
        {
            animator.SetBool(ANIM_WALK, false);
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
            animator.SetBool(ANIM_WALK, false);
            Action();
        }
    }

    private void Walk()
    {
        if (isLeader) return;
        animator.SetBool(ANIM_WALK, true);
        float direction = characterType == CharacterType.Buddy ? 1 : -1;
        float newPositionX = transform.position.x + speed * direction;
        if (IsOutOfBounds(newPositionX))
        {
            return;
        }
        transform.position = new Vector2(newPositionX, transform.position.y);
    }

    public bool IsOutOfBounds(float positionX)
    {
        return positionX < minLimitMovePosition || positionX > maxLimitMovePosition;
    }

    private void EndWalk()
    {
        animator.SetBool(ANIM_WALK, false);
    }

    private void Action()
    {
        if (!canState) return;
        canState = false;

        confirmedTargets = new List<GameObject>(targets);

        if (hasSpecial && specialCost <= magicPowerController.maxMagicPower && specialCoolTime == 0)
        {
            Debug.Log("SP攻撃");
            SpecialAction();
        }
        else if (characterId.ToString() != SARA_ID && hasSkill && skillCost <= magicPowerController.magicPower && SkillCoolTime == 0)
        {
            Debug.Log("Skill攻撃");
            SkillAction();
        }
        else
        {
            if (characterRole != CharacterRole.Supporter)
            {
                Debug.Log("ノーマル攻撃");
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
        animator.SetBool(ANIM_SPECIAL, true);
    }

    public void EndSpecialAction()
    {
        animator.SetBool(ANIM_SPECIAL, false);
        StartCoroutine(SpecialCoolTimeCount());
        canState = true;
    }

    private void SkillAction()
    {
        magicPowerController.magicPower -= skillCost;
        animator.SetBool(ANIM_SKILL, true);
    }

    public void EndSkillAction()
    {
        animator.SetBool(ANIM_SKILL, false);
        StartCoroutine(SkillCoolTimeCount());
        canState = true;
    }

    private void NormalAction()
    {
        var colliders = GetComponents<BoxCollider2D>();
        if (colliders.Length < 2)
        {
            Debug.LogWarning($"{gameObject.name}: コライダーが2つ未満のため処理を中断します");
            return;
        }

        BoxCollider2D attackCollider = colliders[1];
        BoxCollider2D enemyCollider = null;

        var nearTarget = targets.FirstOrDefault(target =>
        {
            var targetCols = target.GetComponents<BoxCollider2D>();

            if (targetCols.Length == 1)
            {
                Debug.Log($"【{target.name}】はコライダーが1個（タワー想定）");
            }
            else if (targetCols.Length >= 2)
            {
                Debug.Log($"【{target.name}】はコライダーが2個以上（通常敵）");
            }
            else
            {
                Debug.LogWarning($"【{target.name}】はコライダーが0個です");
                return false;
            }

            // 喰らい判定として使うコライダーを選ぶ
            BoxCollider2D targetHurtCollider =
                targetCols.Length >= 2 ? targetCols[0] :
                targetCols.Length == 1 ? targetCols[0] : null;

            if (targetHurtCollider == null)
            {
                Debug.LogWarning($"{target.name}: 有効な喰らい判定が見つかりませんでした");
                return false;
            }

            // 実際に交差しているかを確認
            bool intersects = attackCollider.bounds.Intersects(targetHurtCollider.bounds);

            Debug.Log($"→ {gameObject.name} と {target.name} の交差判定結果: {intersects}");

            if (intersects)
            {
                Debug.Log($"★ {gameObject.name}: {target.name} に攻撃可能（交差成功）");
                enemyCollider = targetHurtCollider;
            }

            return intersects;
        });

        if (nearTarget != null && enemyCollider != null)
        {
            float selfEdge, enemyEdge;
            float scaleX = transform.localScale.x;

            if (scaleX > 0f)
            {
                selfEdge = attackCollider.bounds.min.x;
                enemyEdge = enemyCollider.bounds.min.x;
            }
            else if (scaleX < 0f)
            {
                selfEdge = -attackCollider.bounds.max.x;
                enemyEdge = -enemyCollider.bounds.max.x;
            }
            else
            {
                Debug.LogWarning($"{gameObject.name}: スケールXが0です。処理を中止します");
                return;
            }

            frontEdgeDistance = enemyEdge - selfEdge;
            Debug.Log($"◎ 対象: {nearTarget.name}, 距離: {frontEdgeDistance}, isLeader: {isLeader}");

            if (!isLeader)
            {
                Debug.Log("→ 通常キャラ：通常攻撃アニメ");
                animator.SetBool(ANIM_ATTACK, true);
                animator.SetBool(ANIM_LONG, false);
            }
            else
            {
                if (frontEdgeDistance > longAttackDistance)
                {
                    Debug.Log("→ リーダー：長距離攻撃アニメ");
                    animator.SetBool(ANIM_ATTACK, false);
                    animator.SetBool(ANIM_LONG, true);
                }
                else
                {
                    Debug.Log("→ リーダー：通常攻撃アニメ");
                    animator.SetBool(ANIM_ATTACK, true);
                    animator.SetBool(ANIM_LONG, false);
                }
            }
        }
        else
        {
            Debug.LogWarning($"【暴発防止】{gameObject.name}: 攻撃対象が見つかりませんでした（nearTarget: {nearTarget}, enemyCollider: {enemyCollider}）");
            animator.SetBool(ANIM_ATTACK, false);
            animator.SetBool(ANIM_LONG, false);
        }
    }



    public void EndNomalAction()
    {
        if (isLeader)
        {
            animator.SetBool(ANIM_LONG, false);
        }
        animator.SetBool(ANIM_ATTACK, false);
        canState = true;
    }

    public void InflictDamage(int type = 0)
    {
        float ratio = type switch
        {
            STATE_SKILL when hasSkill => skillRatio,
            STATE_SPECIAL when hasSpecial => specialRatio,
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
    }

    private void InflictDamageAsLeader(float ratio)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(ANIM_ATTACK))
        {
            Debug.Log($"【近距離攻撃】{gameObject.name}: 攻撃開始");
            HandleMeleeAttack();
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("LongAttack"))
        {
            foreach (var target in confirmedTargets)
            {
                target.GetComponent<IDamage>().Damage(atkPower * ratio, atkKB);
                break;
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName(ANIM_SKILL))
        {
            foreach (var target in confirmedTargets)
            {
                target.GetComponent<IDamage>().Damage(atkPower * ratio, atkKB);
            }
        }
        confirmedTargets.Clear();
    }

    private void InflictDamageAsNonLeader(float ratio)
    {
        foreach (var target in confirmedTargets)
        {
            target.GetComponent<IDamage>().Damage(atkPower * ratio, atkKB);
            if (attackType == AttackType.Single) break;
        }
        confirmedTargets.Clear();
    }

    private IEnumerator SkillCoolTimeCount()
    {
        if (!canSkillCoolTime) yield break;
        canSkillCoolTime = false;
        var wait = new WaitForSeconds(COOLTIME_INTERVAL);
        SkillCoolTime = skillCoolDown;
        while (SkillCoolTime > 0)
        {
            yield return wait;
            SkillCoolTime--;
        }
        canSkillCoolTime = true;
    }

    private IEnumerator SpecialCoolTimeCount()
    {
        if (!canSpecialCoolTime) yield break;
        canSpecialCoolTime = false;
        var wait = new WaitForSeconds(COOLTIME_INTERVAL);
        while (specialCoolTime > 0)
        {
            yield return wait;
            specialCoolTime--;
        }
        canSpecialCoolTime = true;
    }

    private IEnumerator KnockBack()
    {
        // すでにノックバック中なら処理を無効化
        if (isKnockBack) yield break;
        // ノックバック状態を開始
        isKnockBack = true; 
        // ステートを一時的に無効にする
        canState = false;

        // ノックバックアニメーションを再生する
        animator.SetBool(ANIM_KNOCKBACK, true);

        // キャラクターの種類に応じて、ノックバックの方向を決定する
        float direction = characterType == CharacterType.Buddy ? -1 : 1;

        // 経過時間を初期化
        float elapsedTime = 0f;

        // ノックバックが指定された期間続くまでループする
        while (elapsedTime < KNOCKBACK_DURATION)
        {
            // 経過時間を更新
            elapsedTime += Time.deltaTime;

            // ノックバックの進行度（0から1までの値）を計算
            float t = elapsedTime / KNOCKBACK_DURATION;

            // X座標に対してノックバックの力を加える
            float x = transform.position.x + direction * KNOCKBACK_FORCE * Time.deltaTime;

            // Y座標にジャンプの高さを反映（正弦波で上下移動を表現）
            float y = originalY + KNOCKBACK_JUMP_HEIGHT * Mathf.Sin(t * Mathf.PI);

            // xの値が範囲外の場合は、xの値を元の位置に戻す
            if (IsOutOfBounds(x))
            {
                x = transform.position.x;
            }
            
            transform.position = new Vector2(x, y);

            // 1フレーム待機して次の更新に進む
            yield return null;
        }
        // ノックバックが終了したことを示す
        isKnockBack = false; // ノックバック状態を解除
        // ノックバックが終わった後、Y座標を元の位置に戻す
        transform.position = new Vector2(transform.position.x, originalY);
    }

    public void EndKnockBack()
    {
        animator.SetBool(ANIM_KNOCKBACK, false);
        animator.SetBool(ANIM_ATTACK, false);
        canState = true;
    }

    private void Death()
    {
        canState = false;
        animator.SetTrigger(ANIM_DEATH);
        
        // 死亡時のコールバックを呼び出し
        OnDeath?.Invoke(this);
    }

    public void EndDeath()
    {
        gameObject.SetActive(false);
    }

    public void Damage(float atkPower = 0, float atkKB = 0)
    {
        Hp -= atkPower;
        SEManager.Instance.PlaySE(normalAttackDamageSounds);
        if (Hp <= 0)
        {
            Death();
        }
        else if (!isKnockBack && ((KNOCKBACK_BASE_VALUE + atkKB - defKB) / KNOCKBACK_DIVIDER * UnityEngine.Random.value > 1 || atkKB.Equals(Mathf.Infinity)))
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
        maxHp = Resources.Load<CharacterInfo>($"{CHARACTER_INFO_PATH}{characterId}").status[level].hp;
    }

    private void ResetTargets()
    {
        targets.Clear();
    }
    private void OnTriggerStay2D(Collider2D t)
    {
        if (IsLongRangeTrigger(t) || !IsValidTarget(t)) return;
        
        if (!targets.Contains(t.gameObject))
        {
            targets.Add(t.gameObject);
            SortTargetsByDistance();
        }
    }

    private void OnTriggerExit2D(Collider2D t) 
    {
        if (IsLongRangeTrigger(t) || !IsValidTarget(t)) return;

        if (targets.Contains(t.gameObject))
        {
            targets.Remove(t.gameObject);
            SortTargetsByDistance(); 
        }
    }

    private bool IsValidTarget(Collider2D t)
    {
        return (characterType == CharacterType.Buddy && t.CompareTag("Enemy")) || 
                (characterType == CharacterType.Enemy && t.CompareTag("Buddy"));
    }

    private void SortTargetsByDistance()
    {
        var myPos = GetComponent<RectTransform>().anchoredPosition.x;
        targets = targets.OrderBy(n => Mathf.Abs(n.GetComponent<RectTransform>().anchoredPosition.x - myPos)).ToList();
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

    /// <summary>
    /// キャラクターIDを取得するプロパティ
    /// </summary>
    public CharacterId GetCharacterId()
    {
        return characterId;
    }

    private void HandleMeleeAttack()
    {
        Debug.Log($"【近距離攻撃】{gameObject.name}: 攻撃範囲: {meleeAttackRange}");
        var hits = Physics2D.OverlapCircleAll(transform.position, meleeAttackRange);
        Debug.Log($"【近距離攻撃】{gameObject.name}: 検出されたオブジェクト数: {hits.Length}");
        
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Debug.Log($"【近距離攻撃】{gameObject.name}: 敵を検出: {hit.gameObject.name}");
                hit.GetComponent<IDamage>().Damage(atkPower, atkKB);
            }
        }
    }
}