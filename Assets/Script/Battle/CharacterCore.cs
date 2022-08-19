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
        [SerializeField , Multiline(3)]
        private string memo;

        [SerializeField]
        [Header("キャラクターID")]
        private CharacterId characterId;

        //ステータス
        private int maxHp;
        private int defKB;
        private float maxSpeed;
        private float attackPower;
        private float attackRange;
        private Animator animator;

        //検知している敵キャラ

        //移動可能かどうか
        private bool canMove = true;

        //ステート変更可能かどうか
        private bool canState = true;

        //スキル使用可能かどうか
        private bool canSkill = false;

        //プレイヤーキャラクターの種類
        private enum CharacterId
        {
            a = 01,
            b = 02,
            c = 03
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
        private int hp = 0;
        public int Hp
        {
            get { return hp; }
            set
            {
                if (hp < 0) hp = 0;
                else if (hp > maxHp) hp = maxHp;
                hp = value;
            }
        }
        //Speedプロパティ
        private float speed = 0;
        public float Speed
        {
            get { return speed; }
            set
            {
                if (speed < 0) speed = 0;
                else if (speed > maxSpeed) speed = maxSpeed;
                speed = value;
            }
        }
        private void OnEnable()
        {
            canMove = true;
        }
        private void Start()
        {
            //TODO読み込み部分をカスタムする
            var characterId = ((int)CharacterId.a).ToString("00");
            //maxHp読み込み
            maxHp = Resources.Load<PlayerInfo>($"DataBase/Data/PlayerInfo/{characterId}").MaxHp;
            Hp = maxHp;
            //maxSpeed読み込み
            maxSpeed = Resources.Load<PlayerInfo>($"DataBase/Data/PlayerInfo/{characterId}").MaxSpeed;
            Speed = maxSpeed;
            //attackPower読み込み
            attackPower = Resources.Load<PlayerInfo>($"DataBase/Data/PlayerInfo/{characterId}").AttackPower;
            //attackRange読み込み
            attackRange = Resources.Load<PlayerInfo>($"DataBase/Data/PlayerInfo/{characterId}").AttackRange;
            //animator読み込み
            //animator = Resources.Load<PlayerInfo>($"DataBase/Data/PlayerInfo/{characterId}").Aimator;
            animator = this.GetComponent<Animator>();
            //スキルが使用可能かどうか TODO読み込む
            //canSkill =
            state = State.Walk;
        }
        private void Update()
        {
            Debug.Log(state);
            //状態の優先順位 => 死亡>ノックバック>攻撃>歩く
            if (state == State.Death) Death();
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
            Debug.Log($"{characterId} => 歩く");
            if (!canMove) return;
            this.transform.position = new Vector2(this.transform.position.x - speed, 400);
        }
        /// <summary>
        /// 攻撃するメソッド
        /// </summary>
        private IEnumerator Attack()
        {
            if (!canState) yield break;
            canState = false;
            if (false)
            {
                Debug.Log($"{characterId} => スキル");
                //スキルのインターフェイスを呼ぶ
                this.GetComponent<ISkill>().Skill();
            }
            else
            {
                Debug.Log($"{characterId} => 通常攻撃");
                animator.SetBool("Attack", true);//TODO攻撃判定 スプライトを生成 => スプライトにスクリプトをアタッチ>OnTriggerStay2DでDamage()を呼び出す / その際インターフェイス(IDamage)を使用する
                //TODOサウンドエフェクトを再生
                yield return new WaitForSeconds(2f);
                animator.SetBool("Attack", false);
            }
            canState = true;
        }

        /// <summary>
        /// ノックバックをするメソッド
        /// </summary>
        private IEnumerator KnockBack()
        {
            canState = false;
            if (!canState) yield break;
            Debug.Log($"{characterId} => ノックバック");
            //ノックバック処理
            animator.SetBool("KnockBack", false);
            canState = true;
        }

        /// <summary>
        /// 死亡するメソッド
        /// </summary>
        private void Death()
        {
            canState = false;
            Debug.Log($"{characterId} => 死亡");
            //既に死亡している場合は呼び出さない
            if (state != State.Death) return;
            //死亡処理
            animator.SetBool("Death", true);
            this.gameObject.SetActive(false);
            canState = true;
        }
        /// <summary>
        /// ダメージを受けたときのメソッド(インターフェイス)
        /// </summary>
        /// <param name="attackPower">攻撃力</param>
        public void Damage(int attackPower = 0, int kb = 0)
        {
            Debug.Log($"{characterId} => 被ダメージ");
            //ダメージ計算TODO防御力も計算
            Hp -= attackPower;
            if (Hp == 0)
            {
                state = State.Death;
                //ゲームをストップ
                GameObject.Find("Canvas").GetComponent<BattleController>().GameStop();
                //リザルト画面(敗北)を表示
                GameObject.Find("Canvas/Render/PerformancePanel/loserPanel").SetActive(true);
            }
            else 
            {
                if ((kb - defKB) * Random.value > 1) state = State.KnockBack;
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
            maxHp = Resources.Load<PlayerInfo>($"DataBase/Data/PlayerInfo/{characterId}").MaxHp;
        }

        private void OnTriggerStay2D(Collider2D t)
        {
            if (t.CompareTag("enemy")) state = State.Attack;
            Debug.Log("衝突");
        }

        private void OnTriggerExit2D(Collider2D t)
        {
            if (t.CompareTag("enemy")) state = State.Walk;
        }
    }
}
