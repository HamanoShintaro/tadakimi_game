using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "BattleStageSummonEnemy", menuName = "SettingBattleStageSummonEnemy")]
public class BattleStageSummonEnemy : ScriptableObject
{
	// ステージ番号
	[SerializeField]
	public int stage;
	// 召喚時刻
	[SerializeField]
	public List<float> Times = new List<float>();
	// 生成するモンスターのプレハブ
	[SerializeField]
	public List<GameObject> Enemies = new List<GameObject>();
	// 生成するモンスターのレベル
	[SerializeField]
	public List<int> Levels = new List<int>();
	// タワーのhp
	[SerializeField]
	public int TowerHp;

	public int GetStage()
	{
		return stage;
	}

	public List<float> GetTimes()
	{
		return Times;
	}

	public List<GameObject> GetEnemies()
	{
		return Enemies;
	}
	public List<int> GetLevels()
	{
		return Levels;
	}
	public int GetTowerHp()
    {
		return TowerHp;
    }
}