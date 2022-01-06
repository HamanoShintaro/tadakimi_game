using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
[CreateAssetMenu(fileName = "SenarioTalkScript", menuName = "SettingSenarioTalkScript")]
public class SenarioTalkScript : ScriptableObject
{
	// ステージ番号
	[SerializeField]
	public int stage;
	[SerializeField]
	public List<SenarioTalk> senarioTalks = new List<SenarioTalk>();

	[System.SerializableAttribute]
	public class SenarioTalk
    {
		// 会話内容(日本語最大80文字)
		public string script;
		// 演出種別(none/talk/still/rec テキスト演出を何もしない、通常会話、スチル演出、回想演出)
		public string type;
		// 会話キャラクター名(指定なしの場合は空)
		public string name;
		// 出現位置(通常会話のみ。Lが左,Rが右)
		public string LR;
		// 表情(normal/smile/angly/cry/confuse/shy/unique)
		public string expressions;
		// キャラクタ―ボイス
		public AudioClip voice;
		// 効果音を鳴らす場合は設定する
		public AudioClip SE;
		// 背景画像を変更する場合設定する
		public Sprite bgImage;
		// 背景音を変更する場合設定する 
		public AudioClip BGM;
		// Still演出を行う場合の表示画像
		public Sprite still;

	}


	public int GetStage()
	{
		return stage;
	}

	public List<SenarioTalk> GetSenarioTalks()
	{
		return senarioTalks;
	}

}
