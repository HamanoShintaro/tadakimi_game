using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 敵タワーのゲージ管理のクラス
/// </summary>
public class GuageController : MonoBehaviour
{
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
        smoke.SetActive(false);
        canvas = GameObject.Find("Canvas");
        battleController = canvas.GetComponent<BattleController>();
    }
    private void Update()
    {
        hp = enemyManage.hp;
        if (hp != befHp)
        {
            GaugeReduction(maxHp, 1.0f * befHp / maxHp, 1.0f * hp / maxHp);
        }
        befHp = hp;
        if (!smoke.activeSelf && (1.0f * hp / maxHp) < 0.5f)
        {
            smoke.SetActive(true);
        }
        /*
        if (hp < 0)
        {
            battleController.viewResult("win");//TODO敵タワーで呼ぶ
        }
        */

    }
    /// <summary>
    /// 敵Hpゲージのアニメーションをするメソッド
    /// </summary>
    /// <param name="maxHp"></param>
    /// <param name="valueFrom"></param>
    /// <param name="valueTo"></param>
    /// <param name="time"></param>
    public void GaugeReduction(float maxHp, float valueFrom, float valueTo, float time = 0.5f)
    {
        GreenGauge.fillAmount = valueTo;
        if (redGaugeTween != null) redGaugeTween.Kill();
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
