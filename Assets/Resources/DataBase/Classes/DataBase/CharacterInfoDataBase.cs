using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "CharacterInfoDataBase", menuName = "CreateCharacterInfoDataBase")]
public class CharacterInfoDataBase : ScriptableObject
{
	// キャラクター情報
	[SerializeField]
	private List<CharacterInfo> characterInfoList = new List<CharacterInfo>();

	// IDに合致するキャラクター情報を返却する
	public CharacterInfo getCharacterInfoByID(string value)
	{
		return characterInfoList.Find(characterInfo => characterInfo.id == value );
	}

	// 全てのキャラクター情報を返却する
	public List<CharacterInfo> getAllCharacterInfo()
    {
		return characterInfoList;
    }

}

