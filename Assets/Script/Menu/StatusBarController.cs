using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarController : MonoBehaviour
{
    private GameObject statusBar;
    public GameObject clearStage;
    private Text clearStageText;
    public GameObject money;
    private Text moneyText;
    private int moneyInt;

    private void Start()
    {
        statusBar = this.gameObject;
        clearStageText = clearStage.GetComponent<Text>();
        moneyText = money.GetComponent<Text>();

        //TODOclearStageText.text = senarioTalkScript.stage;
        moneyInt = PlayerPrefs.GetInt(PlayerPrefabKeys.playerMoney);
        moneyText.text = moneyInt.ToString();

        //ステータスバーのUIを更新する
        UpdateStatusUI();
    }

    /// <summary>
    /// ステータスバーのUIを更新するメソッド
    /// </summary>
    public void UpdateStatusUI()
    {
        //所持金額を取得して反映
        var moneyInt = PlayerPrefs.GetInt(PlayerPrefabKeys.playerMoney);
        moneyText.text = moneyInt.ToString();
        PlayerPrefs.SetString(PlayerPrefabKeys.clearStageId, "101");

        //章と話を取得して反映
        var chapter = PlayerPrefs.GetString(PlayerPrefabKeys.clearStageId).Substring(0, 1);
        var story = PlayerPrefs.GetString(PlayerPrefabKeys.clearStageId).Substring(1, 2);
        statusBar.transform.Find("ClearStage/Text").GetComponent<Text>().text = $"{chapter}章{story}話";
    }
}
