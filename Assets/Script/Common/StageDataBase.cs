using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "StageDataBase", menuName = "CreateStageDataBase")]
public class StageDataBase : ScriptableObject
{
    [Header("ステージのデータ")]
    [SerializeField]
    public List<BattleStageData> summonData;
}
