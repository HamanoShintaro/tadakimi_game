using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Battle;
/// <summary>
/// キャラクターを購入するためのボタン
/// </summary>
public class BuyCharacter : MonoBehaviour
{
    [SerializeField]
    [Header("解放レベル")]
    private int releaseStageId;

    [SerializeField]
    private CharacterId characterId;

    [SerializeField]
    [HideInInspector]
    private SaveController saveController;

    private void OnEnable()
    {
        TryReleaseCharacter();
        UpdateCharacterButton();
    }

    public void TryReleaseCharacter()
    {
        if (int.Parse(PlayerPrefs.GetString(PlayerPrefabKeys.maxStageId)) < releaseStageId) return;
        if (saveController.characterSave.list.Exists(characterSave => characterSave.id == characterId.ToString())) return;
        saveController.AddCharacterDate($"{characterId}", 0, false);
    }

    /// <summary>
    /// キャラクターステータス画面でボタンの状態を更新するメソッド(グレー:購入不可のキャラクター カラー:購入可能)
    /// </summary>
    public void UpdateCharacterButton()
    {
        //解放されているキャラクターはカラー表示にする
        if (int.Parse(PlayerPrefs.GetString(PlayerPrefabKeys.maxStageId)) >= releaseStageId)
        {
            GetComponent<Button>().enabled = true;
            transform.Find("Image").GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
            transform.Find("GrayLabel").gameObject.SetActive(false);

            try
            {
                var level = saveController.characterSave.list.Find(characterSave => characterSave.id == characterId.ToString()).level;
            }
            catch
            {
            }
        }
        else
        {
            GetComponent<Button>().enabled = false;
            transform.Find("Image").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            transform.Find("GrayLabel").gameObject.SetActive(true);
        }
    }
}