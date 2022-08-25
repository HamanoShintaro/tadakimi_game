using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// キャラクター > (ステータス管理/歩くor攻撃or死亡orノックバックの処理/回復or被ダメージor強化の処理)をするメソッド
    /// </summary>
    public class CharacterCore : MonoBehaviour, IDamage
    {
        [SerializeField, Multiline(3)]
        private string memo;

        [SerializeField]
        [Header("キャラクターID")]
        private CharacterId characterId;

        [SerializeField]
        [Header("味方or敵")]
        private CharacterType characterType;

        //ステータス
        private float maxHp;
        private float defKB;
        private float maxSpeed;

        //通常攻撃
        private float atkKB;
        private float atkPower;
        private float atkInterval;

        //スキル
        private int skillCost;
        private float skillCoolDown;
        private float skillInterval;
        private bool hasSkill;
        private GameObject skillEffect;
        private RectTransform skillEffectPanel;

        //検知しているターゲットのリスト
        List<GameObject> targets = new List<GameObject>();

        //取得するコンポーネント
        private MagicPowerController magicPowerController;
        private Animator animator;

        //移動可能かどうか
        private bool canMove = true;

        //ステート変更可能かどうか
        private bool canState = true;

        //スキル使用可能かどうか
        private bool canSkill = false;

        //スキルのクールタイムを測れるかどうか
        private bool canSkillCoolTime = true;

        //プレイヤーキャラクターの種類
        private enum CharacterId
        {
            a = 01,
            b = 02,
            c = 03
        }

        //キャラクターの種類=>味方or敵
        private enum CharacterType
        {
            Ally,
            Enemy
        }

        //状態のプロパティ
        private State state;
        private enum State
        {
            Walk,
            Attack,
            KnockBack,
            Death,
            None
        }

        //Hpプロパティ
        private float hp = 0;
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

        private void OnEnable()
        {
            canMove = true;
        }

        private void Start()
        {
            //TODO取得>characterIdから選択できるように変更する
            //var characterId = ((int)CharacterId.a).ToString("00");
            var characterId = "Volcus_01";
            //maxHp取得
            maxHp = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/{characterId}").status[0].hp;
            Hp = maxHp;
            //maxSpeed取得
            maxSpeed = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/{characterId}").status[0].speed / 300;
            Speed = maxSpeed;

            //atkPower取得
            atkPower = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/{characterId}").status[0].attack;

            //atkKB取得
            atkKB = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/{characterId}").status[0].atkKB;

            //defKB取得
            defKB = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/{characterId}").status[0].defKB;

            //スキルのコスト取得
            skillCost = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/volcus_01").skill.cost;

            //スキルクールタイム取得
            skillCoolTime = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/volcus_01").skill.cd;

            //スキルエフェクト取得
            skillCoolTime = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/volcus_01").skill.cd;

            //マジックコントローラー取得
            magicPowerController = GameObject.Find("Canvas/Render/controlPanel/power").GetComponent<MagicPowerController>();

            //アニメーター取得//TODOCharacterInfoから取得するように変更する
            animator = this.GetComponent<Animator>();

            //初めのステートを設定
            state = State.Walk;

            hasSkill = true;//TODOリソースorセーブから読み込む?

            //スキルのクールタイムを測る
            StartCoroutine(Count());
        }

        private void Update()
        {
            //Debug.Log(state);
            //状態の優先順位 => 死亡>ノックバック>攻撃>歩く
            if (!canState) return;
            if (state == State.KnockBack) StartCoroutine(KnockBack());
            else if (state == State.Attack) StartCoroutine(Attack());
            else if (state == State.Walk) Walk();
        }

        /// <summary>
        /// 歩くメソッド
        /// </summary>
        private void Walk()
        {
            //Debug.Log($"{characterType} : {characterId} => 歩く");
            if (!canMove) return;
            if (characterType == CharacterType.Ally) this.transform.position = new Vector2(this.transform.position.x - speed, 400);
            else if (characterType == CharacterType.Enemy) this.transform.position = new Vector2(this.transform.position.x + speed, 400);
        }

        /// <summary>
        /// 攻撃するメソッド
        /// </summary>
        private IEnumerator Attack()
        {
            if (!canState) yield break;
            canState = false;
            if (hasSkill && skillCost <= magicPowerController.magicPower && SkillCoolTime == 0)//TODOスキルクールタイム計算メソッド
            {
                Debug.Log($"{characterType} : {characterId} => スキル");

                //スキル処理>開始
                animator.SetBool("Skill", true);

                //エフェクトを生成 TODO=>インターフェイスで行う & 待機時間のマジックナンバーを修正する
                yield return new WaitForSeconds(3);

                var ef = Instantiate(skillEffect, skillEffectPanel.transform);
                //ef.position = new Vector2()
                yield return new WaitForSeconds(skillInterval);//TODOリソースから取得する

                //スキル処理>終了
                animator.SetBool("Skill", false);
                //スキルのクールタイムを測る
                StartCoroutine(Count());
            }
            /*
            else if (奥義の条件)
            {
                //奥義処理>開始
                yield return new WaitForSeconds(n);

                //エフェクトを生成
                yield return new WaitForSeconds(m);

                //奥義処理>終了
                //奥義のクールタイム測る?
            }
             */
            else
            {
                Debug.Log($"{characterType} : {characterId} => 通常攻撃");

                //通常攻撃の処理>開始
                animator.SetBool("Attack", true);
                //ターゲットをリセット
                ResetTargets();
                yield return null;
                //ダメージ処理
                try
                {
                    foreach (GameObject target in targets)
                    {
                        target.GetComponent<IDamage>().Damage(atkPower, atkKB);
                    }
                }
                catch
                {

                }
                //TODOサウンドエフェクトを再生
                yield return new WaitForSeconds(2f);//TODOマジックナンバー リソースのインターバルから取得する

                //通常攻撃の処理>終了
                animator.SetBool("Attack", false);
            }
            canState = true;
        }

        /// <summary>
        ///  スキルのクールタイムを測るメソッド
        /// </summary>
        /// <returns></returns>
        private IEnumerator Count()
        {
            if (!canSkillCoolTime) yield break;
            canSkillCoolTime = false;
            while (true)
            {
                yield return new WaitForSeconds(1);
                SkillCoolTime--;
                Debug.Log($"{SkillCoolTime}");
                if (SkillCoolTime == 0) break;
            }
            canSkillCoolTime = true;
        }

        /// <summary>
        /// ノックバックをするメソッド
        /// </summary>
        private IEnumerator KnockBack()
        {
            if (!canState) yield break;
            canState = false;
            Debug.Log($"{characterType} : {characterId} => ノックバック");

            //ノックバック処理>開始 TODOノックバック処理
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
            //if (!canState) yield break;
            //canState = false;
            Debug.Log($"{characterType} : {characterId} => 死亡");
            //死亡処理>開始　TODO死亡処理
            animator.SetBool("Death", true);
            yield return new WaitForSeconds(3);

            //死亡処理>終了
            this.gameObject.SetActive(false);

            //canState = true;
        }
        /// <summary>
        /// ダメージを受けたときのメソッド(インターフェイス)
        /// </summary>
        /// <param name="atkPower">攻撃力</param>
        public void Damage(float atkPower = 0, float atkkb = 0)
        {
            //Debug.Log($"{characterType} : {characterId} => 被ダメージ / Hpは{Hp}");
            //ダメージ計算TODO防御力も計算
            Hp -= atkPower;
            if (Hp <= 0)
            {
                //死亡処理
                canState = false;
                StartCoroutine(Death());
            }
            else
            {
                if ((atkkb - defKB) * Random.value > 1) state = State.KnockBack;
            }
        }

        /// <summary>
        /// 回復するメソッド
        /// </summary>
        /// <param name="heal"></param>
        public void Recover(int heal)
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
            maxHp = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/{characterId}").status[0].hp;//TODO現在のレベルを読み込む
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
            if (characterType == CharacterType.Ally && t.CompareTag("enemy") || characterType == CharacterType.Enemy && t.CompareTag("player"))
            {
                state = State.Attack;
                if (!targets.Contains(t.gameObject)) targets.Add(t.gameObject);
                //Debug.Log("攻撃範囲にターゲットがいます");
            }
        }

        private void OnTriggerExit2D(Collider2D t)
        {
            if (t.isTrigger == true) return;
            if (characterType == CharacterType.Ally && t.CompareTag("enemy") || characterType == CharacterType.Enemy && t.CompareTag("player"))
            {
                if (targets.Contains(t.gameObject))
                {
                    targets.Remove(t.gameObject);
                    if (targets.Count == 0) state = State.Walk;
                }
            }
        }
    }
}
