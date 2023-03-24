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
        [Range(0.1f, 1000f)]
        private float speed = 2.0f;

        [SerializeField]
        private float limitZoneDistance = 300;

        [SerializeField]
        private float minLimitMovePosition = -2600f, maxLimitMovePosition = 2600f;

        [SerializeField]
        public bool isMove = true;

        private RectTransform player;

        private void Start()
        {
            player = GetComponent<RectTransform>();
        }

        private void Update()
        {
            //移動入力がある場合は背景を動かす&歩きアニメーション再生 / ない場合はアニメーションを停止
            if (Input.GetKey(KeyCode.D))
            {
                MoveRight();
            }
            else if (Input.GetKey(KeyCode.A))
            {
                MoveLeft();
            }
            else
            {
                isMove = false;
            }
        }

        public void MoveRight()
        {
            //プレイヤーの前向きに回転
            transform.localEulerAngles = Vector3.zero;

            //接敵中は前進不可
            if (GetComponent<CharacterCore>().targets.Count > 0)
            {
                var distance = Vector2.Distance(GetComponent<CharacterCore>().targets[0].GetComponent<RectTransform>().anchoredPosition, GetComponent<RectTransform>().anchoredPosition);
                //敵に近づける限界の距離
                if (distance <= limitZoneDistance) return;
            }

            //範囲を制限
            if (player.anchoredPosition.x > maxLimitMovePosition) return;

            //前進
            transform.localPosition = new Vector3(transform.localPosition.x + speed * Time.deltaTime, -315, 0);
            isMove = true;
        }

        public void MoveLeft()
        {
            //プレイヤーの後向きに回転
            transform.localEulerAngles = new Vector3(0, 180, 0);

            //範囲を制限
            if (player.anchoredPosition.x < minLimitMovePosition) return;

            //後進
            transform.localPosition = new Vector3(transform.localPosition.x - speed * Time.deltaTime, -315, 0);
            isMove = true;
        }

        public void MoveButtonUp()
        {
            isMove = false;
        }
    }
}
