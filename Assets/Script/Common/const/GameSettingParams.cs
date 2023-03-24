using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// データを初期化した際のデータを格納するクラス
/// </summary>
public class GameSettingParams : MonoBehaviour
{
    public const string initCharacter = "Volcus_001";

    public const float characterVoiceVolume = 0.8f;
    public const float cvVolume = 0.8f;
    public const float bgmVolume = 1.0f;
    public const float seVolume = 0.7f;

    public const float inActiveColorParam = 110/255f;
    public const float ActiveColorParam = 255/255f;
    public const float ActiveScaleParam = 1.0f;
    public const float inActiveScaleParam = 0.95f;

    public const int currentLanguage = 0; //日本語が初期設定
}
