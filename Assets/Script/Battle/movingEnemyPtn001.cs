using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class movingEnemyPtn001 : MonoBehaviour
{
    // 移動に関わる処理(自分自身のRigidbodyも)
    public float movingSpeed;
    public int LR; //右方向が＋ 左方向がマイナス
    protected Rigidbody2D enemy;

    // ステータス
    public string characterName;
    public int hp;
    private int maxHp;
    public int attack;
    public float attackInterval;

    //　スキルにかかわる値
    public bool hasSkill;
    public float skillCoolDown;
    public float skillInterval;
    public int cost;
    public GameObject magicPower;
    private MagicPowerController magicPowerController;
    public GameObject characterPanel;
    private SkillManager skillManager;
    private float skillCoolTime;

    // 初期位置
    public float initiateX;
    public float initiateY;

    // ステータスと攻撃、及び、モーションにかかわる部分
    private string status; //walk,attack,stay,death
    private List<GameObject> targets = new List<GameObject>();
    private Animator animator;
    private AudioSource audioSource;

    public AudioClip seAttack;
    public bool isTower;

    // Start is called before the first frame update
    protected virtual void Start()
    { 
        // キャラクター召喚処理
        maxHp = hp;
        enemy = GetComponent<Rigidbody2D>();
        status = "walk";
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(initiateX, initiateY);
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        magicPower = GameObject.Find("power");
        characterPanel = GameObject.Find("characterPanel");

        // スキル設定
        if (hasSkill)
        {
            skillCoolTime = skillCoolDown;
            magicPowerController = magicPower.GetComponent<MagicPowerController>();
            skillManager = characterPanel.GetComponent<SkillManager>();
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //hpがゼロになったら死ぬ処理
        if (hp <= 0 && status != "death")
        {
            status = "death";
            if (isTower)
            {
                // 終了演出を呼び出す
            }
            else
            {
                StartCoroutine("Death");
            }
        }
        if (status == "attack")
        {
            // targetsに含むGameObjectの最新化を行う(存在しないgameObjectを削除する)
            foreach (GameObject target in targets)
            {
                if (target == null) { targets.Remove(target); }
            }
            // 最新化後のtargetsの中身が存在する場合は攻撃モーションへ移行する
            if (targets.Count != 0)
            {
                status = "stay";
                // skill発動条件を満たす場合はskill、そうでなければAttack処理を呼ぶ
                if (hasSkill && skillCoolTime == 0 && cost <= magicPowerController.magicPower)
                {
                    StartCoroutine("Skill");
                }
                else
                {
                    StartCoroutine("Attack");
                }
            }
            else
            {
                // 0件になってい場合は歩き始める
                status = "walk";
            }
        }
        if (status == "walk")
        {
            enemy.constraints = RigidbodyConstraints2D.None;
            enemy.constraints = RigidbodyConstraints2D.FreezeRotation;
            enemy.velocity = new Vector2(LR * movingSpeed * Time.deltaTime, 0);
        }
        // スキル処理
        if (hasSkill)
        {
            skillCoolTime = Mathf.Max(0.0f, skillCoolTime - Time.deltaTime);
        }

    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            if ((this.gameObject.tag == "player" && other.gameObject.tag == "enemy")
                || this.gameObject.tag == "enemy" && other.gameObject.tag == "player")
            {
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
        // Debug.Log("Attack処理が呼ばれました");
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(attackInterval);

        // 既に死んでいる場合は、敵のHPを減らさない
        if (status == "death") { yield break; }

        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                // Debug.Log("targetinstanceは" + target.GetInstanceID());
                //target.GetComponent<movingEnemyPtn001>().hp = (target.GetComponent<movingEnemyPtn001>().hp - this.attack);
            }
        }
        audioSource.PlayOneShot(seAttack);
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("Attack", false);
        status = "attack";
    }

    protected virtual IEnumerator Death()
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
    protected virtual IEnumerator Skill()
    {
        // Debug.Log("Skill処理が呼ばれました");

        // ちゃんとmagicPowerを減らすことが出来たらスキルを発動する
        if (magicPowerController.UseMagicPower(cost))
        {
            animator.SetBool("Skill", true);
            skillManager.ActivateSkill(characterName, this.GetComponent<RectTransform>());
            yield return new WaitForSeconds(skillInterval);
            skillCoolTime = skillCoolDown;
            animator.SetBool("Skill", false);
        }

        status = "attack";
    }
}
