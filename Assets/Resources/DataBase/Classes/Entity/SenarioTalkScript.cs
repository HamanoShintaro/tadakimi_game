using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
[CreateAssetMenu(fileName = "SenarioTalkScript", menuName = "SettingSenarioTalkScript")]
public class SenarioTalkScript : ScriptableObject
{
	// �X�e�[�W�ԍ�
	[SerializeField]
	public int stage;
	[SerializeField]
	public List<SenarioTalk> senarioTalks = new List<SenarioTalk>();

	[System.SerializableAttribute]
	public class SenarioTalk
    {
		// ��b���e(���{��ő�80����)
		public string script;
		// ���o���(none/talk/still/rec �e�L�X�g���o���������Ȃ��A�ʏ��b�A�X�`�����o�A��z���o)
		public string type;
		// ��b�L�����N�^�[��(�w��Ȃ��̏ꍇ�͋�)
		public string name;
		// �o���ʒu(�ʏ��b�̂݁BL����,R���E)
		public string LR;
		// �\��(normal/smile/angly/cry/confuse/shy/unique)
		public string expressions;
		// �L�����N�^�\�{�C�X
		public AudioClip voice;
		// ���ʉ���炷�ꍇ�͐ݒ肷��
		public AudioClip SE;
		// �w�i�摜��ύX����ꍇ�ݒ肷��
		public Sprite bgImage;
		// �w�i����ύX����ꍇ�ݒ肷�� 
		public AudioClip BGM;
		// Still���o���s���ꍇ�̕\���摜
		public Sprite still;

	}


	public int GetStage()
	{
		return stage;
	}

	public List<SenarioTalk> GetSenarioTalks()
	{
		return senarioTalks;
	}

}
