using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SummonData
{
	[SerializeField]
	public List<float> Times = new List<float>();

	[SerializeField]
	public List<GameObject> Characters = new List<GameObject>();

	[SerializeField]
	public List<int> Levels = new List<int>();
}

[Serializable]
[CreateAssetMenu(fileName = "BattleStageData", menuName = "SettingBattleStageData")]
public class BattleStageData: ScriptableObject
{
	[Header("敵の出現データ")]
	[SerializeField]
	public SummonData EnemyData = new SummonData();

	[Header("味方の出現データ")]
	[SerializeField]
	public SummonData BuddyData = new SummonData();

	[Header("ステージ設定")]
	[SerializeField]
	public float TowerHp;

	[SerializeField]
	[Header("勝利報酬")]
	private int victoryReward;

	[SerializeField] 
	[Header("敗北報酬")]
	private int defeatReward;

	[SerializeField]
	[Header("ステージの背景画像")]
	private Sprite stageSprite;

	[SerializeField]
	[Header("敵タワーのオブジェクト")]
	private Sprite enemyTower;

	[SerializeField]
	[Header("ステージのBGM")]
	private AudioClip stageBGM;

	[SerializeField]
	[Header("解放キャラクター")]
	private CharacterInfo unlockCharacter;

	public AudioClip GetStageBGM()
	{
		return stageBGM;
	}

	public List<float> GetEnemyTimes()
	{
		return EnemyData.Times;
	}

	public List<GameObject> GetEnemies()
	{
		return EnemyData.Characters;
	}

	public List<int> GetEnemyLevels() 
	{
		return EnemyData.Levels;
	}

	public List<float> GetBuddyTimes()
	{
		return BuddyData.Times;
	}

	public List<GameObject> GetBuddies()
	{
		return BuddyData.Characters;
	}

	public List<int> GetBuddyLevels()
	{
		return BuddyData.Levels;
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

	public int GetVictoryReward()
	{
		return victoryReward;
	}

	public int GetDefeatReward()
	{
		return defeatReward;
	}

	public CharacterInfo GetUnlockCharacter()
	{
		return unlockCharacter;
	}
}
