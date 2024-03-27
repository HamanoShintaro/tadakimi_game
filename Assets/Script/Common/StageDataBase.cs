using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "StageDataBase", menuName = "CreateStageDataBase")]
public class StageDataBase : ScriptableObject
{
    [Header("ステージの召喚敵")]
    [SerializeField]
    public List<BattleStageSummonEnemy> battleStageSummonEnemies;

    [Header("ステージの召喚仲間")]
    [SerializeField]
    public List<BattleStageSummonBuddy> battleStageSummonBuddies;
}
