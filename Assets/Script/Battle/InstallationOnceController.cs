using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallationOnceController : MonoBehaviour
{
    // ダメージ量に関する設定
    public int attack = 10;

    // 継続時間(effectと合わせる)
    public float attackTime;
    public float destroyTime;

    private GameObject target;
    private List<GameObject> targets = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Attack");
    }

    // 攻撃対象の選定：playerの召喚なら敵に、敵ならプレイヤーに。
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            if ((this.gameObject.tag == "player" && other.gameObject.tag == "enemy")
                || this.gameObject.tag == "enemy" && other.gameObject.tag == "player")
            {
                targets.Add(other.gameObject);
            }
        }
    }
    // トリガーが解消されたらターゲットから外す
    private void OnTriggerExit2D(Collider2D other)
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
    private IEnumerator Attack()
    {
        Debug.Log("エフェクトのAttack処理が呼ばれました");
        yield return new WaitForSeconds(attackTime);

        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                Debug.Log("targetinstanceは" + target.GetInstanceID());
                target.GetComponent<movingEnemyPtn001>().hp =
                       (target.GetComponent<movingEnemyPtn001>().hp - this.attack);
            }
        }
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }

}
