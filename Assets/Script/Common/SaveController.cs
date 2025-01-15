using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

/// <summary>
/// listにキャラクターのデータ保存されている
/// </summary>
public class SaveController : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private GameObject tutorial;

    /// <summary>
    /// セーブデータから取り出した値(list)を格納したCharacterSaveDate
    /// </summary>
    public CharacterSaveData characterSave = new CharacterSaveData();

    /// <summary>
    /// セーブデータから取り出した値(list)を格納したCharacterFormation
    /// </summary>
    public CharacterFormation characterFormation = new CharacterFormation();

    // 初期化完了時のイベント
    public event Action OnInitialized;

    private void Start()
    {
        characterSave.Load();
        characterFormation.Load();

        SetupUserData();
        SetInitialValues();
    }

    /// <summary>
    /// データの初期化するメソッド
    /// </summary>
    public void SetupUserData()
    {
        //menu表示のための設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.currentMenuView)) PlayerPrefs.SetString(PlayerPrefabKeys.currentMenuView, PlayerPrefabKeys.mainMenuView);
        //セーブデータの初期設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.currentStageId)) 
        {
            PlayerPrefs.SetString(PlayerPrefabKeys.currentStageId, "101");
            OnInitialized?.Invoke();
        }
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.clearStageId)) PlayerPrefs.SetString(PlayerPrefabKeys.clearStageId, "100");
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.playerMoney)) PlayerPrefs.SetInt(PlayerPrefabKeys.playerMoney, 0);
        //音量の初期設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.volumeBGM)) PlayerPrefs.SetFloat(PlayerPrefabKeys.volumeBGM, GameSettingParams.bgmVolume);
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.volumeSE)) PlayerPrefs.SetFloat(PlayerPrefabKeys.volumeSE, GameSettingParams.seVolume);
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.volumeCV)) PlayerPrefs.SetFloat(PlayerPrefabKeys.volumeCV, GameSettingParams.cvVolume);

        //言語の初期設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.currentLanguage)) { PlayerPrefs.SetInt(PlayerPrefabKeys.currentLanguage, GameSettingParams.currentLanguage); }

        //広告表示モードを表示に設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.currentAdsMode)) PlayerPrefs.SetInt(PlayerPrefabKeys.currentAdsMode, 0);

        //初期キャラをキャラクターデータに追加
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.playerCharacterData))
        {
            foreach (string characterId in GameSettingParams.initCharacter)
            {
                AddCharacterDate(characterId, 0, true);
            }
            Debug.Log("キャラクターデータ初期化");
        }

        //初期キャラをキャラクターフォーメーション[0]に追加
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.playerCharacterFormation))
        {
            for (int i = 0; i < GameSettingParams.initCharacter.Length; i++)
            {
                UpdateCharacterFormationDate(GameSettingParams.initCharacter[i], i);
            }
            Debug.Log("フォーメーション初期化");
        }

        // ログを表示する
        Debug.Log("ユーザー設定完了");
        
    }
    private void SetInitialValues()
    {
        float bgmVolume = PlayerPrefs.GetFloat(PlayerPrefabKeys.volumeBGM, GameSettingParams.bgmVolume);
        float seVolume = PlayerPrefs.GetFloat(PlayerPrefabKeys.volumeSE, GameSettingParams.seVolume);
        float cvVolume = PlayerPrefs.GetFloat(PlayerPrefabKeys.volumeCV, GameSettingParams.cvVolume);

        audioMixer.SetFloat("BGM", bgmVolume);
        audioMixer.SetFloat("SE", seVolume);
        audioMixer.SetFloat("CV", cvVolume);
    }

    /// <summary>
    /// ユーザーデータを削除するメソッド
    /// </summary>
    public void DeleteUserData()
    {
        // キャラクターデータをクリア
        characterSave.list.Clear();
        characterSave.Save();

        // フォーメーションデータをクリア 
        for (int i = 0; i < characterFormation.list.Length; i++)
        {
            characterFormation.list[i] = "";
        }
        characterFormation.Save();

        // PlayerPrefsをクリア
        PlayerPrefs.DeleteAll();

        // ログを表示
        Debug.Log("ユーザーデータが削除されました");

        SetupUserData();
        SetInitialValues();
    }


    /// <summary>
    /// 保持キャラクターのリストにキャラクターを追加する
    /// </summary>
    /// <param name="id">キャラクターID(string)</param>
    /// <param name="level">キャラクターレベル</param>
    /// <param name="hasSpecial">奥義の有無</param>
    public void AddCharacterDate(string id, int level, bool hasSpecial)
    {
        //追加するキャラクターのデータを作成
        CharacterSaveData.CharacterData characterData = new CharacterSaveData.CharacterData();
        characterData.id = id;
        characterData.level = level;
        characterData.hasSpecial = hasSpecial;

        //characterSaveDate.listに追加するキャラクターのデータを追加
        characterSave.list.Add(characterData);

        // ログを表示する
        Debug.Log("キャラクターが追加されました: ID = " + id + ", レベル = " + (level + 1) + ", 奥義の有無 = " + hasSpecial);

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

        // ログを表示する
        Debug.Log("キャラクターフォーメーションが更新されました: 追加ID = " + addId + ", 選択インデックス = " + selectIndex);

        //上書き保存する
        characterFormation.Save();
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
            string saveData = PlayerPrefs.GetString(PlayerPrefabKeys.playerCharacterFormation);
            Wrapper wrapper = JsonUtility.FromJson<Wrapper>(saveData);
            if (wrapper == null) return;
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = wrapper.list[i];
            }
        }
    }
}