using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarController : MonoBehaviour
{
    public Text clearStageText;
    public Text moneyText;

    private void Start()
    {
        //ステータスバーのUIを更新する
        UpdateStatusUI();
    }

    /// <summary>
    /// ステータスバーのUIを更新するメソッド
    /// </summary>
    public void UpdateStatusUI()
    {
        //所持金額を取得して反映
        var totalMoney = PlayerPrefs.GetInt(PlayerPrefabKeys.playerMoney, 0);
        if (moneyText != null)
        {
            moneyText.text = totalMoney.ToString();
        }
        else
        {
            Debug.LogError("moneyTextがnullです。");
        }

        //章と話を取得して反映
        var clearStageId = PlayerPrefs.GetString(PlayerPrefabKeys.clearStageId, "100");
        var chapter = clearStageId.Substring(0, 1);
        var story = clearStageId.Substring(1, 2);
        if (clearStageText != null)
        {
            clearStageText.text = $"{chapter}章{story}話";
        }
        else
        {
            Debug.LogError("clearStageTextがnullです。");
        }
        clearStageText.text = $"{chapter}章{story}話";
    }
}