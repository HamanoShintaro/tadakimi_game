using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// キャラクター > (ステータス管理/歩くor攻撃or死亡orノックバックの処理/回復or被ダメージor強化の処理)をするメソッド
    /// </summary>
    public class CharacterCore : MonoBehaviour, IDamage, IRecovery, ITemporaryEnhance
    {
        #region 変数
        [SerializeField]
        [Tooltip("キャラクターID")]
        private CharacterId characterId;

        [SerializeField]
        [Tooltip("味方/敵")]
        private CharacterType characterType = CharacterType.Buddy;

        [SerializeField]
        [Tooltip("攻撃範囲の種類")]
        private AttackType attackType = AttackType.single;

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

        //ステータス
        [Tooltip("現在のレベル")]
        [HideInInspector]
        public int level = 0;

        private float maxHp;
        private float defKB;
        private float maxSpeed;

        //通常攻撃
        private float atkKB;
        [HideInInspector]
        public float atkPower;

        //スキル
        private int skillCost;
        private float skillCoolDown;
        private bool hasSkill;
        private float skillRatio;

        //奥義
        private int specialCost;
        private float specialCoolTime;
        private bool hasSpecial;
        private float specialRatio;

        //検知しているターゲットのリスト
        public List<GameObject> targets = new List<GameObject>();

        //取得するコンポーネント
        private MagicPowerController magicPowerController;
        private Animator animator;

        //ステート変更可能かどうか
        public bool canState = true;

        //スキルのクールタイムを測れるかどうか
        private bool canSkillCoolTime = true;

        //スペシャルのクールタイムを測れるかどうか
        private bool canSpecialCoolTime = true;

        private GameObject characterPanel;

        private AudioSource audioSource;

        // 通常攻撃のダメージ音のリスト
        [SerializeField]
        private List<AudioClip> normalAttackDamageSounds;

        private Player player;

        //プレイヤーキャラクターの種類
        public enum CharacterId
        {
            Volcus_01, Volcus_02, Era_01, Eleth_01, Orend_01, Sara_01, Shandy_01, Loxy_01,
            Npc_01, Npc_02, Npc_03, Npc_04, Npc_05, Npc_06, Npc_07, Npc_08, Npc_09, Npc_10, Npc_11, Npc_12, Npc_13, Npc_14,
            Summon_01, Summon_02, Summon_03, Summon_04,
            Enemy_01,Enemy_02,Enemy_03,Enemy_04,Enemy_05,Enemy_06,Enemy_07,Enemy_08,Enemy_09,Enemy_10,Enemy_11,Enemy_12,Enemy_13,Enemy_14,Enemy_15,Enemy_16,Enemy_17,Enemy_18,Enemy_19,Enemy_20, Enemy_21, Enemy_22, Enemy_23,
            Enemy_24, Enemy_25, Enemy_26, Enemy_27, Enemy_28, Enemy_29, Enemy_30, Enemy_31, Enemy_32, Enemy_33,
            Boss_01, Boss_02, Boss_03, Boss_04, Boss_05, Boss_06,
        }

        //キャラクターの種類=>味方or敵
        private enum CharacterType
        {
            Buddy,
            Enemy
        }

        //キャラクターの役割
        private enum CharacterRole
        {
            Attacker,
            Supporter
        }

        //状態のプロパティ
        //private State state;
        private enum State
        {
            Walk,
            Action,
            KnockBack,
            Death,
            StandBy,
            None
        }

        private enum AttackType
        {
            single,
            range
        }

        //Hpプロパティ
        public float hp = 100;
        public float Hp
        {
            get { return hp; }
            set
            {
                hp = value;
                if (hp < 0) hp = 0;
                else if (hp > maxHp) hp = maxHp;
            }
        }

        //Speedプロパティ
        private float speed = 0;
        public float Speed
        {
            get { return speed; }
            set
            {
                speed = value;
                if (speed < 0) speed = 0;
                else if (speed > maxSpeed) speed = maxSpeed;
            }
        }

        //スキルのクールタイム
        private float skillCoolTime;
        public float SkillCoolTime
        {
            get { return skillCoolTime; }
            set
            {
                skillCoolTime = value;
                if (skillCoolTime < 0) skillCoolTime = 0;
                else if (skillCoolTime > skillCoolDown) skillCoolTime = skillCoolDown;
            }
        }
        #endregion
        #region 初期設定
        private void Start()
        {
            if (characterType == CharacterType.Buddy)
            {
                //セーブデータを生成
                SaveController saveController = new SaveController();
                //セーブデータを取得
                saveController.characterSave.Load();
                //characterIdのセーブデータのレベルを取得
                try
                {
                    level = saveController.characterSave.list.Find(characterSave => characterSave.id == characterId.ToString()).level;
                }
                catch
                {
                    level = 1;
                }
            }
            characterPanel = transform.parent.gameObject;
            var characterInfo = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/{characterId}");

            if (characterInfo == null)
            {
                Debug.LogError($"{characterId} : データベースにキャラクターのデータがありません");
                return;
            }
            //maxHp取得
            maxHp = characterInfo.status[level].hp;
            Hp = maxHp;

            //maxSpeed取得
            maxSpeed = characterInfo.status[level].speed / 20;
            Speed = maxSpeed;

            //atkPower取得
            atkPower = characterInfo.status[level].attack;

            //atkKB取得
            atkKB = characterInfo.status[level].atkKB;

            //defKB取得
            defKB = characterInfo.status[level].defKB;

            //スキルのコスト取得
            skillCost = characterInfo.skill.cost;

            //スキルクールタイム取得
            skillCoolDown = characterInfo.skill.cd;

            //スキルレート
            skillRatio = characterInfo.skill.Ratio;

            //スペシャルコスト
            specialCost = characterInfo.skill.cost;

            //スペシャルクールタイム
            specialCoolTime = characterInfo.special.cd;

            //スペシャルレート
            specialRatio = characterInfo.special.Ratio;

            //マジックコントローラー取得
            magicPowerController = GameObject.Find("Canvas_Dynamic/[ControlPanel]/Power").GetComponent<MagicPowerController>();

            //アニメーター取得
            animator = GetComponent<Animator>();

            //スキル名がないならhasSkill = false
            if (characterInfo.skill.name == "9999" || characterInfo.skill.name == null || characterInfo.skill.name == "") hasSkill = false;
            else hasSkill = true;

            //スペシャル名がないならhasSpecial = false
            if (characterInfo.special.name == "9999" || characterInfo.special.name == null || characterInfo.skill.name == "") hasSpecial = false;
            else hasSpecial = true;

            //スキルのクールタイムを測る
            StartCoroutine(SkillCoolTimeCount());

            audioSource = GetComponent<AudioSource>();

            player = GetComponent<Player>();
        }
        #endregion
        private void FixedUpdate()
        {
            //状態の優先順位は死亡>ノックバック>状態遷移可能かどうか>歩くorアクション
            if (isLeader)
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
                    // 敵がいない
                }
                else
                {
                    Action();
                }
            }
            else
            {
                if (!canState) return;
                //ターゲットが居ない=>歩く
                if (targets.Count == 0)
                {
                    Walk();
                }
                //ターゲットが居る=>アクション
                else
                {
                    animator.SetBool("Walk", false);
                    Action();
                }
            }
        }

        /// <summary>
        /// 歩くメソッド
        /// </summary>
        private void Walk()
        {
            if (isLeader) return;
            animator.SetBool("Walk", true);
            if (characterType.Equals(CharacterType.Buddy)) transform.position = new Vector2(transform.position.x + speed, transform.position.y);
            else transform.position = new Vector2(transform.position.x - speed, transform.position.y);
        }

        private void EndWalk()
        {
            animator.SetBool("Walk", false);
        }

        /// <summary>
        /// 攻撃するメソッド
        /// </summary>
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
                NomalAction();
            }
        }

        private void SpecialAction()
        {
            magicPowerController.magicPower -= specialCost;

            var index = characterPanel.transform.childCount - 1;
            transform.SetSiblingIndex(index);

            animator.SetBool("Special", true);
        }

        public void EndSpecialAction()
        {
            animator.SetBool("Special", false);

            StartCoroutine(SpecialCoolTimeCounto());
            canState = true;
        }

        private void SkillAction()
        {
            magicPowerController.magicPower -= skillCost;

            var index = characterPanel.transform.childCount - 1;
            transform.SetSiblingIndex(index);

            animator.SetBool("Skill", true);
        }

        public void EndSkillAction()
        {
            animator.SetBool("Skill", false);

            StartCoroutine(SkillCoolTimeCount());
            canState = true;
        }

        private void NomalAction()
        {
            if (isLeader)
            {
                //近いターゲットを割り出す
                var nearTarget = targets.OrderBy(n => n.GetComponent<RectTransform>().anchoredPosition.x).ToList()[0];
                var distance = Vector2.Distance(nearTarget.GetComponent<RectTransform>().anchoredPosition, GetComponent<RectTransform>().anchoredPosition);
                //遠距離攻撃をする最小の距離
                if (distance > longAttackDistance)
                {
                    animator.SetBool("Long", true);
                }
            }
            animator.SetBool("Attack", true);
        }

        public void EndNomalAction()
        {
            if (isLeader)
            {
                animator.SetBool("Long", false);
                animator.SetBool("Attack", false);
            }
            else
            {
                animator.SetBool("Attack", false);
            }
            canState = true;
        }

        /// <summary>
        /// targetにダメージを与える
        /// </summary>
        /// <param name="type">0:通常攻撃 / 1: スキル攻撃 / 2:スペシャル攻撃</param>
        public void InflictDamage(int type = 0)
        {
            float ratio = 1.0f;

            if (type == 0)
            {
                // 通常攻撃の場合の処理
                ratio = 1.0f;
            }
            else if (type == 1)
            {
                // スキル攻撃の場合の処理
                if (hasSkill) ratio = skillRatio;
            }
            else if (type == 2)
            {
                // スペシャル攻撃の場合の処理
                if (hasSpecial) ratio = specialRatio;
            }

            //ダメージ処理
            try
            {
                foreach (GameObject target in targets)
                {
                    target.GetComponent<IDamage>().Damage(atkPower * ratio, atkKB);
                    if (attackType == AttackType.single) break;
                }
            }
            catch
            {
            }
            //ターゲットをリセット
            ResetTargets();
        }

        /// <summary>
        ///  スキルのクールタイムを測るメソッド
        /// </summary>
        /// <returns></returns>
        private IEnumerator SkillCoolTimeCount()
        {
            if (!canSkillCoolTime) yield break;
            canSkillCoolTime = false;
            var wait = new WaitForSeconds(1);
            skillCoolTime = skillCoolDown;
            while (true)
            {
                yield return wait;
                SkillCoolTime--;
                if (SkillCoolTime == 0) break;
            }
            canSkillCoolTime = true;
        }

        /// <summary>
        ///  スペシャルのクールタイムを測るメソッド
        /// </summary>
        /// <returns></returns>
        private IEnumerator SpecialCoolTimeCounto()
        {
            if (!canSpecialCoolTime) yield break;
            canSpecialCoolTime = false;
            var wait = new WaitForSeconds(1);
            while (true)
            {
                yield return wait;
                specialCoolTime--;
                if (specialCoolTime == 0) break;
            }
            canSpecialCoolTime = true;
        }

        /// <summary>
        /// ノックバックをするメソッド
        /// </summary>
        private IEnumerator KnockBack()
        {
            canState = false;
            animator.SetBool("KnockBack", true);
            var wait = new WaitForSeconds(0.1f);
            for (int i = 0; i < 10; i++)
            {
                yield return wait;
                if (characterType.Equals(CharacterType.Buddy)) transform.position = new Vector2(transform.position.x - 3, transform.position.y);
                else transform.position = new Vector2(transform.position.x + 3, transform.position.y);
            }
        }

        public void EndKnockBack()
        {
            //ノックバック処理>終了
            animator.SetBool("KnockBack", false);
            animator.SetBool("Attack", false);
            canState = true;
        }

        /// <summary>
        /// 死亡するメソッド
        /// </summary>
        private void Death()
        {
            //死亡処理>開始
            canState = false;
            animator.SetTrigger("Death");
        }

        public void EndDeath()
        {
            //死亡処理>終了
            gameObject.SetActive(false);
        }

        /// <summary>
        /// ダメージを受けたときのメソッド(インターフェイス)
        /// </summary>
        /// <param name="atkPower">攻撃力</param>
        public void Damage(float atkPower = 0, float atkKB = 0)
        {
            Hp -= atkPower;
            audioSource.PlayOneShot(normalAttackDamageSounds[0]);
            if (Hp <= 0)
            {
                Death();
            }
            else if ((atkKB - defKB) * Random.value > 1 || atkKB.Equals(Mathf.Infinity))
            {
                StartCoroutine(KnockBack());
            }
        }

        /// <summary>
        /// 回復するメソッド
        /// </summary>
        /// <param name="heal"></param>
        public void Recovery(int heal)
        {
            Hp += heal;
        }

        /// <summary>
        /// 一時的に強化するメソッド
        /// </summary>
        /// <param name="addSpeed">スピード</param>
        /// <param name="addMaxHp">最大HP</param>
        public IEnumerator TemporaryEnhance(float duration = 0.0f, int addSpeed = 0, int addMaxHp = 0)
        {
            Speed += addSpeed;
            maxHp += addMaxHp;
            yield return new WaitForSeconds(duration);
            Speed = speed;
            maxHp = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/{characterId}").status[level].hp;
        }

        /// <summary>
        /// ターゲットリストをリセット(空に)するメソッド
        /// </summary>
        private void ResetTargets()
        {
            targets.Clear();
        }

        private void OnTriggerStay2D(Collider2D t)
        {
            if (t.isTrigger == true) return;
            if (characterRole == CharacterRole.Attacker)
            {
                if (characterType == CharacterType.Buddy && t.CompareTag("Enemy") || characterType == CharacterType.Enemy && t.CompareTag("Buddy"))
                {
                    if (!targets.Contains(t.gameObject)) targets.Add(t.gameObject);
                    targets.OrderBy(n => n.GetComponent<RectTransform>().anchoredPosition.x);
                }
            }
            else if (characterRole == CharacterRole.Supporter)
            {
                if (characterType == CharacterType.Buddy && t.CompareTag("Buddy") || characterType == CharacterType.Enemy && t.CompareTag("Enemy"))
                {
                    if (!targets.Contains(t.gameObject)) targets.Add(t.gameObject);
                    targets.OrderBy(n => n.GetComponent<RectTransform>().anchoredPosition.x);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D t)
        {
            if (t.isTrigger == true) return;
            if (characterRole.Equals(CharacterRole.Attacker))
            {
                if (characterType == CharacterType.Buddy && t.CompareTag("Enemy") || characterType == CharacterType.Enemy && t.CompareTag("Buddy"))
                {
                    if (targets.Contains(t.gameObject)) targets.Remove(t.gameObject);
                    targets.OrderBy(n => n.GetComponent<RectTransform>().anchoredPosition.x);
                }
            }
            else if (characterRole.Equals(CharacterRole.Supporter))
            {
                if (characterType == CharacterType.Buddy && t.CompareTag("Buddy") || characterType == CharacterType.Enemy && t.CompareTag("Enemy"))
                {
                    if (!targets.Contains(t.gameObject)) targets.Remove(t.gameObject);
                    targets.OrderBy(n => n.GetComponent<RectTransform>().anchoredPosition.x);
                }
            }
        }
    }
}



