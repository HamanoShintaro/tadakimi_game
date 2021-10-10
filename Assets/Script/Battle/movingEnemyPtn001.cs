using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class movingEnemyPtn001 : MonoBehaviour
{
    public float movingSpeed;
    public int LR; //右方向が＋ 左方向がマイナス
    protected Rigidbody2D enemy;

    public int hp;
    public int maxHp;
    public int attack;
    public float attackInterval;

    protected bool isMove;
    protected bool isAttack;
    protected bool isInterval;
    protected GameObject target;

    public float initiateX;
    public float initiateY;

    private string status; //walk,attack,stay,death
    private List<GameObject> targets = new List<GameObject>();
    private Animator animator;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        maxHp = hp;
        enemy = GetComponent<Rigidbody2D>();
        status = "walk";
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(initiateX, initiateY);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (status == "walk" ) {
            enemy.constraints = RigidbodyConstraints2D.None;
            enemy.constraints = RigidbodyConstraints2D.FreezeRotation;
            enemy.velocity = new Vector2(LR * movingSpeed, 0);
        }
        //hpがゼロになったら死ぬ処理
        if (hp <= 0 && status != "death") {
            status = "death";
            StartCoroutine("Death");
        }
        if (status == "attack") {
            // targetsに含むGameObjectの最新化を行う(存在しないgameObjectを削除する)
            foreach (GameObject target in targets) {
                if (target == null) { targets.Remove(target); }
            }
            // 最新化後のtargetsの中身が存在する場合は攻撃モーションへ移行する
            if (targets.Count != 0)
            {
                status = "stay";
                StartCoroutine("Attack");
            }
            else {
                // 0件になってい場合は歩き始める
                status = "walk";
            }

        }

    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false) { 
            if ((this.gameObject.tag == "player" && other.gameObject.tag == "enemy")
                || this.gameObject.tag == "enemy" && other.gameObject.tag == "player") {
                targets.Add(other.gameObject);
                enemy.constraints = RigidbodyConstraints2D.FreezeAll;
                if (status == "walk")
                {
                    status = "attack";
                }
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            if ((this.gameObject.tag == "player" && other.gameObject.tag == "enemy")
                   || this.gameObject.tag == "enemy" && other.gameObject.tag == "player")
            {
                targets.Remove(other.gameObject);
            }
        }
    }
    protected virtual IEnumerator Attack()
    {
        Debug.Log("Attack処理が呼ばれました");
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(attackInterval);

        // 既に死んでいる場合は、敵のHPを減らさない
        if (status == "death") { yield break; }

        foreach (GameObject target in targets) {
            if (target != null) {
                Debug.Log("targetinstanceは" + target.GetInstanceID());
                target.GetComponent<movingEnemyPtn001>().hp =
                       (target.GetComponent<movingEnemyPtn001>().hp - this.attack);
            }
        }
        animator.SetBool("Attack", false);
        status = "attack";
    }

    protected virtual IEnumerator Death()
    {
        Debug.Log("死亡処理が呼ばれました");
        foreach (BoxCollider2D collider in GetComponents<BoxCollider2D>()) {
            Destroy(collider);
        }
        animator.SetBool("Death", true);
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }
}
