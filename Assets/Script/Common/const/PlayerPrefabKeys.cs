using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefabKeys : MonoBehaviour
{
    // clear stage history
    public const string currentStageId = "p_current_stage_id";
    public const string clearStageId = "p_clear_stage_id";

    // current view menu
    public const string currentMenuView = "currentMenuView";
    public const string mainMenuView = "main";
    public const string senarioMenuView = "senario";
    public const string characterMenuView = "character";
    public const string settingMenuView = "setting";
}