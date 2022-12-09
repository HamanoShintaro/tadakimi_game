using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
[CreateAssetMenu(fileName = "SenarioTalkScript", menuName = "SettingSenarioTalkScript")]
public class SenarioTalkScript : ScriptableObject
{
	[SerializeField]
	public string stage;
	[SerializeField]
	public string title;
	[SerializeField]
	public List<SenarioTalk> senarioTalksJapanese = new List<SenarioTalk>();

	[SerializeField]
	public List<SenarioTalk> senarioTalksEnglish = new List<SenarioTalk>();

	[System.SerializableAttribute]
	public class SenarioTalk
    {
		public string script;
		public string type;
		public string name;
		public string LR;
		public string expressions;
		public AudioClip voice;
		public AudioClip SE;
		public Sprite bgImage;
		public AudioClip BGM;
		public Sprite still;
	}


	public string GetStage()
	{
		return stage;
	}

	public string GetTitle()
	{
		return title;
	}

	public List<SenarioTalk> GetSenarioTalks()
	{
		var currentLanguage = PlayerPrefs.GetInt(PlayerPrefabKeys.currentLanguage);
		
		if (currentLanguage == 0)
		{
			return senarioTalksJapanese;
		}
        else if (currentLanguage == 1)
		{
			return senarioTalksEnglish;
        }
		else
        {
			//TODO中国語に変更する
			return senarioTalksEnglish;
        }
	}
}
