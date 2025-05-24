using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattlePhaseData
{
    [Header("召喚情報リスト")]
    [SerializeField]
    private List<SummonInfo> summonInfoList = new List<SummonInfo>();

    [Header("次フェーズへの条件:キャラクター倒す数")]
    [SerializeField]
    private int defeatedCharactersForNextPhase;

    [Header("制限時間")]
    [SerializeField]
    private float phaseTimeLimit;

    public List<SummonInfo> SummonInfoList
    {
        get { return summonInfoList; }
        set { summonInfoList = value; }
    }

    public int DefeatedCharactersForNextPhase
    {
        get { return defeatedCharactersForNextPhase; }
        set { defeatedCharactersForNextPhase = value; }
    }

    public float PhaseTimeLimit
    {
        get { return phaseTimeLimit; }
        set { phaseTimeLimit = value; }
    }
}

[Serializable]
public class SummonInfo
{
    [SerializeField]
    private float time;

    [SerializeField]
    private GameObject character;

    [SerializeField]
    private int level;

    public float Time
    {
        get { return time; }
        set { time = value; }
    }

    public GameObject Character
    {
        get { return character; }
        set { character = value; }
    }

    public int Level
    {
        get { return level; }
        set { level = value; }
    }
}