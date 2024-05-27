using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "BattleStageSummonBuddy", menuName = "SettingBattleStageSummonBuddy")]
public class BattleStageSummonBuddy : ScriptableObject
{
	[SerializeField]
	public List<float> Times = new List<float>();

	[SerializeField]
	public List<GameObject> Buddies = new List<GameObject>();

	[SerializeField]
	public List<int> Levels = new List<int>();

	public List<float> GetTimes()
	{
		return Times;
	}

	public List<GameObject> GetBuddies()
	{
		return Buddies;
	}
	public List<int> GetLevels()
	{
		return Levels;
	}
}
