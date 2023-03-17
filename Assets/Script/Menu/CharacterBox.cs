using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBox : MonoBehaviour
{
    [SerializeField]
    private GameObject characterFormationUI;

    [SerializeField]
    private GameObject characterButtonGroup;

    [SerializeField]
    private SaveController saveController;

    [SerializeField]
    private CharacterInfoDataBase characterInfoDataBase;

    [HideInInspector]
    public int selectIndex;

    [SerializeField]
    public GameObject characterGroup;

    private void Start()
    {
        UpdateCharacterUI();
    }

    /// <summary>
    /// 現在選択している編成番号を更新して、編成画面を開くメソッド
    /// </summary>
    /// <param name="selectIndex"></param>
    public void OpenCharacterFormation(int selectIndex)
    {
        this.selectIndex = selectIndex;
        //TODO選択中の枠を強調する
        characterFormationUI.SetActive(true);
        //現在のメニューを更新
        PlayerPrefs.SetString(PlayerPrefabKeys.currentMenuView, PlayerPrefabKeys.characterFormationMenuView);
        UpdateCharacterUI();
    }

    /// <summary>
    /// 編成画面を閉じるメソッド
    /// </summary>
    public void CloseCharacterFormation()
    {
        this.selectIndex = 1;
        //TODO強調表示を消す
        characterFormationUI.SetActive(false);
        UpdateCharacterUI();
    }

    /// <summary>
    /// 編成画面のキャラクターの選択時に呼び出すメソッド
    /// </summary>
    /// <param name="addId"></param>
    public void OnSelectedCharacter(string addId)
    {
        for (int i = 0; i < saveController.characterFormation.list.Length; i++)
        {
            if (saveController.characterFormation.list[i] == addId)
            {
                return;
            }
        }
        saveController.UpdateCharacterFormationDate(addId, selectIndex);
        UpdateCharacterUI();
        Debug.Log($"{selectIndex}番を選択して{addId}に変更する");
    }

    /// <summary>
    /// キャラクターUIを表示を更新するメソッド
    /// </summary>
    public void UpdateCharacterUI()
    {
        //所持キャラクター分の空のキャラクターリストを作成
        int count;
        try
        {
            count = saveController.characterSave.list.Count - characterGroup.transform.childCount;
        }
        catch
        {
            count = 0;
        }
        for (int i = 0; i < count; i++)
        {
            var nullCharacterIconButtonPrefab = Resources.Load<GameObject>($"Prefabs/Menu/Character_Button").gameObject;
            var nullCharacterIconButton = Instantiate(nullCharacterIconButtonPrefab, Vector2.zero, Quaternion.identity);
            nullCharacterIconButton.transform.parent = characterGroup.transform;
            nullCharacterIconButton.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
        }

        //キャラクターリストの更新
        try
        {
            for (int i = 0; i < saveController.characterSave.list.Count; i++)
            {
                var characterId = saveController.characterSave.list[i].id;
                CharacterInfo characterInfo = characterInfoDataBase.GetCharacterInfoByID(characterId);
                characterGroup.transform.GetChild(i).GetComponent<CharacterButton>().characterId = characterId;
                characterGroup.transform.GetChild(i).GetComponent<Image>().sprite = characterInfo.image.icon;
                //編成リストに採用されているものはグレー帯を表示、それ以外は非表示
                for (int j = 0; j < saveController.characterFormation.list.Length; j++)
                {
                    if (characterId == saveController.characterFormation.list[j])
                    {
                        characterGroup.transform.GetChild(i).transform.Find("GrayLabel").GetComponent<Image>().enabled = true;
                        characterGroup.transform.GetChild(i).transform.Find("Text").GetComponent<Text>().enabled = true;
                        Debug.Log("グレー帯表示");
                        break;
                    }
                    else
                    {
                        characterGroup.transform.GetChild(i).transform.Find("GrayLabel").GetComponent<Image>().enabled = false;
                        characterGroup.transform.GetChild(i).transform.Find("Text").GetComponent<Text>().enabled = false;
                        Debug.Log("グレー帯非表示");
                    }
                }
            }
        }
        catch
        {

        }

        //編成リストの更新
        for (int i = 0; i < characterButtonGroup.transform.childCount; i++)
        {
            try
            {
                var characterId = saveController.characterFormation.list[i];
                CharacterInfo characterInfo = characterInfoDataBase.GetCharacterInfoByID(characterId);
                characterButtonGroup.transform.GetChild(i).GetComponent<Image>().sprite = characterInfo.image.icon;
            }
            catch
            {
            }
        }
    }
}