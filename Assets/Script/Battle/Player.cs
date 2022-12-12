using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// プレイヤーキャラクターのステータス & 行動処理 > Walk or Attack
    /// </summary>
    public class Player : MonoBehaviour
    {
        [SerializeField]
        public bool isMove = true;

        private RectTransform player;

        private void Start()
        {
            player = this.GetComponent<RectTransform>();
        }
        
        private void Update()
        {
            //移動入力がある場合は背景を動かす&歩きアニメーション再生 / ない場合はアニメーションを停止

            if (Input.GetKey(KeyCode.D))
            {
                //プレイヤーの前向きに回転
                player.transform.localEulerAngles = new Vector3(0, 0, 0);

                //接敵中は前進不可
                if (player.GetComponent<CharacterCore>().targets.Count > 0) return;

                //範囲を制限
                if (player.anchoredPosition.x > 2300) return;

                //前進
                player.anchoredPosition = new Vector3(player.anchoredPosition.x + 2, -250, 0);
                isMove = true;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                //プレイヤーの後向きに回転
                player.transform.localEulerAngles = new Vector3(0, 180, 0);

                //範囲を制限
                if (player.anchoredPosition.x < -2300) return;

                //後進
                player.anchoredPosition = new Vector3(player.anchoredPosition.x - 2, -250, 0);
                isMove = true;
            }
            else
            {
                isMove = false;
            }
        }
    }
}
