using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Battle.Dominator;

/// <summary>
/// バトルシーンの管理をするクラス
/// </summary>
public class BattleController : MonoBehaviour
{
    [SerializeField]
    [Header("獲得金額テキスト")]
    private Text[] getMoneyText;

    [SerializeField]
    [Header("トータル金額テキスト")]
    private Text[] totalMoneyText;

    [SerializeField]
    [Header("プレイ時間に乗算するレート")]
    [Tooltip("獲得金額=プレイ時間 * rate")]
    [Min(1)]
    private int rate = 10;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip animationSound;

    [SerializeField]
    [Header("クリア後からリザルト画面にいくまでの時間")]
    private int waitTime;

    [SerializeField]
    [Header("ボタン")]
    private GameObject[] buttons;

    [SerializeField]
    [Header("ガイドテキスト")]
    private Text[] texts;

    [SerializeField]
    private Button doubleSpeedButton;

    [SerializeField]
    private Text doubleSpeedText;

    [SerializeField]
    private GameObject performancePanel;

    private Dictionary<int, float> recovery_magic = new Dictionary<int, float>();
    private Dictionary<int, int> max_magic = new Dictionary<int, int>();

    [Header("マジックステータス")]
    public float magic_power;
    public int magic_level;
    public int magic_recovery_level;
    private float magic_recovery_adjust;

    [SerializeField, HideInInspector]
    private GameObject magicPower;

    [SerializeField]
    [Header("ゲームタイマー")]
    public int gameTimer;

    [SerializeField]
    private Image backGround;

    [SerializeField]
    private Sprite play;

    [SerializeField]
    private Sprite doubleSpeed;

    private bool isGameStopped = false;

    private MagicPowerController magicPowerController;

    /// <summary>
    /// ステージ情報が格納されたクラス
    /// </summary>
    private BattleStageSummonEnemy battleStageSummonEnemy;

    void Start()
    {
        InitializeMagicSystem();
        InitializeStageSettings();
        InitializeAudioSettings();
        doubleSpeedButton.onClick.AddListener(ToggleTimeScale);
    }

    private void InitializeMagicSystem()
    {
        magic_level = 1;
        magic_recovery_level = 0;
        magic_recovery_adjust = 1 + magic_level * 0.2f;

        InitializeMagicRecoveryValues();
        InitializeMagicMaxValues();

        magicPowerController = magicPower.GetComponent<MagicPowerController>();
        UpMagicLevel();
    }

    private void InitializeMagicRecoveryValues()
    {
        recovery_magic[1] = 5.0f * magic_recovery_adjust;
        recovery_magic[2] = 7.0f * magic_recovery_adjust;
        recovery_magic[3] = 10.0f * magic_recovery_adjust;
        recovery_magic[4] = 15.0f * magic_recovery_adjust;
        recovery_magic[5] = 22.5f * magic_recovery_adjust;
        recovery_magic[6] = 30.0f * magic_recovery_adjust;
        recovery_magic[7] = 40.0f * magic_recovery_adjust;
    }

    private void InitializeMagicMaxValues()
    {
        max_magic[1] = 50 + (magic_level - 1) * 5;
        max_magic[2] = 100 + (magic_level - 1) * 10;
        max_magic[3] = 150 + (magic_level - 1) * 15;
        max_magic[4] = 200 + (magic_level - 1) * 20;
        max_magic[5] = 300 + (magic_level - 1) * 30;
        max_magic[6] = 400 + (magic_level - 1) * 40;
        max_magic[7] = 500 + (magic_level - 1) * 50;
    }

    private void InitializeStageSettings()
    {
        var currentStageId = PlayerPrefs.GetString(PlayerPrefabKeys.currentStageId);
        battleStageSummonEnemy = Resources.Load<BattleStageSummonEnemy>($"DataBase/Data/BattleStageSummonEnemy/{currentStageId}");
        backGround.sprite = battleStageSummonEnemy.GetBackGround();
        StartCoroutine(StartTimer());
    }

    private void InitializeAudioSettings()
    {
        this.GetComponent<AudioSource>().volume = GameSettingParams.bgmVolume * PlayerPrefs.GetFloat(PlayerPrefabKeys.volumeBGM);
    }

    /// <summary>
    /// 戦闘時間を測るメソッド
    /// </summary>
    private IEnumerator StartTimer()
    {
        var wait = new WaitForSeconds(1f);
        while(true)
        {
            yield return wait;
            gameTimer++;
        }
    }

    /// <summary>
    /// レベルアップしたら、マジックパワーのレベルを上げるメソッド
    /// </summary>
    public void UpMagicLevel()
    {
        magic_recovery_level++;
        magicPowerController.maxMagicPower = max_magic[magic_recovery_level];
        magicPowerController.recoverMagicPower = recovery_magic[magic_recovery_level];
    }

