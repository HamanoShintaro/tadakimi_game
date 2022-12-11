using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

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
        var x = Input.GetAxis("Horizontal");
        
        Debug.Log(rectTransform.anchoredPosition.x);
        if (x == 0)
        {
            isMove = false;
        }
        else if (x > 0)
        {
            //前進する処理
            if (rectTransform.anchoredPosition.x < -3600) return;
            player.transform.localEulerAngles = new Vector3(0, 180, 0);
            if (player.GetComponent<CharacterCore>().targets.Count > 0) return;
            isMove = true;
            rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x - x * 2, 0, 0);
            
        }
        else if (x < 0)
        {
            //後進する処理
            if (rectTransform.anchoredPosition.x > 0) return;
            player.transform.localEulerAngles = new Vector3(0, 0, 0);
            isMove = true;
            rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x - x * 2, 0, 0);
        }
    }
}
