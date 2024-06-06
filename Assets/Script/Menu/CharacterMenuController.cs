using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    /// <summary>
    /// string型でCharacterInfoを指定してデータを画面に表示する
    /// </summary>
    /// <param name="characterId"></param>
    public void SetCharacter(string characterId)
    {
        //IDに合致するキャラクター情報を返却する
        CharacterInfo character = characterInfoDataBase.GetCharacterInfoByID(characterId);
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

    /// <summary>
    /// キャラクターレベルをあげて、フラグを返すメソッド
    /// </summary>
    public void CharacterLevelUp()
    {
        var characterId = currentCharacter;
        CharacterInfo character = characterInfoDataBase.GetCharacterInfoByID(characterId);

        // すでに最大レベルであればレベルを上げない
        int level = saveController.characterSave.list.Find(characterSave => characterSave.id == characterId).level;
        int maxLevet = character.status.Count;
        if (level == maxLevet) return;

        int index = level;//TODO
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
}

