using System.Collections;
using System.Collections.Generic;
using StateManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tapStart : MonoBehaviour
{
    private TouchManager _touch_manager;
    private bool oneTimeFlg; 

    // Start is called before the first frame update
    void Start()
    {
        // タッチ管理マネージャ生成
        this._touch_manager = new TouchManager();
        oneTimeFlg = true;
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

            Debug.Log("画面タッチを検知");

            if (oneTimeFlg)
            {
                oneTimeFlg = false;
                SceneManager.LoadScene("MainMenu");
            }

            if (touch_state._touch_phase == TouchPhase.Began)
            {

            }
        }
    }

}
