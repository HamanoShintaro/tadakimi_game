using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "CharacterBasicInfo", menuName = "SettingCharacterBasicInfo")]
public class CharacterBasicInfo : ScriptableObject
{
	// ?L?[
	[SerializeField]
	public string key;
	// ?L?????N?^?[??
	[SerializeField]
	public string characterName;
	// ????
	[SerializeField]
	public string description;
	// ?C???X?g????
	[SerializeField]
	public Sprite normal;
	// ?C???X?g????(????)
	[SerializeField]
	public Sprite smile;
	// ?C???X?g????(?{??)
	[SerializeField]
	public Sprite angry;
	// ?C???X?g????(????)
	[SerializeField]
	public Sprite cry;
	// ?C???X?g????(????)
	[SerializeField]
	public Sprite confuse;
	 // ?C???X?g????(????)
	 [SerializeField]
	public Sprite unique;
	[SerializeField]
	public Sprite shyness;
	// ?C???X?g????(????)

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
	public Sprite Getshyness()
	{
		return shyness;
	}
	public Sprite GetSprite(string expressions)
    {
		switch (expressions)
		{
			//?????P
			case "normal":
				return normal;
			case "smile":
				return smile;
			//?????R
			case "angry":
				return angry;
			case "cry":
				return cry;
			case "confuse":
				return confuse;
			case "unique":
				return unique;
			case "shyness":
				return shyness;
			//?f?t?H???g????
			default:
				return normal;
		}
	}
}
