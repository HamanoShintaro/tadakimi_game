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
        var totalMoney = PlayerPrefs.GetInt(PlayerPrefabKeys.playerMoney);
        moneyText.text = totalMoney.ToString();

        //章と話を取得して反映
        var chapter = PlayerPrefs.GetString(PlayerPrefabKeys.clearStageId).Substring(0, 1);
        var story = PlayerPrefs.GetString(PlayerPrefabKeys.clearStageId).Substring(1, 2);
        clearStageText.text = $"{chapter}章{story}話";
    }
}
