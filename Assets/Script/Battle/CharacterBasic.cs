using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CharacterBasic : MonoBehaviour
{
    // キャラクターの種類とレベル
    public string id;
    public int lv;

    // 該当キャラクターのステータスをロードする
    private CharacterInfo characterInfo;
    private CharacterInfo.Status st_params;

    // ステータス
    private string characterName;
    private int hp;
    private int maxHp;
    private int defKB;

    // ステータスと攻撃、及び、モーションにかかわる部分
    private Animator animator;
    private AudioSource audioSource;

    public AudioClip seAttack;

    // 初期位置X,Y と レベル
    public void InitStatus(float initiateX,float initiateY, int lv) {

        characterInfo = Resources.Load<CharacterInfo>(ResourcePath.characterInfoPath + id);
        st_params = characterInfo.status[lv];

        // キャラクター召喚処理
        characterName = characterInfo.name;
        maxHp = st_params.hp;
        hp = st_params.hp;

        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(initiateX, initiateY);
    }

    public string Damage(int d, int kb) {
        hp =- d;
        string status = "";
        if(hp <= 0)
        {
            StartCoroutine(Death());
            status = "death";
        }
        else if (hp > 0 && (kb * (100 - defKB) / 100) > Random.Range(1, 100))
        {
            status = "kb";
        }
        return status;
    }
    public void Recover(int d) {
        hp = Mathf.Max(hp + d, maxHp);
    }
    private IEnumerator Death()
    {
        // Debug.Log("死亡処理が呼ばれました");
        foreach (BoxCollider2D collider in GetComponents<BoxCollider2D>())
        {
            Destroy(collider);
        }
        animator.SetBool("Death", true);
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }
}
