using System.Collections;
using System.Collections.Generic;
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

        [HideInInspector]
        private Player player;

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
        private float atkInterval;

        //スキル
        private int skillCost;
        private float skillCoolDown;
        private float skillInterval;
        private bool hasSkill;
        private float skillRatio;
        //private GameObject skillEffect;
        //private RectTransform skillEffectPanel;

        //奥義
        private int specialCost;
        private float specialCoolTime;
        private float specialInterval;
        private bool hasSpecial;
        private float specialRatio;

        //検知しているターゲットのリスト
        public List<GameObject> targets = new List<GameObject>();

        //取得するコンポーネント
        private MagicPowerController magicPowerController;
        private Animator animator;

        //ステート変更可能かどうか
        private bool canState = true;

        //スキル使用可能かどうか
        private bool canSkill = false;

        //スキルのクールタイムを測れるかどうか
        private bool canSkillCoolTime = true;

        //スペシャルのクールタイムを測れるかどうか
        private bool canSpecialCoolTime = true;

        //プレイヤーキャラクターの種類
        public enum CharacterId
        {
            Volcus_01,
            Volcus_02,
            Era_01,
            Eleth_01,
            Orend_01,
            Sara_01,
            Shandy_01,
            Loxy_01,
            Soldier_01,
            Collobo_01,
            Vivien_01
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
        private State state;
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
        private float hp = 100;
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
        private void OnEnable()
        {
        }

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
            var characterInfo = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/{characterId}");
            //maxHp取得
            maxHp = characterInfo.status[level].hp;
            Hp = maxHp;

            //maxSpeed取得
            maxSpeed = characterInfo.status[level].speed / 300;
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
            skillCoolTime = characterInfo.skill.cd;

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

            //初めのステートを設定
            state = State.Walk;

            //スキル名がないならhasSkill = false
            if (characterInfo.skill.name == "" || characterInfo.skill.name == null) hasSkill = false;
            else hasSkill = true;

            //スペシャル名がないならhasSpecial = false
            if (characterInfo.special.name == "" || characterInfo.special.name == null) hasSpecial = false;
            else hasSpecial = true;

            try
            {
                player = GameObject.Find("Player").GetComponent<Player>();
            }
            catch
            {
            }
            //スキルのクールタイムを測る
            StartCoroutine(SkillCoolTimeCount());
        }
        #endregion
        private void FixedUpdate()
        {
            //状態の優先順位は死亡>ノックバック>状態遷移可能かどうか>歩くorアクション
            //ノックバック処理
            if (state == State.KnockBack) StartCoroutine(KnockBack());
            if (!canState) return;
            if (isLeader && player.isMove)
            {
                //animator.SetBool("Walk", true);
                return;
            }
            //ターゲットが居ない=>歩く
            if (targets.Count == 0)
            {
                Walk();
            }
            //ターゲットが居る=>アクション
            else
            {
                Action();
            }
        }

        /// <summary>
        /// 歩くメソッド
        /// </summary>
        private void Walk()
        {
            if (isLeader) return;
            if (characterType == CharacterType.Buddy) transform.position = new Vector2(transform.position.x + speed, transform.position.y);
            else if (characterType == CharacterType.Enemy) transform.position = new Vector2(transform.position.x - speed, transform.position.y);
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
            //奥義処理>開始
            animator.SetBool("Special", true);
        }

        public void EndSpecialAction()
        {
            //奥義処理>終了
            animator.SetBool("Special", false);

            //奥義のクールタイム測る
            StartCoroutine(SpecialCoolTimeCounto());

            canState = true;
        }

        private void SkillAction()
        {
            //スキル処理>開始
            animator.SetBool("Skill", true);
        }

        public void EndASkillAction()
        {
            //スキル処理>終了
            animator.SetBool("Skill", false);

            //スキルのクールタイムを測る
            StartCoroutine(SkillCoolTimeCount());

            canState = true;
        }

        private void NomalAction()
        {
            //通常攻撃の処理>開始
            animator.SetBool("Attack", true);
        }

        public void EndNomalAction()
        {
            //通常攻撃の処理>終了
            animator.SetBool("Attack", false);

            canState = true;
        }

        /// <summary>
        /// targetにダメージを与える
        /// </summary>
        /// <param name="type">0:通常攻撃 / 1: スキル攻撃 / 2:スペシャル攻撃</param>
        public void InflictDamage(int type)
        {
            float ratio = 1f;
            if (type == 1 && hasSkill) ratio = skillRatio;
            else if (type == 2 && hasSpecial) ratio = specialRatio;

            //ダメージ処理
            try
            {
                foreach (GameObject target in targets)
                {
                    //攻撃力をスキルと奥義で分ける
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

        public void GiveRecovery()
        {
            try
            {
                foreach (GameObject target in targets)
                {
                    target.GetComponent<IRecovery>().Recovery(2);
                }
            }
            catch
            {
            }
        }

        public void GiveTemporaryEnhance()
        {
            try
            {
                foreach (GameObject target in targets)
                {
                    target.GetComponent<ITemporaryEnhance>().TemporaryEnhance(5,5,5);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        ///  スキルのクールタイムを測るメソッド
        /// </summary>
        /// <returns></returns>
        private IEnumerator SkillCoolTimeCount()
        {
            if (!canSkillCoolTime) yield break;
            canSkillCoolTime = false;
            while (true)
            {
                yield return new WaitForSeconds(1);
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
            while (true)
            {
                yield return new WaitForSeconds(1);
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
            if (!canState) yield break;
            canState = false;

            //ノックバック処理>開始
            yield return new WaitForSeconds(1);

            //ノックバック処理>終了
            animator.SetBool("KnockBack", false);

            canState = true;
        }

        /// <summary>
        /// 死亡するメソッド
        /// </summary>
        private IEnumerator Death()
        {
            canState = false;
            //死亡処理>開始
            animator.SetBool("Death", true);

            yield return new WaitForSeconds(1);

            //死亡処理>終了
            this.gameObject.SetActive(false);
            canState = true;
        }

        /// <summary>
        /// ダメージを受けたときのメソッド(インターフェイス)
        /// </summary>
        /// <param name="atkPower">攻撃力</param>
        public void Damage(float atkPower = 0, float atkKB = 0)
        {
            Hp -= atkPower;
            if (Hp <= 0)
            {
                //死亡処理
                canState = false;
                StartCoroutine(Death());
            }
            else if ((atkKB - defKB) * Random.value > 1 || atkKB.Equals(Mathf.Infinity))
            {
                state = State.KnockBack;
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
            this.Speed = speed;
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
                    state = State.Action;
                    if (!targets.Contains(t.gameObject)) targets.Add(t.gameObject);
                }
            }
            else if (characterRole == CharacterRole.Supporter)
            {
                if (characterType == CharacterType.Buddy && t.CompareTag("Buddy") || characterType == CharacterType.Enemy && t.CompareTag("Enemy"))
                {
                    state = State.Action;
                    if (!targets.Contains(t.gameObject)) targets.Add(t.gameObject);
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
                }
            }
            else if (characterRole.Equals(CharacterRole.Supporter))
            {
                if (characterType == CharacterType.Buddy && t.CompareTag("Buddy") || characterType == CharacterType.Enemy && t.CompareTag("Enemy"))
                {
                    if (!targets.Contains(t.gameObject)) targets.Remove(t.gameObject);
                }
            }
        }
    }
}
