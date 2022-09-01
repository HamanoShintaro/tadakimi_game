using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "BattleStageSummonEnemy", menuName = "SettingBattleStageSummonEnemy")]
public class BattleStageSummonEnemy : ScriptableObject
{
	[SerializeField]
	public int stage;

	[SerializeField]
	public List<float> Times = new List<float>();

	[SerializeField]
	public List<GameObject> Enemies = new List<GameObject>();

	[SerializeField]
	public List<int> Levels = new List<int>();

	[SerializeField]
	public int TowerHp;

	[SerializeField]
	[Header("BackGround")]
	private Sprite stageSprite;

	[SerializeField]
	[Header("EnemyTower")]
	private GameObject enemyTower;

	[SerializeField]
	[Header("EnemyTowerPosition")]
	private Vector2 position;

	[SerializeField]
	[Header("Wide")]
	private float wide;

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