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
	public List<SenarioTalk> senarioTalks = new List<SenarioTalk>();

	[System.SerializableAttribute]
	public class SenarioTalk
	{
		public string script;
		public string script_jp;
		public string script_en;
		public string script_ch;
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
		//現在の言語設定を取得
		var currentLanguage = PlayerPrefs.GetInt(PlayerPrefabKeys.currentLanguage);
		//言語の切り替え
		switch (currentLanguage)
		{
			case 0:
				foreach (SenarioTalk senarioTalk in senarioTalks) senarioTalk.script = senarioTalk.script_jp;
				break;
			case 1:
				foreach (SenarioTalk senarioTalk in senarioTalks) senarioTalk.script = senarioTalk.script_en;
				break;
			case 3:
				foreach (SenarioTalk senarioTalk in senarioTalks) senarioTalk.script = senarioTalk.script_ch;
				break;
		}
		return senarioTalks;
	}
}
