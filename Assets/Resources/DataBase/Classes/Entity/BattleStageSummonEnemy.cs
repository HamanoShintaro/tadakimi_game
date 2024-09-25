using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "BattleStageSummonEnemy", menuName = "SettingBattleStageSummonEnemy")]
public class BattleStageSummonEnemy : ScriptableObject
{
	[SerializeField]
	public List<float> Times = new List<float>();

	[SerializeField]
	public List<GameObject> Enemies = new List<GameObject>();

	[SerializeField]
	public List<int> Levels = new List<int>();

	[SerializeField]
	public float TowerHp;

	[SerializeField]
	[Header("ステージの背景画像")]
	private Sprite stageSprite;

	[SerializeField]
	[Header("敵タワーのオブジェクト")]
	private Sprite enemyTower;

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
	public float GetTowerHp()
    {
		return TowerHp;
    }

	public Sprite GetBackGround()
    {
		return stageSprite;
    }

	public Sprite GetEnemyTower()
    {
		return enemyTower;
    }
}