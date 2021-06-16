using System.Collections;
using System.Collections.Generic;
using StateManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tapStart : MonoBehaviour
{
    private TouchManager _touch_manager;
    private bool oneTimeFlg;
    public GameObject canvasGroupObjct;

    // Start is called before the first frame update
    void Start()
    {
        // タッチ管理マネージャ生成
        this._touch_manager = new TouchManager();
        oneTimeFlg = true;
        StartCoroutine(canvasGroupObjct.GetComponent<transitionController>().StartTransition(canvasGroupObjct.GetComponent<CanvasGroup>()));
    }

    // Update is called once per frame
    void Update()
    {
        // タッチ状態更新
        this._touch_manager.update();

        // タッチ取得
        TouchManager touch_state = this._touch_manager.getTouch();

        // タッチされていたら処理
        if (touch_state._touch_flag)
        {

            if (oneTimeFlg)
            {
                oneTimeFlg = false;
                StartCoroutine(canvasGroupObjct.GetComponent<transitionController>().ChangeScene(canvasGroupObjct.GetComponent<CanvasGroup>(), "MainMenu"));
            }

            if (touch_state._touch_phase == TouchPhase.Began)
            {

            }
        }
    }

}
