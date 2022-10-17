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
    /// キャラクターを選択時に呼び出すメソッド
    /// </summary>
    /// <param name="addId"></param>
    public void OnSelectedCharacter(string addId)
    {
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
        var count = saveController.characterSave.list.Count - characterGroup.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            var nullCharacterIconButtonPrefab = Resources.Load<GameObject>($"Prefabs/Menu/Character_Button").gameObject;
            var nullCharacterIconButton = Instantiate(nullCharacterIconButtonPrefab, Vector2.zero, Quaternion.identity);
            nullCharacterIconButton.transform.parent = characterGroup.transform;
            nullCharacterIconButton.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
        }

        //TODOsaveController.characterSave.list[i].idをレベル順に並び替えて、新たなリストに入れて更新する

        for (int i = 0; i < saveController.characterSave.list.Count; i++)
        {
            if (saveController.characterSave.list[i].id == saveController.characterFormation.list[i])
            {
                //処理を飛ばす
            }
        }

        //キャラクターリストの更新
        for (int i = 0; i < saveController.characterSave.list.Count; i++)
        {
            var characterId = saveController.characterSave.list[i].id;
            CharacterInfo characterInfo = characterInfoDataBase.GetCharacterInfoByID(characterId);
            characterGroup.transform.GetChild(i).GetComponent<CharacterButton>().characterId = characterId;
            characterGroup.transform.GetChild(i).GetComponent<Image>().sprite = characterInfo.image.icon;
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