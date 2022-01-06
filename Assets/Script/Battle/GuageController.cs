using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuageController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Image GreenGauge;
    [SerializeField]
    private Image RedGauge;
    [SerializeField]
    private GameObject smoke;

    private Tween redGaugeTween;

    private GameObject tower;
    private movingEnemyPtn001 enemyManage;

    private int maxHp;
    private int befHp;
    private int hp;

    private GameObject canvas;
    private BattleController battleController;

    private void Start()
    {
        tower = transform.parent.gameObject;
        enemyManage = tower.GetComponent<movingEnemyPtn001>();
        maxHp = enemyManage.hp;
        // Debug.Log(maxHp);
        smoke.SetActive(false);
        canvas = GameObject.Find("Canvas");
        battleController = canvas.GetComponent<BattleController>();

    }

    private void Update()
    {

        hp = enemyManage.hp;
        if(hp != befHp) {
            // Debug.Log("変化前" + befHp.ToString() + "  変化後" + hp.ToString());
            GaugeReduction(maxHp, 1.0f * befHp / maxHp, 1.0f * hp / maxHp);
        }
        befHp = hp;
        if(!smoke.activeSelf && (1.0f * hp / maxHp) < 0.5f)
        {
            smoke.SetActive(true);
        }
        if (hp < 0) { 
            battleController.viewResult("win"); 
        }
    }
    // hpゲージを減らす処理。
    // 初期値、現在のhp、変化前の割合、変化後の割合、オプションで変化にかける時間
    public void GaugeReduction(float maxHp, float valueFrom, float valueTo, float time = 0.5f)
    {
        // Debug.Log("hp現象処理開始");
        // Debug.Log(valueFrom);
        // Debug.Log(valueTo);
        // 緑ゲージ減少
        GreenGauge.fillAmount = valueTo;

        if (redGaugeTween != null)
        {
            redGaugeTween.Kill();
        }

        // 赤ゲージ減少
        redGaugeTween = DOTween.To(
            () => valueFrom,
            x => {
                RedGauge.fillAmount = x;
            },
            valueTo,
            time
        );
    }

}
