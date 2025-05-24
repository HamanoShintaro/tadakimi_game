using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "BattleStageData", menuName = "SettingBattleStageData")]
public class BattleStageData: ScriptableObject
{
	// バトルフェーズデータ
	[Header("味方のフェーズデータ")]
	[SerializeField] private List<BattlePhaseData> buddyBattlePhases = new List<BattlePhaseData>();
	
	[Header("敵のフェーズデータ")]
	[SerializeField] private List<BattlePhaseData> enemyBattlePhases = new List<BattlePhaseData>();
	
	// プロパティ
	public List<BattlePhaseData> BuddyBattlePhases
	{
		get { return buddyBattlePhases; }
		set { buddyBattlePhases = value; }
	}
	
	public List<BattlePhaseData> EnemyBattlePhases
	{
		get { return enemyBattlePhases; }
		set { enemyBattlePhases = value; }
	}
	
	// ステージ設定
	[Header("ステージ設定")]
	[SerializeField] public float TowerHp;
	
	[Header("勝利報酬")]
	[SerializeField] private int victoryReward;
	
	[Header("敗北報酬")]
	[SerializeField] private int defeatReward;
	
	[Header("ステージの背景画像")]
	[SerializeField] private Sprite stageSprite;
	
	[Header("敵タワーのオブジェクト")]
	[SerializeField] private Sprite enemyTower;
	
	[Header("ステージのBGM")]
	[SerializeField] private AudioClip stageBGM;
	
	[Header("解放キャラクター")]
	[SerializeField] private CharacterInfo unlockCharacter;
	
	// ゲッターメソッド
	public float GetTowerHp() { return TowerHp; }
	public int GetVictoryReward() { return victoryReward; }
	public int GetDefeatReward() { return defeatReward; }
	public Sprite GetBackGround() { return stageSprite; }
	public Sprite GetEnemyTower() { return enemyTower; }
	public AudioClip GetStageBGM() { return stageBGM; }
	public CharacterInfo GetUnlockCharacter() { return unlockCharacter; }
}
