using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [SerializeField] private GameObject dialogPanel;  // ダイアログパネル
    [SerializeField] private GameObject dialogContent; // ダイアログの内容を表示するパネル
    [SerializeField] private Button yesButton; // Yesボタン
    [SerializeField] private Button noButton; // Noボタン

    // ダイアログの結果を通知するためのイベント
    public Action<bool> OnDialogResult { get; set; }
    
    private void Awake()
    {
        dialogPanel.SetActive(false);
    }

    private void Start()
    {
        yesButton.onClick.AddListener(OnYesButtonPressed);
        noButton.onClick.AddListener(OnNoButtonPressed);
    }

    // ダイアログを表示
    public void ShowDialog()
    {
        dialogPanel.SetActive(true);
    }

    // ダイアログを非表示
    public void HideDialog() 
    {
        dialogPanel.SetActive(false);
    }

    // Yesボタンが押された時の処理
    public void OnYesButtonPressed()
    {
        OnDialogResult?.Invoke(true);
        HideDialog();
    }

    // Noボタンが押された時の処理 
    public void OnNoButtonPressed()
    {
        OnDialogResult?.Invoke(false);
        HideDialog();
    }
}
