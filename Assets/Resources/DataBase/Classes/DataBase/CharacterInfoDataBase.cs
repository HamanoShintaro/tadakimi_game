using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "CharacterInfoDataBase", menuName = "CreateCharacterInfoDataBase")]
public class CharacterInfoDataBase : ScriptableObject
{
	/// <summary>
    /// キャラクター情報のデータベース
    /// </summary>
	[SerializeField]
	public List<CharacterInfo> characterInfoList = new List<CharacterInfo>();

	/// <summary>
	/// IDに合致するキャラクター情報を返却する
	/// </summary>
	/// <param name="value">取得するキャラクターのID(string)</param>
	/// <returns>保持キャラクターのリストの中のIDに一致するデータ</returns>
	public CharacterInfo GetCharacterInfoByID(string value)
	{
		return characterInfoList.Find(characterInfo => characterInfo.id == value );
	}

	/// <summary>
	/// 全てのキャラクター情報を返却する
	/// </summary>
	/// <returns>保持キャラクターのリスト</returns>
	public List<CharacterInfo> GetAllCharacterInfo()
    {
		return characterInfoList;
    }
}

