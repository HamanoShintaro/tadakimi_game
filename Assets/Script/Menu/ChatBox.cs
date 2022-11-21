using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour
{
    [SerializeField]
    private Text chatBox;

    private string[] messageArray;

    private void Start()
    {
        UpdateMessageArray();
        DisplayMessage();
    }

    /// <summary>
    /// 言語設定を更新して、メッセージを画面に表示する
    /// </summary>
    public void OnClickChatBox()
    {
        DisplayMessage();
    }

    /// <summary>
    ///　chatBoxのメッセージをランダムでテキスト表示するメソッド
    /// </summary>
    public void DisplayMessage()
    {
        var index = Random.Range(0, messageArray.Length);
        chatBox.text = messageArray[index];
    }

    //TODOSettingのOnChangeLanguageで呼び出すのもあり
    /// <summary>
    /// 言語設定を更新するメソッド
    /// </summary>
    public void UpdateMessageArray()
    {
        var currentLanguage = PlayerPrefs.GetInt(PlayerPrefabKeys.currentLanguage);
        messageArray = Resources.Load<Message>("DataBase/Data/ChatBoxInfo/ChatBox").GetMessage(currentLanguage);
        Debug.Log("言語を更新");
    }
}
