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
        [Range(0.1f,100f)]
        private float speed = 2.0f;

        [SerializeField]
        public bool isMove = true;

        private RectTransform player;

        private Animator animator;

        private void Start()
        {
            player = this.GetComponent<RectTransform>();
            animator = this.GetComponent<Animator>();
        }

        private void Update()
        {
            //Debug.Log($"{Screen.width} * {Screen.height} : {Screen.fullScreenMode}");
            //移動入力がある場合は背景を動かす&歩きアニメーション再生 / ない場合はアニメーションを停止
            if (Input.GetKey(KeyCode.D))
            {
                //isMove = true;
                MoveRight();
            }
            else if (Input.GetKey(KeyCode.A))
            {
                //isMove = true;
                MoveLeft();
            }
            else
            {
                isMove = false;//TODOタッチ操作に対応
            }
        }

        public void MoveRight()
        {
            //プレイヤーの前向きに回転
            player.transform.localEulerAngles = new Vector3(0, 0, 0);

            //接敵中は前進不可
            if (player.GetComponent<CharacterCore>().targets.Count > 0)
            {
                var distance = Vector2.Distance(player.GetComponent<CharacterCore>().targets[0].GetComponent<RectTransform>().anchoredPosition, GetComponent<RectTransform>().anchoredPosition);
                //敵に近づける限界の距離
                var limitZoneDistance = 300;
                if (distance <= limitZoneDistance) return;
            }

            //範囲を制限
            if (player.anchoredPosition.x > 2600) return;

            //前進
            player.anchoredPosition = new Vector3(player.anchoredPosition.x + speed, -250, 0);
            isMove = true;
        }

        public void MoveLeft()
        {
            //プレイヤーの後向きに回転
            player.transform.localEulerAngles = new Vector3(0, 180, 0);

            //範囲を制限
            if (player.anchoredPosition.x < -2600) return;

            //後進
            player.anchoredPosition = new Vector3(player.anchoredPosition.x - speed, -250, 0);
            isMove = true;
        }
    }
}
