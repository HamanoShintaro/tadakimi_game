using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "SenarioTalkScriptDateBase", menuName = "CreateSenarioTalkScriptDateBase")]
public class SenarioTalkScriptDateBase : ScriptableObject
{
    public List<SenarioTalkScript> senarioTalkScripts = new List<SenarioTalkScript>();
}