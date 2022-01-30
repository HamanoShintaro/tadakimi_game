using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
[CreateAssetMenu(fileName = "SenarioTalkScript", menuName = "SettingSenarioTalkScript")]
public class SenarioTalkScript : ScriptableObject
{
	// ?X?e?[?W????
	[SerializeField]
	public string stage;
	[SerializeField]
	public string title;
	[SerializeField]
	public List<SenarioTalk> senarioTalks = new List<SenarioTalk>();

	[System.SerializableAttribute]
	public class SenarioTalk
    {
		// ???b???e(???{??????80????)
		public string script;
		// ???o????(none/talk/still/rec ?e?L?X?g???o?????????????A???????b?A?X?`?????o?A???z???o)
		public string type;
		// ???b?L?????N?^?[??(?w????????????????)
		public string name;
		// ?o?????u(???????b?????BL????,R???E)
		public string LR;
		// ?\??(normal/smile/angly/cry/confuse/shy/unique)
		public string expressions;
		// ?L?????N?^?\?{?C?X
		public AudioClip voice;
		// ????????????????????????????
		public AudioClip SE;
		// ?w?i?????????X????????????????
		public Sprite bgImage;
		// ?w?i???????X???????????????? 
		public AudioClip BGM;
		// Still???o???s?????????\??????
		public Sprite still;

	}


	public string GetStage()
	{
		return stage;
	}

	public string GetTitle()
	{
		return title;
	}

	public List<SenarioTalk> GetSenarioTalks()
	{
		return senarioTalks;
	}

}
