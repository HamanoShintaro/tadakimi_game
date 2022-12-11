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
        
        //TODOデータ(list)を毎回初期化している
        //データがあるならLoad()
        //データがないor初期化ならInitUser()

        characterSave.list.RemoveRange(0, characterSave.list.Count);

        //2キャラだけ解放する
        InitUser();
        AddCharacterDate("Era_01", 1, false);

        characterSave.Load();
        characterFormation.Load();

        Debug.Log(characterFormation.list[0]);
    }

    /// <summary>
    /// データの初期化するメソッド
    /// </summary>
    private void InitUser()
    {
        //menu表示のための設定
        PlayerPrefs.SetString(PlayerPrefabKeys.currentMenuView, PlayerPrefabKeys.mainMenuView);
        //セーブデータの初期設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.currentStageId)) { PlayerPrefs.SetString(PlayerPrefabKeys.currentStageId, "101"); }
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.clearStageId)) { PlayerPrefs.SetString(PlayerPrefabKeys.clearStageId, "101"); }
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.playerMoney)) { PlayerPrefs.SetInt(PlayerPrefabKeys.playerMoney, 0); }
        //音量の初期設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.volumeBGM)) { PlayerPrefs.SetFloat(PlayerPrefabKeys.volumeBGM, GameSettingParams.bgmVolume); }
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.volumeSE)) { PlayerPrefs.SetFloat(PlayerPrefabKeys.volumeSE, GameSettingParams.seVolume); }
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.volumeCV)) { PlayerPrefs.SetFloat(PlayerPrefabKeys.volumeCV, GameSettingParams.cvVolume); }

        //言語の初期設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.currentLanguage)) { PlayerPrefs.SetInt(PlayerPrefabKeys.currentLanguage, GameSettingParams.currentLanguage); }

        //初期キャラを追加
        AddCharacterDate(GameSettingParams.initCharacter, 1, false);

        //初期編成の設定
        CharacterFormation characterFormation = new CharacterFormation();
        characterFormation.list[0] = GameSettingParams.initCharacter;
        characterFormation.list[1] = "";
        characterFormation.list[2] = "";
        characterFormation.list[3] = "";

        characterFormation.Save();
    }

    /// <summary>
    /// 保持キャラクターのリストにキャラクターを追加する
    /// </summary>
    /// <param name="id">キャラクターID(string)</param>
    /// <param name="level">キャラクターレベル</param>
    /// <param name="hasSpecial">奥義の有無</param>
    private void AddCharacterDate(string id, int level, bool hasSpecial)
    {
        //追加するキャラクターのデータを作成
        CharacterSaveData.CharacterData characterData = new CharacterSaveData.CharacterData();
        characterData.id = id;
        characterData.level = level;
        characterData.hasSpecial = hasSpecial;

        //characterSaveDate.listに追加するキャラクターのデータを追加
        characterSave.list.Add(characterData);

        //上書き保存をする
        characterSave.Save();
    }

    /// <summary>
    /// キャラクターフォーメーションのリストにキャラクターを追加するメソッド
    /// </summary>
    /// <param name="characterData">追加するキャラクターのid</param>
    /// <param name="selectIndex">選択したインデックス</param>
    public void UpdateCharacterFormationDate(string addId, int selectIndex = 0)
    {
        characterFormation.list[selectIndex] = addId;

        //上書き保存する
        //characterFormation.Save();
    }

    /// <summary>
    /// 保持キャラクターデータを保存するクラス
    /// </summary>
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
            Debug.Log("保持キャラクターをロードします");
            if (PlayerPrefs.HasKey(PlayerPrefabKeys.playerCharacterData))
            {
                string saveData = PlayerPrefs.GetString(PlayerPrefabKeys.playerCharacterData);
                Wrapper wrapper = JsonUtility.FromJson<Wrapper>(saveData);
                list = wrapper.list;
            }
        }
    }

    /// <summary>
    /// キャラクターフォーメーションデータを保存するクラス
    /// </summary>
    public class CharacterFormation
    {
        public string[] list = new string[4];

        [System.Serializable]
        class Wrapper
        {
            public string[] list;
        }

        public void Save()
        {
            Wrapper wrapper = new Wrapper();
            wrapper.list = list;
            string saveData = JsonUtility.ToJson(wrapper);
            PlayerPrefs.SetString(PlayerPrefabKeys.playerCharacterFormation, saveData);
        }

        public void Load()
        {
            Debug.Log("フォーメーションデータをロードします");
            string saveData = PlayerPrefs.GetString(PlayerPrefabKeys.playerCharacterFormation);
            Wrapper wrapper = JsonUtility.FromJson<Wrapper>(saveData);
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = wrapper.list[i];
            }
        }
    }
}