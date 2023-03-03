using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefabKeys : MonoBehaviour
{
    //player status
    public const string currentStageId = "p_current_stage_id";
    public const string clearStageId = "p_clear_stage_id";
    public const string playerMoney = "p_money";
    public const string playerGetMoney = "p_get_money";
    public const string playerSaveData = "p_save_data";
    public const string playerCharacterFormation = "p_save_character_formation";
    public const string playerCharacterData = "p_save_character_data";
    public const string playTime = "p_playTime";

    //current view menu
    public const string currentMenuView = "current_menu_view";
    public const string mainMenuView = "main";
    public const string senarioMenuView = "senario";
    public const string characterMenuView = "character";
    public const string settingMenuView = "setting";
    public const string characterFormationMenuView = "characterFormation";

    //volume controll
    public const string volumeBGM = "volume_bgm";
    public const string volumeSE = "volume_se";
    public const string volumeCV = "volume_cv";

    //setting
    /// <summary>
    /// 現在の使用言語(0:日本語 | 1:英語 | 2:中国語)
    /// </summary>
    public const string currentLanguage = "current_language";

    /// <summary>
    /// 広告のモード(0:表示 | 1:非表示)
    /// </summary>
    public const string currentAdsMode = "currentAdsMode";
}