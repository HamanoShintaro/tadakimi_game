using System.Collections;
using System.Collections.Generic;
using StateManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tapStart : MonoBehaviour
{
    private TouchManager _touch_manager;
    private bool oneTimeFlg;
    public GameObject canvasGroupObjct;

    // Start is called before the first frame update
    void Start()
    {
        // タッチ管理マネージャ生成
        this._touch_manager = new TouchManager();
        oneTimeFlg = false;
        StartCoroutine(canvasGroupObjct.GetComponent<transitionController>().StartTransition(canvasGroupObjct.GetComponent<CanvasGroup>()));
        StartCoroutine(waitTime());
    }

    // Update is called once per frame
    void Update()
    {
        // タッチ状態更新
        this._touch_manager.update();

        // タッチ取得
        TouchManager touch_state = this._touch_manager.getTouch();

        // タッチされていたら処理
        if (touch_state._touch_flag)
        {

            if (oneTimeFlg)
            {
                oneTimeFlg = false;
                //init user data
                initUser();
                StartCoroutine(canvasGroupObjct.GetComponent<transitionController>().ChangeScene(canvasGroupObjct.GetComponent<CanvasGroup>(), "MainMenu"));
            }

            if (touch_state._touch_phase == TouchPhase.Began)
            {

            }
        }
    }
    private void initUser() {

        // menu表示のための設定
        PlayerPrefs.SetString(PlayerPrefabKeys.currentMenuView, PlayerPrefabKeys.mainMenuView);
        // セーブデータの初期設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.currentStageId)) { PlayerPrefs.SetString(PlayerPrefabKeys.currentStageId, "101"); }
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.clearStageId)) { PlayerPrefs.SetString(PlayerPrefabKeys.clearStageId, "101"); }
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.playerMoney)) { PlayerPrefs.SetInt(PlayerPrefabKeys.playerMoney, 0); }
        // 音量の初期設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.volumeBGM)) { PlayerPrefs.SetFloat(PlayerPrefabKeys.volumeBGM, GameSettingParams.bgmVolume); }
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.volumeSE)) { PlayerPrefs.SetFloat(PlayerPrefabKeys.volumeSE, GameSettingParams.seVolume); }
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.volumeCV)) { PlayerPrefs.SetFloat(PlayerPrefabKeys.volumeCV, GameSettingParams.cvVolume); }
        // 初期キャラの設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.playerCharacterData)) {
            SaveController.CharacterSaveData.CharacterData characterData = new SaveController.CharacterSaveData.CharacterData();
            characterData.id = GameSettingParams.initCharacter;
            characterData.level = 1;
            characterData.hasSpecial = false;

            SaveController.CharacterSaveData characterSaveData = new SaveController.CharacterSaveData();
            characterSaveData.list.Add(characterData);
            characterSaveData.save();
        }
        // 初期編成の設定
        if (!PlayerPrefs.HasKey(PlayerPrefabKeys.playerCharacterFormation))
        {
            SaveController.CharacterFormation characterFormation = new SaveController.CharacterFormation();
            characterFormation.character_1_id = GameSettingParams.initCharacter;
            characterFormation.character_2_id = "none";
            characterFormation.character_3_id = "none";
            characterFormation.character_4_id = "none";

            characterFormation.save();
        }

    }

    private IEnumerator waitTime() {
        yield return new WaitForSeconds(5.0f);
        oneTimeFlg = true;
    }
}
