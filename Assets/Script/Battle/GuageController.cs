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

    public GameObject tower;
    private Tween redGaugeTween;

    // hpゲージを減らす処理。
    // 初期値、現在のhp、変化前の割合、変化後の割合、オプションで変化にかける時間
    public void GaugeReduction(float maxHp, float hp, float valueFrom, float valueTo, float time = 0.5f)
    {
        
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
