using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "CharacterBasicInfo", menuName = "SettingCharacterBasicInfo")]
public class CharacterBasicInfo : ScriptableObject
{
	// キー
	[SerializeField]
	public string key;
	// キャラクター名
	[SerializeField]
	public string characterName;
	// 紹介
	[SerializeField]
	public string description;
	// イラスト普通
	[SerializeField]
	public Sprite normal;
	// イラスト差分(笑顔)
	[SerializeField]
	public Sprite smile;
	// イラスト差分(怒り)
	[SerializeField]
	public Sprite angry;
	// イラスト差分(泣き)
	[SerializeField]
	public Sprite cry;
	// イラスト差分(困り)
	[SerializeField]
	public Sprite confuse;
	 // イラスト差分(照れ)
	 [SerializeField]
	public Sprite unique;
	// イラスト差分(特徴)

	public string GetKey()
	{
		return key;
	}

	public string GetCharacterName()
	{
		return characterName;
	}

	public string GetDescription()
	{
		return description;
	}
	public Sprite GetNormal()
	{
		return normal;
	}
	public Sprite GetSmile()
	{
		return smile;
	}
	public Sprite GetAngry()
	{
		return angry;
	}
	public Sprite GetCry()
	{
		return cry;
	}
	public Sprite GetConfuse()
	{
		return confuse;
	}
	public Sprite GetUnique()
	{
		return unique;
	}
	public Sprite GetSprite(string expressions)
    {
		switch (expressions)
		{
			//条件１
			case "normal":
				return normal;
			case "smile":
				return smile;
			//条件３
			case "angry":
				return angry;
			case "cry":
				return cry;
			case "confuse":
				return confuse;
			case "unique":
				return unique;
			//デフォルト処理
			default:
				return normal;
		}
	}
}
