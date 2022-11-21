using System.Collections;
using System.Collections.Generic;
using StateManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TapStart : MonoBehaviour
{
    public GameObject canvasGroupObjct;

    /// <summary>
    /// 現在保持しているキャラクターのデータ
    /// </summary>
    public SaveController.CharacterSaveData characterSaveData;
    /*
    private void Awake()
    {
        InitUser();
    }

    private void InitUser()
    {
        // menu表示のための設定
        PlayerPrefs.SetString(PlayerPrefabKeys.currentMenuView, PlayerPrefabKeys.mainMenuView);
        // セーブデータの初期設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.currentStageId)) { PlayerPrefs.SetString(PlayerPrefabKeys.currentStageId, "101"); }
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.clearStageId)) { PlayerPrefs.SetString(PlayerPrefabKeys.clearStageId, "101"); }
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.playerMoney)) { PlayerPrefs.SetInt(PlayerPrefabKeys.playerMoney, 0); }
        // 音量の初期設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.volumeBGM)) { PlayerPrefs.SetFloat(PlayerPrefabKeys.volumeBGM, GameSettingParams.bgmVolume); }
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.volumeSE)) { PlayerPrefs.SetFloat(PlayerPrefabKeys.volumeSE, GameSettingParams.seVolume); }
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.volumeCV)) { PlayerPrefs.SetFloat(PlayerPrefabKeys.volumeCV, GameSettingParams.cvVolume); }

        // 初期キャラの設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.playerCharacterData))
        {
            SaveController.CharacterSaveData.CharacterData characterData = new SaveController.CharacterSaveData.CharacterData();
            //ここで設定を決めている(以下の3つの変数を持ったSaveController.CharacterSaveData.CharacterDataをlistに追加するとキャラが増える
            characterData.id = GameSettingParams.initCharacter;
            characterData.level = 1;
            characterData.hasSpecial = false;

            //characterSaveDate.list(リスト)に現在保持しているキャラクターのデータを追加
            characterSaveData.list.Add(characterData);
            characterSaveData.Save();
        }

        // 初期編成の設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.playerCharacterFormation))
        {
            SaveController.CharacterFormation characterFormation = new SaveController.CharacterFormation();
            characterFormation.character_1_id = GameSettingParams.initCharacter;
            characterFormation.character_2_id = "none";
            characterFormation.character_3_id = "none";
            characterFormation.character_4_id = "none";

            characterFormation.Save();
        }

    }
    */
}
