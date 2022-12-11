using System;
using UnityEngine;
using UnityEngine.UI;

public class OkCancelDialog : MonoBehaviour
{
    public enum DialogResult
    {
        OK,
        Cancel,
    }
    public GameObject textObj;

    // ダイアログが操作されたときに発生するイベント
    public Action<DialogResult> FixDialog { get; set; }

    // OKボタンが押されたとき
    public void OnOk()
    {
        this.FixDialog?.Invoke(DialogResult.OK);
        Destroy(this.gameObject);
    }

    // Cancelボタンが押されたとき
    public void OnCancel()
    {
        // イベント通知先があれば通知してダイアログを破棄してしまう
        this.FixDialog?.Invoke(DialogResult.Cancel);
        Destroy(this.gameObject);
    }

    // ゲームオブジェクト
    public void SetText(string text) {

        // テキストの設定
        textObj.GetComponent<Text>().text = text;
    }
}