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
        HideOverlay();
        ShowOverlay(selectIndex);
        characterFormationUI.SetActive(true);
        SetOverlayEnabled(this.selectIndex, true);

        //現在のメニューを更新
        PlayerPrefs.SetString(PlayerPrefabKeys.currentMenuView, PlayerPrefabKeys.characterFormationMenuView);
        UpdateCharacterUI();
    }

    /// <summary>
    /// 編成画面を閉じるメソッド
    /// </summary>
    public void CloseCharacterFormation()
    {
        selectIndex = 1;
        HideOverlay();
        characterFormationUI.SetActive(false);
        UpdateCharacterUI();
    }

    /// <summary>
    /// 編成画面のキャラクターの選択時に呼び出すメソッド
    /// </summary>
    /// <param name="addId"></param>
    public void OnSelectedCharacter(string addId)
    {
        if (string.IsNullOrEmpty(addId))
        {
            saveController.UpdateCharacterFormationDate(null, selectIndex);
        }
        else
        {
            if (IsCharacterAlreadyInFormation(addId)) return;
            saveController.UpdateCharacterFormationDate(addId, selectIndex);
        }
        UpdateCharacterUI();
        CloseCharacterFormation();
        Debug.Log($"{selectIndex}番を選択して{addId}に変更する");
    }

    private bool IsCharacterAlreadyInFormation(string addId)
    {
        foreach (var characterId in saveController.characterFormation.list)
        {
            if (characterId == addId) return true;
        }
        return false;
    }

    private void ShowOverlay(int index)
    {
        SetOverlayEnabled(index, false);
    }

    private void HideOverlay()
    {
        for (int i = 0; i < 4; i++)
        {
            SetOverlayEnabled(i, false);
        }
    }

    private void SetOverlayEnabled(int index, bool isEnabled)
    {
        characterButtonGroup.transform.GetChild(index).Find("Overlay").GetComponent<Image>().enabled = isEnabled;
    }

    /// <summary>
    /// キャラクターUIを表示を更新するメソッド
    /// </summary>
    public void UpdateCharacterUI()
    {
        AdjustCharacterGroupSize();
        UpdateCharacterList();
        UpdateFormationList();
    }

    private void AdjustCharacterGroupSize()
    {
        int count = saveController.characterSave.list.Count - characterGroup.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            var nullCharacterIconButtonPrefab = Resources.Load<GameObject>($"Prefabs/Menu/Character_Button").gameObject;
            var nullCharacterIconButton = Instantiate(nullCharacterIconButtonPrefab, Vector2.zero, Quaternion.identity);
            nullCharacterIconButton.transform.SetParent(characterGroup.transform);
            nullCharacterIconButton.GetComponent<RectTransform>().localScale = Vector2.one;
        }
    }

    private void UpdateCharacterList()
    {
        var sortedCharacterList = new List<SaveController.CharacterSaveData.CharacterData>(saveController.characterSave.list);
        sortedCharacterList.RemoveAll(character => character.id == "Volcus_01");
        sortedCharacterList.Sort((cs1, cs2) => cs1.level.CompareTo(cs2.level));
        for (int i = 0; i < sortedCharacterList.Count; i++)
        {
            var characterId = sortedCharacterList[i].id;
            if (string.IsNullOrEmpty(characterId))
            {
                characterButtonGroup.transform.GetChild(i).GetComponent<Image>().sprite = null;
            }
            CharacterInfo characterInfo = characterInfoDataBase.GetCharacterInfoByID(characterId);
            var characterButton = characterGroup.transform.GetChild(i).GetComponent<CharacterButton>();
            characterButton.characterId = characterId;
            characterButton.GetComponent<Image>().sprite = characterInfo.image.icon;
            UpdateCharacterOverlay(i, characterId);
            var flameImage = characterGroup.transform.GetChild(i).transform.Find("Frame").GetComponent<Image>();
            UpdateFrame(flameImage, i, characterId, characterInfo);
        }
    }

    private void UpdateCharacterOverlay(int index, string characterId)
    {
        bool isInFormation = false;
        for (int j = 0; j < saveController.characterFormation.list.Length; j++)
        {
            if (characterId == saveController.characterFormation.list[j])
            {
                isInFormation = true;
                break;
            }
        }
        var grayLabel = characterGroup.transform.GetChild(index).transform.Find("GrayLabel").GetComponent<Image>();
        var text = characterGroup.transform.GetChild(index).transform.Find("Text").GetComponent<Text>();
        grayLabel.enabled = isInFormation;
        text.enabled = isInFormation;
    }

    private void UpdateFormationList()
    {
        for (int i = 0; i < characterButtonGroup.transform.childCount; i++)
        {
            var characterId = saveController.characterFormation.list[i];
            CharacterInfo characterInfo = characterInfoDataBase.GetCharacterInfoByID(characterId);
            if (characterInfo != null)
            {
                Image characterButton = characterButtonGroup.transform.GetChild(i).GetComponent<Image>();
                characterButton.sprite = characterInfo.image.icon;
            }
            else
            {
                characterButtonGroup.transform.GetChild(i).GetComponent<Image>().sprite = null;
            }
            var flameImage = characterButtonGroup.transform.GetChild(i).transform.Find("Frame").GetComponent<Image>();
            UpdateFrame(flameImage, i, characterId, characterInfo);
        }
    }

    private void UpdateFrame(Image flameImage, int index, string characterId, CharacterInfo characterInfo)
    {
        if (string.IsNullOrEmpty(characterId))
        {
            flameImage.sprite = Resources.Load<Sprite>($"Image/Decoration/flame/silver");
        }
        else
        {
            int rank = characterInfo.rank;
            const int RANK_RARE = 3;
            if (rank == RANK_RARE)
            {
                flameImage.sprite = Resources.Load<Sprite>($"Image/Decoration/flame/gold");
            }
            else
            {
                flameImage.sprite = Resources.Load<Sprite>($"Image/Decoration/flame/silver");
            }
        }
    }
}