using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// listにキャラクターのデータ保存されている
/// </summary>
public class SaveController : MonoBehaviour
{
    /// <summary>
    /// セーブデータから取り出した値(list)を格納したCharacterSaveDate
    /// </summary>
    public CharacterSaveData characterSave = new CharacterSaveData();

    /// <summary>
    /// セーブデータから取り出した値(list)を格納したCharacterFormation
    /// </summary>
    public CharacterFormation characterFormation = new CharacterFormation();

    private void Start()
    {
        characterFormation.Load();
        characterSave.Load();

        //TODOデータ(list)を毎回初期化している
        characterSave.list.RemoveRange(0, characterSave.list.Count);

        //2キャラだけ解放する
        InitUser();
        AddCharacterDate("era_01", 1, false);

        Debug.Log("初期化");
        Debug.Log(characterSave.list[0].level);
        Debug.Log(characterSave.list.Count);
    }

    /// <summary>
    /// データの初期化するメソッド
    /// </summary>
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
        CharacterSaveData.CharacterData characterData = new CharacterSaveData.CharacterData();
        //ここで設定を決めている(以下の3つの変数を持ったSaveController.CharacterSaveData.CharacterDataをlistに追加するとキャラが増える
        characterData.id = GameSettingParams.initCharacter;
        characterData.level = 1;
        characterData.hasSpecial = false;

        //characterSaveDate.list(リスト)に現在保持しているキャラクターのデータを追加
        characterSave.list.Add(characterData);
        characterSave.Save();
        Debug.Log(characterData.id);


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

    private void AddCharacterDate(string id, int level, bool hasSpecial)
    {
        SaveController.CharacterSaveData.CharacterData characterData = new SaveController.CharacterSaveData.CharacterData();
        //ここで設定を決めている(以下の3つの変数を持ったSaveController.CharacterSaveData.CharacterDataをlistに追加するとキャラが増える
        characterData.id = id;
        characterData.level = level;
        characterData.hasSpecial = hasSpecial;

        //characterSaveDate.list(リスト)に現在保持しているキャラクターのデータを追加
        characterSave.list.Add(characterData);
        characterSave.Save();
    }

    public class CharacterSaveData
    {
        public List<CharacterData> list = new List<CharacterData>();

        [System.Serializable]
        class Wrapper
        {
            public List<CharacterData> list;
        }

        [System.Serializable]
        public class CharacterData
        {
            // キャラクターID
            public string id;
            // レベル
            public int level;
            // 奥義の有無
            public bool hasSpecial;
        }

        public void Save()
        {
            Wrapper wrapper = new Wrapper();
            wrapper.list = list;
            string saveData = JsonUtility.ToJson(wrapper);
            PlayerPrefs.SetString(PlayerPrefabKeys.playerCharacterData, saveData);
        }

        public void Load()
        {
            Debug.Log("セーブデータをロードします(保持キャラクター)");
            if (PlayerPrefs.HasKey(PlayerPrefabKeys.playerCharacterData))
            {
                string saveData = PlayerPrefs.GetString(PlayerPrefabKeys.playerCharacterData);
                Wrapper wrapper = JsonUtility.FromJson<Wrapper>(saveData);
                list = wrapper.list;
            }
        }
    }

    // キャラクターフォーメーションデータ
    public class CharacterFormation
    {
        public string character_1_id;
        public string character_2_id;
        public string character_3_id;
        public string character_4_id;

        [System.Serializable]
        class Wrapper
        {
            public List<string> list;
        }

        public void Save() {

            List<string> formation = new List<string>();
            formation.Add(character_1_id); Debug.Log(character_1_id);
            formation.Add(character_2_id); Debug.Log(character_2_id);
            formation.Add(character_3_id); Debug.Log(character_3_id);
            formation.Add(character_4_id); Debug.Log(character_4_id);

            Wrapper wrapper = new Wrapper();
            wrapper.list = formation;

            string saveData = JsonUtility.ToJson(wrapper);
            PlayerPrefs.SetString(PlayerPrefabKeys.playerCharacterFormation, saveData);
        }

        public void Load()
        {
            Debug.Log("セーブデータをロードします(フォーメーション)");
            if (PlayerPrefs.HasKey(PlayerPrefabKeys.playerCharacterFormation))
            {
                string saveData = PlayerPrefs.GetString(PlayerPrefabKeys.playerCharacterFormation);
                Wrapper wrapper = JsonUtility.FromJson<Wrapper>(saveData);
               
                character_1_id = wrapper.list[0];
                character_2_id = wrapper.list[1];
                character_3_id = wrapper.list[2];
                character_4_id = wrapper.list[3];
            }
        }
    }
}
