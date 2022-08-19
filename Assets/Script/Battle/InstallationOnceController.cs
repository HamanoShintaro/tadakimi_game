using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallationOnceController : MonoBehaviour
{
    public int attack = 10;

    public float attackTime;
    public float destroyTime;

    /// <summary>
    /// 与ノックバック値
    /// </summary>
    public int kb;

    private GameObject target;
    private List<GameObject> targets = new List<GameObject>();

    private void Start()
    {
        Debug.Log("発生");
    }
    private void OnTriggerEnter2D(Collider2D t)
    {
        Debug.Log("攻撃範囲に入りました");
        if (t.isTrigger) return; //敵本体に衝突していない場合は除く
        if (this.CompareTag("player") && t.CompareTag("enemy") || this.CompareTag("enemy") && t.CompareTag("enemy")) StartCoroutine(Attack(t.gameObject));
    }
    private IEnumerator Attack(GameObject t)
    {
        Debug.Log("攻撃が当たりました");
        //yield return new WaitForSeconds(attackTime);
        t.GetComponent<IDamage>().Damage(attack, kb);
        yield return new WaitForSeconds(destroyTime);
        this.gameObject.SetActive(false);
    }
    /*
    void Start()
    {
        StartCoroutine("Attack");
    }
    
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
    // ?g???K?[???????????????^?[?Q?b?g?????O??
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
        Debug.Log("?G?t?F?N?g??Attack??????????????????");
        yield return new WaitForSeconds(attackTime);

        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                Debug.Log("targetinstance??" + target.GetInstanceID());
                target.GetComponent<movingEnemyPtn001>().hp =
                       (target.GetComponent<movingEnemyPtn001>().hp - this.attack);
            }
        }
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }
    */
}
