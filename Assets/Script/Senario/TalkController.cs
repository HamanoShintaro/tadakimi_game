using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkController : MonoBehaviour
{
    // ?g?[?N???????????????p
    private string stageId;
    public SenarioTalkScript senarioTalkScript;
    public int num;
    private GameObject currentBublle;
    private GameObject secondBubble;
    public GameObject canvasGroupObjct;
    private bool changeSceneFlg = true;


    // ?g?[?N???????????????I?u?W?F?N?g??????
    public GameObject canvas; // BGM????
    private AudioSource bgmSource;
    public GameObject Render; // SE????
    private AudioSource seSource;
    private AudioSource voiceSource;

    // ?g?[?N???????????v???n?u????
    public GameObject prefabNormal;

    // ?L?????N?^?[???????i?[???X?g
    public Dictionary<string, CharacterBasicInfo> characterBasicInfos = new Dictionary<string, CharacterBasicInfo>();

    // ?g?[?N???????L?????N?^?[?\????????????
    public GameObject characterL;
    public GameObject characterR;

    // ?w?i???X?????????I?u?W?F?N?g????
    public GameObject BackGroundObj;
    private Image BackGroundImage;

    // Start is called before the first frame update
    void Start()
    {
        // ?f?o?b?O?p
        PlayerPrefs.SetString(PlayerPrefabKeys.currentStageId, "001");

        stageId = PlayerPrefs.GetString(PlayerPrefabKeys.currentStageId);
        senarioTalkScript = Resources.Load<SenarioTalkScript>(ResourcePath.senarioTalkScriptPath + stageId);
        num = -1;

        // ???????????A
        bgmSource = canvas.GetComponent<AudioSource>();
        bgmSource.volume = 0.0f; // ?????[??????
        seSource = Render.GetComponent<AudioSource>();
        seSource.volume = GameSettingParams.seVolume;
        voiceSource = this.GetComponent<AudioSource>();
        voiceSource.volume = GameSettingParams.characterVoiceVolume;

        // ?L?????N?^?[???????[?h
        List<string> characterKeys = new List<string>();
        foreach (SenarioTalkScript.SenarioTalk senarioTalk in senarioTalkScript.senarioTalks) {
            if (!characterKeys.Contains(senarioTalk.name))
            {
                characterKeys.Add(senarioTalk.name);
                characterBasicInfos.Add(senarioTalk.name, Resources.Load<CharacterBasicInfo>(ResourcePath.characterBasicInfoPath + senarioTalk.name));
            }
        }
        // ?w?iImage?R???|?[?l???g??????
        BackGroundImage = BackGroundObj.GetComponent<Image>();
    }

    public GameObject createBubble() {
        GameObject bubble = Instantiate(prefabNormal, this.transform);
        bubble.transform.SetParent(this.gameObject.transform, false);
        return bubble;
    }

    public void nextTalk()
    {
        //Debug.Log("?^?b?v??????");
        // ???????b??????????????
        num++;
        if (num < senarioTalkScript.senarioTalks.Count) {

            // type??talk?????????o?u?????i????
            if(senarioTalkScript.senarioTalks[num].type == "talk") {

                // ?L?????N?^?[??????????
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

            // ?????K?????s???????o
            // Voice??????????????????Voice??????
            voiceSource.Stop();
            if (senarioTalkScript.senarioTalks[num].voice) {
                voiceSource.PlayOneShot(senarioTalkScript.senarioTalks[num].voice);
            }
            // BGM??????????????????BGM??????
            if (senarioTalkScript.senarioTalks[num].BGM)
            {
                StartCoroutine(ChangeBGM(senarioTalkScript.senarioTalks[num].BGM));
            }
            // SE??????????????????SE??????
            if (senarioTalkScript.senarioTalks[num].SE)
            {
                seSource.Stop();
                voiceSource.PlayOneShot(senarioTalkScript.senarioTalks[num].SE);
            }
            // ?o?b?N?O???E???h???????????????????o?b?N?O???E???h????????????
            if (senarioTalkScript.senarioTalks[num].bgImage)
            {
                StartCoroutine(ChangeBackGround(senarioTalkScript.senarioTalks[num].bgImage));
            }
        }
        else {
            if (changeSceneFlg)
            {
                changeSceneFlg = false;
                StartCoroutine(canvasGroupObjct.GetComponent<transitionController>().ChangeScene(canvasGroupObjct.GetComponent<CanvasGroup>(), "Battle"));
            }

        }
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
