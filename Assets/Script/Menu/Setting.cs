using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    [SerializeField]
    private Image[] language;

    [SerializeField]
    private ChatBox chatBox;

    public enum Language
    {
        Japanese = 0,
        English = 1,
        Chinese = 2
    }

    public void Reset()
    {
        PlayerPrefs.SetInt(PlayerPrefabKeys.currentStageId, 101);
        PlayerPrefs.SetInt(PlayerPrefabKeys.clearStageId, 101);
    }

    public void OnChangeLanguage(int index)
    {
        for (int i = 0; i < language.Length; i++) language[i].color = new Color(250/255f, 248/255f, 235/255f, 56/255f);
        language[index].color = new Color(154/255f, 132/255f, 0);
        //言語の変更をセーブデータに反映
        PlayerPrefs.SetInt(PlayerPrefabKeys.currentLanguage, index);
        chatBox.UpdateMessageArray();
        chatBox.DisplayMessage();
        Debug.Log("言語の変更");
    }
}
