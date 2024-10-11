using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkController : MonoBehaviour
{
    private string stageId;
    public SenarioTalkScript senarioTalkScript;
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

    private Coroutine bgmCoroutine = null; // 排他制御用の変数

    private void Start()
    {
        // シナリオ番号の読み込み
        stageId = PlayerPrefs.GetString(PlayerPrefabKeys.currentStageId);
        senarioTalkScript = Resources.Load<SenarioTalkScript>($"{ResourcePath.senarioTalkScriptPath}{stageId}");
        num = -1;

        bgmSource = canvas.GetComponent<AudioSource>();
        bgmSource.volume = 0.0f;
        bgmVolume = GameSettingParams.bgmVolume * PlayerPrefs.GetFloat(PlayerPrefabKeys.volumeBGM);

        // デバッグログ: BGMの初期化
        Debug.Log($"[BGM] 初期化: BGM音量設定 = {bgmVolume}");

        seSource = Render.GetComponent<AudioSource>();
        seSource.volume = GameSettingParams.seVolume * PlayerPrefs.GetFloat(PlayerPrefabKeys.volumeSE);
        voiceSource = this.GetComponent<AudioSource>();
        voiceSource.volume = GameSettingParams.characterVoiceVolume * PlayerPrefs.GetFloat(PlayerPrefabKeys.volumeCV);

        // キャラクター情報の読み込み
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
                // 2番目のトークを削除
                if (secondBubble)
                {
                    secondBubble.GetComponent<TalkTextController>().toDeleteTalk();
                }
                // 1番目のトークを後方移動
                if (currentBublle)
                {
                    currentBublle.GetComponent<TalkTextController>().toSecondTalk();
                    secondBubble = currentBublle;
                }
                // 次のトークを生成
                currentBublle = createBubble();
            }

            // BGMとSEの処理前にログを出力
            Debug.Log($"[BGM] トーク番号 {num} に移行します。");

            voiceSource.Stop();
            if (senarioTalkScript.GetSenarioTalks()[num].voice)
            {
                voiceSource.PlayOneShot(senarioTalkScript.GetSenarioTalks()[num].voice);
                Debug.Log($"[Voice] トーク番号 {num} のボイスを再生します。");
            }
            if (senarioTalkScript.GetSenarioTalks()[num].BGM)
            {
                Debug.Log($"[BGM] トーク番号 {num} のBGMを変更します。BGMクリップ名: {senarioTalkScript.GetSenarioTalks()[num].BGM.name}");
                StartCoroutine(ChangeBGM(senarioTalkScript.GetSenarioTalks()[num].BGM));
            }
            if (senarioTalkScript.GetSenarioTalks()[num].SE)
            {
                seSource.Stop();
                voiceSource.PlayOneShot(senarioTalkScript.GetSenarioTalks()[num].SE);
                Debug.Log($"[SE] トーク番号 {num} のSEを再生します。SEクリップ名: {senarioTalkScript.GetSenarioTalks()[num].SE.name}");
            }
            if (senarioTalkScript.GetSenarioTalks()[num].bgImage)
            {
                StartCoroutine(ChangeBackGround(senarioTalkScript.GetSenarioTalks()[num].bgImage));
                Debug.Log($"[Background] トーク番号 {num} の背景画像を変更します。");
            }
        }
        else
        {
            if (changeSceneFlg)
            {
                changeSceneFlg = false;
                Debug.Log("[Scene] トークが終了しました。'Battle'シーンに遷移します。");
                StartCoroutine(canvasGroupObjct.GetComponent<TransitionController>().ChangeScene(canvasGroupObjct.GetComponent<CanvasGroup>(), "Battle"));
            }
        }
    }

    private IEnumerator ChangeBGM(AudioClip bgm)
    {
        if (bgmCoroutine != null)
        {
            Debug.LogWarning("[ChangeBGM] 既にBGMの変更処理が実行中です。新しい変更処理をスキップします。");
            yield break;
        }
        bgmCoroutine = StartCoroutine(ChangeBGMCoroutine(bgm));
        yield return bgmCoroutine;
    }

    private IEnumerator ChangeBGMCoroutine(AudioClip bgm)
    {
        Debug.Log($"[ChangeBGM] 新しいBGMを設定します。BGMクリップ名: {bgm.name}");
        Debug.Log($"[ChangeBGM] bgmVolumeの値: {bgmVolume}");

        // フェードアウト処理
        Debug.Log("[ChangeBGM] フェードアウト開始");
        float fadeDuration = 1.0f; // フェードアウトにかける時間（秒）
        float currentTime = 0f;
        float startVolume = bgmSource.volume;

        while (currentTime < fadeDuration)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, currentTime / fadeDuration);
            currentTime += Time.deltaTime;
            Debug.Log($"[ChangeBGM] フェードアウト中: 現在の音量 = {bgmSource.volume}");
            yield return null;
        }
        bgmSource.volume = 0f;
        bgmSource.Stop();
        Debug.Log("[ChangeBGM] フェードアウト完了。BGMを停止しました。");

        // 新しいBGMの設定と再生
        bgmSource.clip = bgm;
        bgmSource.Play();
        Debug.Log($"[ChangeBGM] 新しいBGMを再生開始: {bgm.name}");

        // フェードイン処理
        Debug.Log("[ChangeBGM] フェードイン開始");
        currentTime = 0f;
        startVolume = 0f;

        while (currentTime < fadeDuration)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, bgmVolume, currentTime / fadeDuration);
            currentTime += Time.deltaTime;
            Debug.Log($"[ChangeBGM] フェードイン中: 現在の音量 = {bgmSource.volume}");
            yield return null;
        }
        bgmSource.volume = bgmVolume;
        Debug.Log("[ChangeBGM] フェードイン完了。BGMの音量が設定されました。");

        bgmCoroutine = null; // コルーチンの終了を通知
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
