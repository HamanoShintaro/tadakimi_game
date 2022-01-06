using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "CharacterBasicInfoDataBase", menuName = "CreateCharacterBasicInfoDataBase")]
public class CharacterBasicInfoDataBase : ScriptableObject
{
	// �L�[
	[SerializeField]
	private List<CharacterBasicInfo> characterBasicInfoList = new List<CharacterBasicInfo>();
}
