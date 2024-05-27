using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// listにキャラクターのデータ保存されている
/// </summary>
public class SaveController : MonoBehaviour
{
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

    private void Start()
    {
        //ロード
        characterSave.Load();
        characterFormation.Load();

        //ロードしてデータがないor初期化ならInitUser()
        if (characterSave == null || characterSave.list.Count == 0)
        {
            InitUser();
        }
    }

    /// <summary>
    /// データの初期化するメソッド
    /// </summary>
    private void InitUser()
    {
        //チュートリアルを表示
        tutorial.SetActive(true);

        //menu表示のための設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.currentMenuView)) PlayerPrefs.SetString(PlayerPrefabKeys.currentMenuView, PlayerPrefabKeys.mainMenuView);
        //セーブデータの初期設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.currentStageId)) PlayerPrefs.SetString(PlayerPrefabKeys.currentStageId, "101");
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.clearStageId)) PlayerPrefs.SetString(PlayerPrefabKeys.clearStageId, "100");
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.playerMoney)) PlayerPrefs.SetInt(PlayerPrefabKeys.playerMoney, 0);
        //音量の初期設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.volumeBGM)) PlayerPrefs.SetFloat(PlayerPrefabKeys.volumeBGM, GameSettingParams.bgmVolume);
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.volumeSE)) PlayerPrefs.SetFloat(PlayerPrefabKeys.volumeSE, GameSettingParams.seVolume);
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.volumeCV)) PlayerPrefs.SetFloat(PlayerPrefabKeys.volumeCV, GameSettingParams.cvVolume);

        //言語の初期設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.currentLanguage)) { PlayerPrefs.SetInt(PlayerPrefabKeys.currentLanguage, GameSettingParams.currentLanguage); }

        //初期キャラをキャラクターデータに追加
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.playerCharacterData))
        {
            /*
            AddCharacterDate("Orend_01", 1, true);
            AddCharacterDate("Npc_01", 1, true);
            AddCharacterDate("Npc_02", 1, true);
            AddCharacterDate("Npc_03", 1, true);
            */
            Debug.Log("キャラクターデータ初期化");
        }

        //初期キャラをキャラクターフォーメーション[0]に追加
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.playerCharacterFormation))
        {
            /*
            UpdateCharacterFormationDate("Orend_01", 0);
            UpdateCharacterFormationDate("Npc_01", 1);
            UpdateCharacterFormationDate("Npc_02", 2);
            UpdateCharacterFormationDate("Npc_03", 3);
            */
            Debug.Log("フォーメーション初期化");
        }

        //広告表示モードを表示に設定
        PlayerPrefs.SetInt(PlayerPrefabKeys.currentAdsMode, 0);
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
            //Debug.Log("保持キャラクターをロードします");
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