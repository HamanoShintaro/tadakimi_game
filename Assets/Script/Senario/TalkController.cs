using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkController : MonoBehaviour
{
    // トーク情報の読み込み用
    private string stageId;
    public SenarioTalkScript senarioTalkScript;
    public int num;
    private GameObject currentBublle;
    private GameObject secondBubble;


    // トーク画面に存在するオブジェクトの設定
    public GameObject canvas; // BGM再生
    private AudioSource bgmSource;
    public GameObject Render; // SE再生
    private AudioSource seSource;
    private AudioSource voiceSource;

    // トーク種別ごとのプレハブ設定
    public GameObject prefabNormal;

    // キャラクター情報の格納リスト
    public Dictionary<string, CharacterBasicInfo> characterBasicInfos = new Dictionary<string, CharacterBasicInfo>();

    // トーク画面のキャラクター表示領域の設定
    public GameObject characterL;
    public GameObject characterR;

    // 背景変更のためのオブジェクト設定
    public GameObject BackGroundObj;
    private Image BackGroundImage;

    // Start is called before the first frame update
    void Start()
    {
        // デバッグ用
        PlayerPrefs.SetString(PlayerPrefabKeys.currentStageId, "001");

        stageId = PlayerPrefs.GetString(PlayerPrefabKeys.currentStageId);
        senarioTalkScript = Resources.Load<SenarioTalkScript>(ResourcePath.senarioTalkScriptPath + stageId);
        num = -1;

        // 音声処理関連
        bgmSource = canvas.GetComponent<AudioSource>();
        bgmSource.volume = 0.0f; // 音量ゼロ固定
        seSource = Render.GetComponent<AudioSource>();
        seSource.volume = GameSettingParams.seVolume;
        voiceSource = this.GetComponent<AudioSource>();
        voiceSource.volume = GameSettingParams.characterVoiceVolume;

        // キャラクター情報ロード
        List<string> characterKeys = new List<string>();
        foreach (SenarioTalkScript.SenarioTalk senarioTalk in senarioTalkScript.senarioTalks) {
            if (!characterKeys.Contains(senarioTalk.name))
            {
                characterKeys.Add(senarioTalk.name);
                characterBasicInfos.Add(senarioTalk.name, Resources.Load<CharacterBasicInfo>(ResourcePath.characterBasicInfoPath + senarioTalk.name));
            }
        }
        // 背景Imageコンポーネントの取得
        BackGroundImage = BackGroundObj.GetComponent<Image>();
    }

    public GameObject createBubble() {
        GameObject bubble = Instantiate(prefabNormal, this.transform);
        bubble.transform.SetParent(this.gameObject.transform, false);
        return bubble;
    }

    public void nextTalk()
    {
        //Debug.Log("タップされた");
        // 次の会話が存在する場合
        num++;
        if (num < senarioTalkScript.senarioTalks.Count) {

            // typeがtalkの場合はバブルを進める
            if(senarioTalkScript.senarioTalks[num].type == "talk") {

                // キャラクターの入れ替え
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

            // 以下必ず実行する演出
            // Voiceの設定がある場合はVoiceを流す
            voiceSource.Stop();
            if (senarioTalkScript.senarioTalks[num].voice) {
                voiceSource.PlayOneShot(senarioTalkScript.senarioTalks[num].voice);
            }
            // BGMの設定がある場合はBGMを流す
            if (senarioTalkScript.senarioTalks[num].BGM)
            {
                StartCoroutine(ChangeBGM(senarioTalkScript.senarioTalks[num].BGM));
            }
            // SEの設定がある場合はSEを流す
            if (senarioTalkScript.senarioTalks[num].SE)
            {
                seSource.Stop();
                voiceSource.PlayOneShot(senarioTalkScript.senarioTalks[num].SE);
            }
            // バックグラウンドの設定がある場合はバックグラウンドを差し替える
            if (senarioTalkScript.senarioTalks[num].bgImage)
            {
                StartCoroutine(ChangeBackGround(senarioTalkScript.senarioTalks[num].bgImage));
            }
        }
        else { Debug.Log("最後の会話まで出力したよ"); }
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
