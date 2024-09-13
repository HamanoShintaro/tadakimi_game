using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リザルト画面に変更を加える処理
/// </summary>
public class ResultController : MonoBehaviour
{
    [SerializeField] private GameObject winningPanel;
    [SerializeField] private GameObject loserPanel;

    /// <summary>
    /// リザルトパネルを表示・非表示する
    /// </summary>
    /// <param name="isWinner">勝利 or 敗北</param>
    /// <param name="isOn">表示 or 非表示</param>
    public void OnResultPanel(bool isWinner = true, bool isOn = true)
    {
        if (isWinner) winningPanel.SetActive(isOn);
        else loserPanel.SetActive(isOn);
    }
}