using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Setting : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Image[] language;

    [SerializeField]
    private ChatBox chatBox;

    [SerializeField]
    private Slider bgmSlider;

    [SerializeField]
    private Slider seSlider;

    [SerializeField]
    private Slider cvSlider;

    public enum Language
    {
        Japanese = 0,
        English = 1,
        Chinese = 2
    }

    private void Start()
    {
        InitializeSliders();
        AddSliderListeners();
        SetInitialValues();
    }

    private void InitializeSliders()
    {
        SetSliderValue(bgmSlider, PlayerPrefabKeys.volumeBGM, GameSettingParams.bgmVolume);
        SetSliderValue(seSlider, PlayerPrefabKeys.volumeSE, GameSettingParams.seVolume);
        SetSliderValue(cvSlider, PlayerPrefabKeys.volumeCV, GameSettingParams.cvVolume);
    }

    private void AddSliderListeners()
    {
        AddSliderListener(bgmSlider, OnBgmSliderValueChanged);
        AddSliderListener(seSlider, OnSeSliderValueChanged);
        AddSliderListener(cvSlider, OnCvSliderValueChanged);
    }

    private void SetInitialValues()
    {
        SetAudioMixerValue("BGM", PlayerPrefabKeys.volumeBGM, GameSettingParams.bgmVolume);
        SetAudioMixerValue("SE", PlayerPrefabKeys.volumeSE, GameSettingParams.seVolume);
        SetAudioMixerValue("CV", PlayerPrefabKeys.volumeCV, GameSettingParams.cvVolume);
    }

    private void SetSliderValue(Slider slider, string key, float defaultValue)
    {
        var value = Mathf.Pow(10f, PlayerPrefs.GetFloat(key, defaultValue) / 20f);
        slider.value = value;
    }

    private void SetAudioMixerValue(string parameterName, string key, float defaultValue)
    {
        float volume = PlayerPrefs.GetFloat(key, defaultValue);
        audioMixer.SetFloat(parameterName, volume);
        Debug.Log($"{key}を{volume}に変更");
    }

    private void AddSliderListener(Slider slider, UnityEngine.Events.UnityAction<float> callback)
    {
        slider.onValueChanged.AddListener(callback);
    }

    public void Reset()
    {
        PlayerPrefs.SetInt(PlayerPrefabKeys.currentStageId, 101);
        PlayerPrefs.SetInt(PlayerPrefabKeys.clearStageId, 101);
        Debug.Log("リセットが呼び出されました");
    }

    public void OnChangeLanguage(int index)
    {
        UpdateLanguageColors(index);
        PlayerPrefs.SetInt(PlayerPrefabKeys.currentLanguage, index);
        chatBox.UpdateMessageArray();
        chatBox.DisplayMessage();
        Debug.Log($"言語の変更: {index}");
    }

    private void UpdateLanguageColors(int index)
    {
        for (int i = 0; i < language.Length; i++)
        {
            language[i].color = new Color(250 / 255f, 248 / 255f, 235 / 255f, 56 / 255f);
        }
        language[index].color = new Color(154 / 255f, 132 / 255f, 0);
    }

    private void OnBgmSliderValueChanged(float value)
    {
        UpdateVolume(PlayerPrefabKeys.volumeBGM, value);
    }

    private void OnSeSliderValueChanged(float value)
    {
        UpdateVolume(PlayerPrefabKeys.volumeSE, value);
    }

    private void OnCvSliderValueChanged(float value)
    {
        UpdateVolume(PlayerPrefabKeys.volumeCV, value);
    }

    private void UpdateVolume(string key, float value)
    {
        value = (value == 0) ? -80 : Mathf.Log10(value) * 20;
        PlayerPrefs.SetFloat(key, value);
        SetInitialValues();
    }
}
