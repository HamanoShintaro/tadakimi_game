using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkController : MonoBehaviour
{
    // �g�[�N���̓ǂݍ��ݗp
    private string stageId;
    public SenarioTalkScript senarioTalkScript;
    public int num;
    private GameObject currentBublle;
    private GameObject secondBubble;


    // �g�[�N��ʂɑ��݂���I�u�W�F�N�g�̐ݒ�
    public GameObject canvas; // BGM�Đ�
    private AudioSource bgmSource;
    public GameObject Render; // SE�Đ�
    private AudioSource seSource;
    private AudioSource voiceSource;

    // �g�[�N��ʂ��Ƃ̃v���n�u�ݒ�
    public GameObject prefabNormal;

    // �L�����N�^�[���̊i�[���X�g
    public Dictionary<string, CharacterBasicInfo> characterBasicInfos = new Dictionary<string, CharacterBasicInfo>();

    // �g�[�N��ʂ̃L�����N�^�[�\���̈�̐ݒ�
    public GameObject characterL;
    public GameObject characterR;

    // �w�i�ύX�̂��߂̃I�u�W�F�N�g�ݒ�
    public GameObject BackGroundObj;
    private Image BackGroundImage;

    // Start is called before the first frame update
    void Start()
    {
        // �f�o�b�O�p
        PlayerPrefs.SetString(PlayerPrefabKeys.currentStageId, "001");

        stageId = PlayerPrefs.GetString(PlayerPrefabKeys.currentStageId);
        senarioTalkScript = Resources.Load<SenarioTalkScript>(ResourcePath.senarioTalkScriptPath + stageId);
        num = -1;

        // ���������֘A
        bgmSource = canvas.GetComponent<AudioSource>();
        bgmSource.volume = 0.0f; // ���ʃ[���Œ�
        seSource = Render.GetComponent<AudioSource>();
        seSource.volume = GameSettingParams.seVolume;
        voiceSource = this.GetComponent<AudioSource>();
        voiceSource.volume = GameSettingParams.characterVoiceVolume;

        // �L�����N�^�[��񃍁[�h
        List<string> characterKeys = new List<string>();
        foreach (SenarioTalkScript.SenarioTalk senarioTalk in senarioTalkScript.senarioTalks) {
            if (!characterKeys.Contains(senarioTalk.name))
            {
                characterKeys.Add(senarioTalk.name);
                characterBasicInfos.Add(senarioTalk.name, Resources.Load<CharacterBasicInfo>(ResourcePath.characterBasicInfoPath + senarioTalk.name));
            }
        }
        // �w�iImage�R���|�[�l���g�̎擾
        BackGroundImage = BackGroundObj.GetComponent<Image>();
    }

    public GameObject createBubble() {
        GameObject bubble = Instantiate(prefabNormal, this.transform);
        bubble.transform.SetParent(this.gameObject.transform, false);
        return bubble;
    }

    public void nextTalk()
    {
        //Debug.Log("�^�b�v���ꂽ");
        // ���̉�b�����݂���ꍇ
        num++;
        if (num < senarioTalkScript.senarioTalks.Count) {

            // type��talk�̏ꍇ�̓o�u����i�߂�
            if(senarioTalkScript.senarioTalks[num].type == "talk") {

                // �L�����N�^�[�̓���ւ�
                if (senarioTalkScript.senarioTalks[num].LR == "L")
                {
                    StartCoroutine(characterL.GetComponent<TalkCharacterController>().Active());
                    StartCoroutine(characterR.GetComponent<TalkCharacterController>().inActive());
                }
                else
                {
                    StartCoroutine(characterR.GetComponent<TalkCharacterController>().Active());
                    StartCoroutine(characterL.GetComponent<TalkCharacterController>().inActive());
                }

                if (secondBubble)
                {
                    secondBubble.GetComponent<TalkTextController>().toDeleteTalk();
                }
                if (currentBublle)
                {
                    currentBublle.GetComponent<TalkTextController>().toSecondTalk();
                    secondBubble = currentBublle;
                }
                currentBublle = createBubble();
                
            }

            // �ȉ��K�����s���鉉�o
            // Voice�̐ݒ肪����ꍇ��Voice�𗬂�
            voiceSource.Stop();
            if (senarioTalkScript.senarioTalks[num].voice) {
                voiceSource.PlayOneShot(senarioTalkScript.senarioTalks[num].voice);
            }
            // BGM�̐ݒ肪����ꍇ��BGM�𗬂�
            if (senarioTalkScript.senarioTalks[num].BGM)
            {
                StartCoroutine(ChangeBGM(senarioTalkScript.senarioTalks[num].BGM));
            }
            // SE�̐ݒ肪����ꍇ��SE�𗬂�
            if (senarioTalkScript.senarioTalks[num].SE)
            {
                seSource.Stop();
                voiceSource.PlayOneShot(senarioTalkScript.senarioTalks[num].SE);
            }
            // �o�b�N�O���E���h�̐ݒ肪����ꍇ�̓o�b�N�O���E���h�������ւ���
            if (senarioTalkScript.senarioTalks[num].bgImage)
            {
                StartCoroutine(ChangeBackGround(senarioTalkScript.senarioTalks[num].bgImage));
            }
        }
        else { Debug.Log("�Ō�̉�b�܂ŏo�͂�����"); }
    }

    private IEnumerator ChangeBGM(AudioClip bgm) {

        while (bgmSource.volume > 0) { 
            bgmSource.volume -= Time.deltaTime * 3.0f;
            yield return new WaitForSecondsRealtime(0.03f);
        }
        bgmSource.Stop();
        bgmSource.clip = bgm;
        bgmSource.Play();
        while (bgmSource.volume < GameSettingParams.bgmVolume)
        {
            bgmSource.volume += Time.deltaTime * 1.0f;
            yield return new WaitForSecondsRealtime(0.03f);
        }

    }
    private IEnumerator ChangeBackGround(Sprite bgImage)
    {
        float colorParam;
        while(BackGroundImage.color.r > 0.0f)
        {
            colorParam = BackGroundImage.color.r - Time.deltaTime * 3.0f;
            BackGroundImage.color = new Color(colorParam, colorParam, colorParam);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        BackGroundImage.sprite = bgImage;
        while (BackGroundImage.color.r < 1.0f)
        {
            colorParam = BackGroundImage.color.r + Time.deltaTime * 3.0f;
            BackGroundImage.color = new Color(colorParam, colorParam, colorParam);
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
}
