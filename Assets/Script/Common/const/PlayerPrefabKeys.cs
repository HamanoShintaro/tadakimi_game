using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのプレハブキーを定義するクラス
/// </summary>
public class PlayerPrefabKeys
{
    /// <summary>
    /// 現在のステージID
    /// </summary>
    public const string currentStageId = "p_current_stage_id";

    /// <summary>
    /// クリアしたステージID
    /// </summary>
    public const string clearStageId = "p_clear_stage_id";

    /// <summary>
    /// プレイヤーの所持金
    /// </summary>
    public const string playerMoney = "p_money";

    /// <summary>
    /// プレイヤーが獲得したお金
    /// </summary>
    public const string playerGetMoney = "p_get_money";

    /// <summary>
    /// プレイヤーのセーブデータ
    /// </summary>
    public const string playerSaveData = "p_save_data";

    /// <summary>
    /// プレイヤーのキャラクター編成
    /// </summary>
    public const string playerCharacterFormation = "p_save_character_formation";

    /// <summary>
    /// プレイヤーのキャラクターデータ
    /// </summary>
    public const string playerCharacterData = "p_save_character_data";

    /// <summary>
    /// プレイ時間
    /// </summary>
    public const string playTime = "p_playTime";

    /// <summary>
    /// 現在のメニュー表示
    /// </summary>
    public const string currentMenuView = "current_menu_view";

    /// <summary>
    /// メインメニュー表示
    /// </summary>
    public const string mainMenuView = "main";

    /// <summary>
    /// シナリオメニュー表示
    /// </summary>
    public const string senarioMenuView = "senario";

    /// <summary>
    /// キャラクターメニュー表示
    /// </summary>
    public const string characterMenuView = "character";

    /// <summary>
    /// 設定メニュー表示
    /// </summary>
    public const string settingMenuView = "setting";

    /// <summary>
    /// キャラクター編成メニュー表示
    /// </summary>
    public const string characterFormationMenuView = "characterFormation";

    /// <summary>
    /// BGMの音量
    /// </summary>
    public const string volumeBGM = "volume_bgm";

    /// <summary>
    /// SEの音量
    /// </summary>
    public const string volumeSE = "volume_se";

    /// <summary>
    /// CVの音量
    /// </summary>
    public const string volumeCV = "volume_cv";

    /// <summary>
    /// 現在の使用言語(0:日本語 | 1:英語 | 2:中国語)
    /// </summary>
    public const string currentLanguage = "current_language";

    /// <summary>
    /// 広告のモード(0:表示 | 1:非表示)
    /// </summary>
    public const string currentAdsMode = "currentAdsMode";
}