using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class movingEnemyPtn001 : MonoBehaviour
{
    public float movingSpeed;
    public int LR; //右方向が＋ 左方向がマイナス
    private Rigidbody2D enemy;

    public int hp;
    public int attack;
    public float attackInterval;

    private bool isMove;
    private bool isAttack;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
        isMove = true;
        isAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isMove == true) {
            enemy.constraints = RigidbodyConstraints2D.None;
            enemy.constraints = RigidbodyConstraints2D.FreezeRotation;
            enemy.velocity = new Vector2(LR * movingSpeed, 0);
        }
        //hpがゼロになったら死ぬ処理
        if (hp <= 0) {
            Destroy(this.gameObject);
        }
        if (isAttack == true) {
Debug.Log("update内で攻撃を呼び出します");
            isAttack = false;
            StartCoroutine("Attack");
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
Debug.Log(this.gameObject.tag);
Debug.Log(collision.gameObject.tag);
        if ((this.gameObject.tag == "player" && collision.gameObject.tag == "enemy")
            || this.gameObject.tag == "enemy" && collision.gameObject.tag == "player") {
Debug.Log("if文入りました");
            target = collision.gameObject;
            enemy.constraints = RigidbodyConstraints2D.FreezeAll;
            isAttack = true;
            isMove = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
Debug.Log("コリジョン状態から解放されました");
        if ((this.gameObject.tag == "player" && collision.gameObject.tag == "enemy")
            || this.gameObject.tag == "enemy" && collision.gameObject.tag == "player")
        {
Debug.Log("歩き出しのif文入りました");
            isMove = true;
            isAttack = false;
        }
    }
    private IEnumerator Attack()
    {
Debug.Log("Attack処理が呼ばれました");
        yield return new WaitForSeconds(attackInterval);
        this.target.GetComponent<movingEnemyPtn001>().hp =
            (this.target.GetComponent<movingEnemyPtn001>().hp - this.attack);
Debug.Log("攻撃しました");

//        if(this.target.GetComponent<movingEnemyPtn001>().hp > 0) { 
//            isAttack = true;
//        }
    }
}
