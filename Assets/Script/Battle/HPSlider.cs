using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPSlider : MonoBehaviour
{
    [SerializeField]
        private Image GreenGauge;

        [SerializeField]
        private Image RedGauge;

        private Tween redGaugeTween;

        [SerializeField]
        private CharacterCore player;
        //Hpゲージの変数
        public float maxHp;
        public float befHp;
        public float hp;

        private void Start()
        {
            StartCoroutine(Initialize());
        }
        
        private IEnumerator Initialize()
        {
            yield return null;
            maxHp = player.Hp;
        }

        private void Update()
        {
            hp = player.Hp;
            if (hp == befHp) return;
            GaugeReduction(maxHp, befHp / maxHp, hp / maxHp);
            befHp = hp;
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