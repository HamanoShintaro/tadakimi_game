using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour
{
    [SerializeField]
    private Text chatBox;

    private string[] messageArray;
    private List<AudioClip> voiceList;

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
        PlayVoice();
    }

    /// <summary>
    ///　chatBoxのメッセージをランダムでテキスト表示するメソッド
    /// </summary>
    public void DisplayMessage()
    {
        var index = Random.Range(0, messageArray.Length);
        chatBox.text = messageArray[index];
    }

    /// <summary>
    ///　chatBoxのメッセージに対応するボイスを再生するメソッド
    /// </summary>
    public void PlayVoice()
    {
        if(voiceList == null || voiceList.Count == 0)
        {
            return;
        }
        var index = Random.Range(0, voiceList.Count);
        AudioSource.PlayClipAtPoint(voiceList[index], transform.position);
    }

    //TODOSettingのOnChangeLanguageで呼び出すのもあり
    /// <summary>
    /// 言語設定を更新するメソッド
    /// </summary>
    public void UpdateMessageArray()
    {
        var currentLanguage = PlayerPrefs.GetInt(PlayerPrefabKeys.currentLanguage);
        var message = Resources.Load<Message>("DataBase/Data/ChatBoxInfo/ChatBox");
        messageArray = message.GetMessage(currentLanguage);
        voiceList = message.GetVoice(currentLanguage);
        Debug.Log("言語を更新");
    }

    /// <summary>
    /// 現在の時刻を取得して、朝昼晩でログを出すメソッド
    /// </summary>
    public void LogTimeOfDay()
    {
        var currentHour = System.DateTime.Now.Hour;
        if (currentHour >= 6 && currentHour < 12)
        {
            Debug.Log("おはようございます");
        }
        else if (currentHour >= 12 && currentHour < 18)
        {
            Debug.Log("こんにちは");
        }
        else
        {
            Debug.Log("こんばんは");
        }
    }
}