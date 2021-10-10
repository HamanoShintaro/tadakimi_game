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

    // hp�Q�[�W�����炷�����B
    // �����l�A���݂�hp�A�ω��O�̊����A�ω���̊����A�I�v�V�����ŕω��ɂ����鎞��
    public void GaugeReduction(float maxHp, float hp, float valueFrom, float valueTo, float time = 0.5f)
    {
        
        // �΃Q�[�W����
        GreenGauge.fillAmount = valueTo;

        if (redGaugeTween != null)
        {
            redGaugeTween.Kill();
        }

        // �ԃQ�[�W����
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
