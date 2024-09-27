using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
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

        private Tower tower;

        //Hpゲージの変数
        private float maxHp;
        private float befHp;
        private float hp;

        private void Start()
        {
            //敵タワーのキャラクターコア コンポーネントを取得
            tower = this.transform.parent.GetComponent<Tower>();
            maxHp = tower.Hp;
            smoke.SetActive(false);
        }

        private void Update()
        {
            hp = tower.Hp;
            if (hp == befHp) return;
            //Hpに変更があるならゲージアニメーションをする
            GaugeReduction(maxHp, 1.0f * befHp / maxHp, 1.0f * hp / maxHp);
            befHp = hp;
            if ((1.0f * hp / maxHp) < 0.5f) smoke.SetActive(true);
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
                x =>
                {
                    RedGauge.fillAmount = x;
                },
                valueTo,
                time
            );
        }
    }
}
