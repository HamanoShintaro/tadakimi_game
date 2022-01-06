using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "BattleStageSummonEnemy", menuName = "SettingBattleStageSummonEnemy")]
public class BattleStageSummonEnemy : ScriptableObject
{
	// �X�e�[�W�ԍ�
	[SerializeField]
	public int stage;
	// ��������
	[SerializeField]
	public List<float> Times = new List<float>();
	// �������郂���X�^�[�̃v���n�u
	[SerializeField]
	public List<GameObject> Enemies = new List<GameObject>();
	// �������郂���X�^�[�̃��x��
	[SerializeField]
	public List<int> Levels = new List<int>();
	// �^���[��hp
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