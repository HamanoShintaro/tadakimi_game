using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveController : MonoBehaviour
{
    public CharacterSaveData characterSave;
    public CharacterFormation characterFormation;

    void Start()
    {
        characterFormation.load();
        characterSave.load();
    }

    [System.Serializable]
    public class CharacterSaveData {

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

        public void save()
        {
            Wrapper wrapper = new Wrapper();
            wrapper.list = list;
            string saveData = JsonUtility.ToJson(wrapper);
            PlayerPrefs.SetString(PlayerPrefabKeys.playerCharacterData, saveData);
        }
        public void load()
        {
            if (PlayerPrefs.HasKey(PlayerPrefabKeys.playerCharacterData)) { 
                string saveData = PlayerPrefs.GetString(PlayerPrefabKeys.playerCharacterData);
                Wrapper wrapper = JsonUtility.FromJson<Wrapper>(saveData);
                list = wrapper.list;
            }
        }
	}

    // キャラクターフォーメーションデータ
    [System.Serializable]
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

        public void save() {

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

        public void load()
        {
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
