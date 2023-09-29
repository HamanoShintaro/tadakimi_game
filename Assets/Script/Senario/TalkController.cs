using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkController : MonoBehaviour
{
    private string stageId;
    public SenarioTalkScript senarioTalkScript; // TODO修正
    public int num;
    private GameObject currentBublle;
    private GameObject secondBubble;
    public GameObject canvasGroupObjct;
    private bool changeSceneFlg = true;

    public GameObject canvas;
    private AudioSource bgmSource;
    public GameObject Render;
    private AudioSource seSource;
    private AudioSource voiceSource;
    private float bgmVolume;

    public GameObject prefabNormal;

    public Dictionary<string, CharacterBasicInfo> characterBasicInfos = new Dictionary<string, CharacterBasicInfo>();

    public GameObject characterL;
    public GameObject characterR;

    public GameObject BackGroundObj;
    private Image BackGroundImage;

    private void Start()
    {
        //シナリオ番号の読み込み
        stageId = PlayerPrefs.GetString(PlayerPrefabKeys.currentStageId);
        senarioTalkScript = Resources.Load<SenarioTalkScript>($"{ResourcePath.senarioTalkScriptPath}{stageId}");
        num = -1;

        bgmSource = canvas.GetComponent<AudioSource>();
        bgmSource.volume = 0.0f;
        seSource = Render.GetComponent<AudioSource>();
        seSource.volume = GameSettingParams.seVolume * PlayerPrefs.GetFloat(PlayerPrefabKeys.volumeSE);
        voiceSource = this.GetComponent<AudioSource>();
        voiceSource.volume = GameSettingParams.characterVoiceVolume * PlayerPrefs.GetFloat(PlayerPrefabKeys.volumeCV);
        bgmVolume = GameSettingParams.bgmVolume * PlayerPrefs.GetFloat(PlayerPrefabKeys.volumeBGM);

        List<string> characterKeys = new List<string>();
        foreach (SenarioTalkScript.SenarioTalk senarioTalk in senarioTalkScript.GetSenarioTalks())
        {
            if (!characterKeys.Contains(senarioTalk.name))
            {
                characterKeys.Add(senarioTalk.name);
                characterBasicInfos.Add(senarioTalk.name, Resources.Load<CharacterBasicInfo>(ResourcePath.characterBasicInfoPath + senarioTalk.name));
            }
        }
        BackGroundImage = BackGroundObj.GetComponent<Image>();
    }

    public GameObject createBubble()
    {
        GameObject bubble = Instantiate(prefabNormal, this.transform);
        bubble.transform.SetParent(this.gameObject.transform, false);
        return bubble;
    }

    public void nextTalk()
    {
        num++;
        if (num < senarioTalkScript.GetSenarioTalks().Count)
        {
            if (senarioTalkScript.GetSenarioTalks()[num].type == "talk")
            {
                if (senarioTalkScript.GetSenarioTalks()[num].LR == "L")
                {
                    characterL.GetComponent<TalkCharacterController>().CallActive();
                    characterR.GetComponent<TalkCharacterController>().CallInActive();
                }
                else
                {
                    characterR.GetComponent<TalkCharacterController>().CallActive();
                    characterL.GetComponent<TalkCharacterController>().CallInActive();
                }
                //2番目のトークを削除
                if (secondBubble)
                {
                    secondBubble.GetComponent<TalkTextController>().toDeleteTalk();
                }
                //1番目のトークを後方移動
                if (currentBublle)
                {
                    currentBublle.GetComponent<TalkTextController>().toSecondTalk();
                    secondBubble = currentBublle;
                }
                //次のトークを生成
                currentBublle = createBubble();
            }
            voiceSource.Stop();
            if (senarioTalkScript.GetSenarioTalks()[num].voice)
            {
                voiceSource.PlayOneShot(senarioTalkScript.GetSenarioTalks()[num].voice);
            }
            if (senarioTalkScript.GetSenarioTalks()[num].BGM)
            {
                StartCoroutine(ChangeBGM(senarioTalkScript.GetSenarioTalks()[num].BGM));
            }
            if (senarioTalkScript.GetSenarioTalks()[num].SE)
            {
                seSource.Stop();
                voiceSource.PlayOneShot(senarioTalkScript.GetSenarioTalks()[num].SE);
            }
            if (senarioTalkScript.GetSenarioTalks()[num].bgImage)
            {
                StartCoroutine(ChangeBackGround(senarioTalkScript.GetSenarioTalks()[num].bgImage));
            }
        }
        else
        {
            if (changeSceneFlg)
            {
                changeSceneFlg = false;
                StartCoroutine(canvasGroupObjct.GetComponent<transitionController>().ChangeScene(canvasGroupObjct.GetComponent<CanvasGroup>(), "Battle"));
            }
        }
    }

    private IEnumerator ChangeBGM(AudioClip bgm)
    {
        while (bgmSource.volume > 0)
        {
            bgmSource.volume -= Time.deltaTime * 10.0f;
            yield return new WaitForSecondsRealtime(0.03f);
        }
        bgmSource.Stop();
        bgmSource.clip = bgm;
        bgmSource.Play();
        while (bgmSource.volume < bgmVolume)
        {
            bgmSource.volume += Time.deltaTime * 1.0f;
            yield return new WaitForSecondsRealtime(0.03f);
        }
    }

    private IEnumerator ChangeBackGround(Sprite bgImage)
    {
        float colorParam;
        while (BackGroundImage.color.r > 0.0f)
        {
            colorParam = BackGroundImage.color.r - Time.deltaTime * 10.0f;
            BackGroundImage.color = new Color(colorParam, colorParam, colorParam);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        BackGroundImage.sprite = bgImage;
        while (BackGroundImage.color.r < 1.0f)
        {
            colorParam = BackGroundImage.color.r + Time.deltaTime * 10.0f;
            BackGroundImage.color = new Color(colorParam, colorParam, colorParam);
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
}
