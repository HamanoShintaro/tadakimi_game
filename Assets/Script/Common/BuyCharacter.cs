using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyCharacter : MonoBehaviour
{
    [SerializeField]
    [Header("解放レベル")]
    private int releaseStageId;

    private void Start()
    {
        UpdateCharacterButton();
    }

    private void OnEnable()
    {
        UpdateCharacterButton();
    }

    /// <summary>
    /// キャラクターステータス画面でボタンの状態を更新するメソッド(グレー:購入不可のキャラクター カラー:購入可能)
    /// </summary>
    public void UpdateCharacterButton()
    {
        //解放されているキャラクターはカラー表示にする
        if (PlayerPrefs.GetInt(PlayerPrefabKeys.currentStageId) >= releaseStageId)
        {
            GetComponent<Button>().enabled = true;
            transform.Find("Image").GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
            transform.Find("GrayLabel").gameObject.SetActive(false);
        }
        else
        {
            GetComponent<Button>().enabled = false;
            transform.Find("Image").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            transform.Find("GrayLabel").gameObject.SetActive(true);
        }
    }
}