using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// プレイヤーキャラクターのステータス & 行動処理 > Walk or Attack
    /// </summary>
    public class Player : MonoBehaviour
    {
        [SerializeField]
        [Header("プレイヤーキャラクター")]
        private CharacterId characterId;

        private int maxHp;

        private float maxSpeed;
        private float attackPower;
        private float attackRange;

        private Animator animator;

        //移動可能かどうか
        private bool isMove = true;

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
            Death,
            
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
            isMove = true;
        }
        private void Start()
        {
            var playerCharacterId = ((int)CharacterId.a).ToString("00");
            //maxHp読み込み
            maxHp = Resources.Load<PlayerInfo>($"DataBase/Data/PlayerInfo/{playerCharacterId}").MaxHp;
            Hp = maxHp;
            //maxSpeed読み込み
            maxSpeed = Resources.Load<PlayerInfo>($"DataBase/Data/PlayerInfo/{playerCharacterId}").MaxSpeed;
            Speed = maxSpeed;
            //attackPower読み込み
            attackPower = Resources.Load<PlayerInfo>($"DataBase/Data/PlayerInfo/{playerCharacterId}").AttackPower;
            //attackRange読み込み
            attackRange = Resources.Load<PlayerInfo>($"DataBase/Data/PlayerInfo/{playerCharacterId}").AttackRange;
            //animator読み込み
            animator = Resources.Load<PlayerInfo>($"DataBase/Data/PlayerInfo/{playerCharacterId}").Aimator;
        }
        private void FixedUpdate()
        {
            switch (state)
            {
                case State.Walk:
                    Walk();
                    break;
                case State.Attack:
                    Attack();
                    break;
                case State.Death:
                    Death();
                    break;
                default:
                    Walk();
                    break;
            }
        }
        //歩きメソッド
        private void Walk()
        {
            Debug.Log("プレイヤー: 歩く");
            if (!isMove) return;
            this.transform.position = new Vector2(this.transform.position.x - speed, 400);
            //TODO歩きアニメーションに切り替え animator.SetFloat("speed", speed)
        }
        //攻撃のメソッド //コルーチン処理にする
        private void Attack()
        {
            Debug.Log("プレイヤー: 攻撃");
            //TODO攻撃アニメーションに切り替え animator.SetFloat("speed", 0)
            //TODO攻撃判定 スプライトを生成 => スプライトにスクリプトをアタッチ>OnTriggerStay2DでDamage()を呼び出す / その際インターフェイス(IDamage)を使用する
            //TODOサウンドエフェクトを再生
        }

        //死亡のメソッド //TODOコルーチン
        private void Death()
        {
            //死亡処理

        }
        //ダメージ処理
        public void Damage(int attackPower = 0, int kb = 0)
        {
            Debug.Log("プレイヤー : ダメージを受けました");
            //ダメージ計算TODO防御力も計算
            Hp -= attackPower;
            if (Hp == 0)
            {
                state = State.Death;
                //ゲームをストップ
                GameObject.Find("Canvas").GetComponent<BattleController>().GameStop(Dominator.TypeLeader.BuddyLeader);
                //リザルト画面(敗北)を表示
                GameObject.Find("Canvas/Render/PerformancePanel").GetComponent<ResultController>().OnResultPanel(false);
            }
        }
        private void OnTriggerStay2D(Collider2D t)
        {
            if (t.CompareTag("enemy")) state = State.Attack;
        }
        private void OnTriggerExit2D(Collider2D t)
        {
            if (t.CompareTag("enemy")) state = State.Walk;
        }
    }
}
