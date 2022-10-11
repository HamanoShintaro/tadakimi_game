using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewport : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    public bool isMove;

    private void FixedUpdate()
    {
        //移動入力がある場合は背景を動かす&歩きアニメーション再生 / ない場合はアニメーションを停止
        if (Input.GetKey(KeyCode.A))
        {
            isMove = true;
            rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x + 2f, 0, 0);
            player.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            isMove = true;
            rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x - 2f, 0, 0);
            player.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            isMove = false;
        }
    }
}
