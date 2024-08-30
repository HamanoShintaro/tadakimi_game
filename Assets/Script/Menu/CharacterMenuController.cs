using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CharacterMenuController : MonoBehaviour
{
    // キャラクター情報に関するオブジェクト
    public GameObject characterNameObj;
    public GameObject characterAiliasObj;
    public GameObject characterDescriptionObj;
    public GameObject characterSkillDescriptionObj;

    // キャラクター画像に関するオブジェクト
    public GameObject characterFullSizeImage;
    public GameObject characterBackground;
    public GameObject characterEffect;

    // キャラクターステータスに関するオブジェクト
    public GameObject characterLevelObj;
    public GameObject characterLevelMaxObj;
    public GameObject characterAttackObj;
    public GameObject characterHpObj;
    public GameObject characterCostObj;
    public GameObject characterSpeedObj;
    public GameObject characterKnockBackObj;
    public GameObject characterKnockBackDeffenceObj;
    public GameObject characterLvUpCostObj;

    [SerializeField]
    private GameObject characterView;

    // コントローラークラス、リソースのロード用
    public MenuController menuController;
    private CharacterInfoDataBase characterInfoDataBase;
    private SaveController saveController;

    //現在選択しているキャラクター名
    private string currentCharacter;

    // オーディオに関するオブジェクト
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    private void Start()
    {
        characterInfoDataBase = Resources.Load<CharacterInfoDataBase>(ResourcePath.CharacterInfoDataBasePath);
        saveController = menuController.GetComponent<SaveController>();
        SetCharacter(saveController.characterSave.list[0].id);
    }

    private void OnEnable() 
    {
        Invoke("SortCharacterButtonsByLevel", 0.1f);
    }

    /// <summary>
    /// string型でCharacterInfoを指定してデータを画面に表示する
    /// </summary>
    /// <param name="characterId"></param>
    public void SetCharacter(string characterId)
    {
        //IDに合致するキャラクター情報を返却する
        CharacterInfo character = characterInfoDataBase.GetCharacterInfoByID(characterId);
        if (character == null)
        {
            Debug.LogError("Character not found: " + characterId);
            return;
        }
        currentCharacter = characterId;

        //キャラクター情報のセット
        characterNameObj.GetComponent<Text>().text = character.name;
        characterAiliasObj.GetComponent<Text>().text = "ーー" + character.alias;
        characterDescriptionObj.GetComponent<Text>().text = character.detail;
        characterSkillDescriptionObj.GetComponent<Text>().text = character.skill.name + "  消費魔力：" + character.skill.cost + "\n" + character.skill.Detail;

        //キャラクター画像のセット
        characterBackground.GetComponent<Image>().sprite = character.image.backGround;
        characterFullSizeImage.GetComponent<Image>().sprite = character.image.fullsize;
        characterEffect.GetComponent<Image>().sprite = character.image.effect;

        //レベルの取得
        int level = saveController.characterSave.list.Find(characterSave => characterSave.id == characterId).level;
        int index = level - 1;

        //キャラクターステータスのセット
        // インデックスが範囲内にあるか確認
        if (index >= 0 && index < character.status.Count)
        {
            // キャラクターステータスのセット
            characterLevelObj.GetComponent<Text>().text = level.ToString();
            characterLevelMaxObj.GetComponent<Text>().text = character.status.Count.ToString();
            characterAttackObj.GetComponent<Text>().text = character.status[index].attack.ToString();
            characterHpObj.GetComponent<Text>().text = character.status[index].hp.ToString();
            characterCostObj.GetComponent<Text>().text = character.status[index].cost.ToString();
            characterSpeedObj.GetComponent<Text>().text = character.status[index].speed.ToString();
            characterKnockBackObj.GetComponent<Text>().text = character.status[index].atkKB.ToString();
            characterKnockBackDeffenceObj.GetComponent<Text>().text = character.status[index].defKB.ToString();
            characterLvUpCostObj.GetComponent<Text>().text = character.status[index].growth.ToString();
        }
        else
        {
            // インデックスが範囲外の場合のエラーハンドリング
            Debug.LogWarning("キャラクターのステータスインデックスが範囲外です。");
        }
    }

    /// <summary>
    /// キャラクターレベルをあげて、フラグを返すメソッド
    /// </summary>
    public void CharacterLevelUp()
    {
        var characterId = currentCharacter;
        CharacterInfo character = characterInfoDataBase.GetCharacterInfoByID(characterId);
        if (character == null)
        {
            Debug.LogError("Character not found: " + characterId);
            return;
        }

        // すでに最大レベルであればレベルを上げない
        int level = saveController.characterSave.list.Find(characterSave => characterSave.id == characterId).level;
        int maxLevet = character.status.Count;
        if (level == maxLevet) return;

        int index = level;
        int cost = character.status[index].growth;
        int money = PlayerPrefs.GetInt(PlayerPrefabKeys.playerMoney);

        // お金がコストを上回っている場合、処理をする
        if (money >= cost)
        {
            PlayerPrefs.SetInt(PlayerPrefabKeys.playerMoney, money - cost);
            saveController.characterSave.list.Find(characterSave => characterSave.id == characterId).level++;
            saveController.characterSave.Save();
            SetCharacter(characterId);
            audioSource.PlayOneShot(clip);
        }
    }

    private void SortCharacterButtonsByLevel()
    {
        // キャラクターのビューのボタンのリストを取得
        List<GameObject> entityCharacterList = new List<GameObject>();
        foreach (Transform child in characterView.transform)
        {
            entityCharacterList.Add(child.gameObject);
        }

        // キャラクターのソートしたリストを取得
        var sortedCharacterList = new List<SaveController.CharacterSaveData.CharacterData>(saveController.characterSave.list);
        // レベルの高い順に並べ替え
        sortedCharacterList.Sort((cs1, cs2) => cs2.level.CompareTo(cs1.level));

        // キャラクターボタンを存在するものと存在しないもので分ける
        List<GameObject> foundList = new List<GameObject>();
        List<GameObject> notFoundList = new List<GameObject>();

        foreach (var button in entityCharacterList)
        {
            var id = button.name;
            var foundIndex = sortedCharacterList.FindIndex(data => data.id.Equals(id));
            if (foundIndex != -1)
            {
                foundList.Add(button);
            }
            else
            {
                notFoundList.Add(button);
            }
        }

        // 存在するキャラクターのビューのボタンのリストを並べ替え
        foundList.Sort((a, b) => {
            var aId = a.name;
            var bId = b.name;
            var aIndex = sortedCharacterList.FindIndex(data => data.id.Equals(aId));
            var bIndex = sortedCharacterList.FindIndex(data => data.id.Equals(bId));
            return aIndex.CompareTo(bIndex);
        });

        // 存在しないキャラクターのビューのボタンのリストをアルファベット順にソート
        notFoundList.Sort((a, b) => a.name.CompareTo(b.name));

        // 並べ替えた順序にビューのボタンを配置
        int buttonIndex = 0;
        foreach (var button in foundList)
        {
            button.transform.SetSiblingIndex(buttonIndex++);
        }
        foreach (var button in notFoundList)
        {
            button.transform.SetSiblingIndex(buttonIndex++);
        }
   }

    private void ShowOverlay(int index)
    {
        SetOverlayEnabled(index, false);
    }

    public void HideOverlay()
    {
        for (int i = 0; i < 4; i++)
        {
            SetOverlayEnabled(i, false);
        }
    }

    // AllHidel >> ON
    public void SetOverlayEnabled(int index, bool isEnabled)
    {
        characterView.transform.GetChild(index).Find("Overlay").GetComponent<Image>().enabled = isEnabled;
    }
}