    /// <summary>
    /// 戦闘終了時に呼び出すメソッド
    /// </summary>
    public void GameStop(TypeLeader type)
    {
        if (isGameStopped) return;
        isGameStopped = true;

        Time.timeScale = 1.0f;
        SavePlayTime(type);
        
        bool isVictory = type != TypeLeader.BuddyLeader;
        performancePanel.GetComponent<ResultController>().OnResultPanel(isVictory);
        
        int reward = CalculateReward(isVictory);
        StartCoroutine(AnimationMoneyUI(reward));

        if (isVictory)
        {
            NextStage();
        }
        
        Debug.Log("<color=red>ゲーム終了!</color>");
    }

    private void SavePlayTime(TypeLeader type)
    {
        if (type == TypeLeader.BuddyLeader)
        {
            PlayerPrefs.SetInt(PlayerPrefabKeys.playTime, PlayerPrefs.GetInt(PlayerPrefabKeys.playTime) + gameTimer);
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefabKeys.playTime, gameTimer);
        }
    }

    private int CalculateReward(bool isVictory)
    {
        if (isVictory)
        {
            return battleStageSummonEnemy.GetVictoryReward();
        }
        else
        {
            return (PlayerPrefs.GetInt(PlayerPrefabKeys.playTime) / 120) * battleStageSummonEnemy.GetDefeatReward();
        }
    }

    /// <summary>
    /// 獲得金額とトータル金額を表示する
    /// </summary>
    private void DisplayMoneyUI(int reward)
    {
        foreach (var text in getMoneyText)
        {
            text.text = reward.ToString();
        }

        var totalMoney = PlayerPrefs.GetInt(PlayerPrefabKeys.playerMoney);
        foreach (var text in totalMoneyText)
        {
            text.text = totalMoney.ToString();
        }
    }

    /// <summary>
    /// 獲得金額とトータル金額のアニメーションをかける
    /// </summary>
    public IEnumerator AnimationMoneyUI(int reward)
    {
        DisplayMoneyUI(reward);
        
        if (!PlayerPrefs.GetInt(PlayerPrefabKeys.currentAdsMode).Equals(0))
        {
            ShowButtons();
        }

        yield return new WaitForSeconds(1.0f);

        var totalMoney = PlayerPrefs.GetInt(PlayerPrefabKeys.playerMoney);
        totalMoney += reward;
        PlayerPrefs.SetInt(PlayerPrefabKeys.playerMoney, totalMoney);

        yield return StartCoroutine(AnimateMoneyCount(reward, totalMoney));

        if (PlayerPrefs.GetInt(PlayerPrefabKeys.currentAdsMode).Equals(0))
        {
            ShowButtons();
        }
    }

    private IEnumerator AnimateMoneyCount(int reward, int totalMoney)
    {
        int displayedTotalMoney = totalMoney - reward;
        int displayedReward = reward;

        yield return new WaitForSeconds(1.5f);

        float animationDuration = 1f;
        int animationSteps = 30;
        float stepDuration = animationDuration / animationSteps;
        int increment = Mathf.CeilToInt((float)reward / animationSteps);

        audioSource.PlayOneShot(animationSound);

        for (int i = 0; i < animationSteps && displayedReward > 0; i++)
        {
            int stepValue = Mathf.Min(increment, displayedReward);
            displayedReward -= stepValue;
            displayedTotalMoney += stepValue;

            UpdateMoneyTexts(displayedReward, displayedTotalMoney);

            yield return new WaitForSeconds(stepDuration);
        }

        UpdateMoneyTexts(0, totalMoney);
    }

    private void UpdateMoneyTexts(int displayedReward, int displayedTotalMoney)
    {
        foreach (var text in getMoneyText)
        {
            text.text = displayedReward.ToString();
        }

        foreach (var text in totalMoneyText)
        {
            text.text = displayedTotalMoney.ToString();
        }
    }

    private void ShowButtons()
    {
        foreach(GameObject button in buttons)
        {
            button.SetActive(true);
        }
    }

    /// <summary>
    /// 現在のステージとクリアステージを記録する
    /// </summary>
    private void NextStage()
    {
        var currentStageId = PlayerPrefs.GetString(PlayerPrefabKeys.currentStageId);
        var nextStageId = int.Parse(currentStageId) + 1;
        PlayerPrefs.SetString(PlayerPrefabKeys.clearStageId, currentStageId);
        PlayerPrefs.SetString(PlayerPrefabKeys.currentStageId, nextStageId.ToString("000"));
    }

    /// <summary>
    /// 2倍速ボタンが押された時の処理
    /// </summary>
    public void ToggleTimeScale()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 2;
            doubleSpeedText.text = "▶︎";
            Debug.Log("Time scale set to 2");
        }
        else
        {
            Time.timeScale = 1;
            doubleSpeedText.text = "▶︎▶︎";
            StopAllCoroutines();
            Debug.Log("Time scale set to 1");
        }
    }

    private void OnDestroy() 
    {
        Time.timeScale = 1;
    }
}