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
        private State state;
        //状態
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

            state = State.Walk;
        }
        private void Update()
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
            if (!isMove) return;
            this.transform.position = new Vector2(this.transform.position.x - speed, 400);
            //TODO歩きアニメーションに切り替え animator.SetFloat("speed", speed)
        }
        //攻撃のメソッド 
        private void Attack()
        {
            //TODO攻撃アニメーションに切り替え animator.SetFloat("speed", 0)
            //TODO攻撃判定
            //1.攻撃エフェクトを再生 => エフェクトにスクリプトをアタッチ>OnParticleCollisionでDamage()を呼び出す / その際インターフェイス(IDamage)を使用する
            //2.スプライトを生成 => スプライトにスクリプトをアタッチ>OnTriggerEnter2DでDamage()を呼び出す / その際インターフェイス(IDamage)を使用する
            //TODOサウンドエフェクトを再生
        }
        private void Death()
        {
            //死亡処理
        }
        //ダメージ処理
        public void Damage(int damage)
        {
            Hp -= damage;
            //TODOUpdateUI();
            //TODOif (Hp == 0) リザルトコントローラーにアクセスして敗北パネルを表示する
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
