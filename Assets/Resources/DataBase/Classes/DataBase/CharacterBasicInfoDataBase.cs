using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharacterBasicInfoDataBase : ScriptableObject
{
	// ??????????
	[SerializeField]
	private List<CharacterBasicInfo> characterBasicInfoList = new List<CharacterBasicInfo>();
}
