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
    [Header("クリア後からリザルト画面にいくまでの時間")]
    private int waitTime;

    [SerializeField]
    [Header("ボタン")]
    private GameObject[] buttons;

    [SerializeField]
    [Header("ガイドテキスト")]
    private Text[] texts;

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

    private  bool isGameStopped = false;

    private MagicPowerController magicPowerController;

    /// <summary>
    /// ステージ情報が格納されたクラス
    /// </summary>
    private BattleStageSummonEnemy battleStageSummonEnemy;

    void Start()
    {
        magic_level = 1;

        magic_recovery_level = 0;
        magic_recovery_adjust = 1 + magic_level * 0.2f;

        //レベルごとの回復量
        recovery_magic[1] = 5.0f * magic_recovery_adjust;
        recovery_magic[2] = 7.0f * magic_recovery_adjust;
        recovery_magic[3] = 10.0f * magic_recovery_adjust;
        recovery_magic[4] = 15.0f * magic_recovery_adjust;
        recovery_magic[5] = 22.5f * magic_recovery_adjust;
        recovery_magic[6] = 30.0f * magic_recovery_adjust;
        recovery_magic[7] = 40.0f * magic_recovery_adjust;

        //レベルごとの最大値
        max_magic[1] = 50 + (magic_level - 1) * 5;
        max_magic[2] = 100 + (magic_level - 1) * 10;
        max_magic[3] = 150 + (magic_level - 1) * 15;
        max_magic[4] = 200 + (magic_level - 1) * 20;
        max_magic[5] = 300 + (magic_level - 1) * 30;
        max_magic[6] = 400 + (magic_level - 1) * 40;
        max_magic[7] = 500 + (magic_level - 1) * 50;

        magicPowerController = magicPower.GetComponent<MagicPowerController>();

        UpMagicLevel();

        //ステージ番号を取得
        var currentStageId = PlayerPrefs.GetString(PlayerPrefabKeys.currentStageId);

        //ステージ情報(ステージ番号)が格納されたクラスを取得
        battleStageSummonEnemy = Resources.Load<BattleStageSummonEnemy>($"DataBase/Data/BattleStageSummonEnemy/{currentStageId}");

        //背景画像を設定
        backGround.sprite = battleStageSummonEnemy.GetBackGround();

        //タイマーをスタート
        StartCoroutine(StartTimer());

        //戦闘背景音の設定
        this.GetComponent<AudioSource>().volume = GameSettingParams.bgmVolume * PlayerPrefs.GetFloat(PlayerPrefabKeys.volumeBGM);
    }

    /// <summary>
    /// 戦闘時間を測るメソッド
    /// </summary>
    /// <returns></returns>
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
        //レベルを上げる
        magic_recovery_level++;
        //マジックパワーコントローラーの上限を引き上げる
        magicPowerController.maxMagicPower = max_magic[magic_recovery_level];
        //マジックパワーコントローラーの回復量を引き上げる
        magicPowerController.recoverMagicPower = recovery_magic[magic_recovery_level];
    }

    /// <summary>
    /// 戦闘終了時に呼び出すメソッド
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public void GameStop(TypeLeader type)
    {
        if (isGameStopped) return;
        isGameStopped = true;
        //ゲームのプレイ時間を保存
        if (type == TypeLeader.BuddyLeader)
        {
            PlayerPrefs.SetInt(PlayerPrefabKeys.playTime, PlayerPrefs.GetInt(PlayerPrefabKeys.playTime) + gameTimer);
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefabKeys.playTime, gameTimer);
        }
        //リザルト画面(勝利または敗北)を表示
        performancePanel.GetComponent<ResultController>().OnResultPanel(type != TypeLeader.BuddyLeader);
        StartCoroutine(AnimationMoneyUI(1));
        if (type != TypeLeader.BuddyLeader)
        {
            // 敵のリーダー(タワー)がGameStopを起動した場合は、次のステージへ進む
            NextStage();
        }
        Debug.Log("<color=red>ゲーム終了!</color>");
    }

    /// <summary>
    /// 獲得金額とトータル金額を表示する
    /// </summary>
    private void DisplayMoneyUI(int adRate = 1)
    {
        //獲得した金額
        var getMoney = rate * PlayerPrefs.GetInt(PlayerPrefabKeys.playTime) * adRate;
        getMoneyText[0].text = $"{getMoney}";
        getMoneyText[1].text = $"{getMoney}";

        //所持している金額
        var totalMoney = PlayerPrefs.GetInt(PlayerPrefabKeys.playerMoney);
        totalMoneyText[0].text = $"{totalMoney}";
        totalMoneyText[1].text = $"{totalMoney}";
    }

    /// <summary>
    /// 獲得金額とトータル金額のアニメーションをかける
    /// </summary>
    /// <param name="getMoney"></param>
    public IEnumerator AnimationMoneyUI(int adRate = 1)
    {
        DisplayMoneyUI(adRate);
        if (!PlayerPrefs.GetInt(PlayerPrefabKeys.currentAdsMode).Equals(0))
        {
            ShowButtons();
        }

        yield return new WaitForSeconds(1.0f);

        //取得した金額を計算して取得
        var getMoney = rate * PlayerPrefs.GetInt(PlayerPrefabKeys.playTime) * adRate;

        //所持している金額を取得
        var totalMoney = PlayerPrefs.GetInt(PlayerPrefabKeys.playerMoney);

        //取得金額を保存
        PlayerPrefs.SetInt(PlayerPrefabKeys.playerGetMoney, getMoney);

        yield return new WaitForSeconds(1.5f);

        //所持金額+獲得金額を所持金額を更新して、保存
        PlayerPrefs.SetInt(PlayerPrefabKeys.playerMoney, totalMoney + getMoney);
        while (getMoney >= 100)
        {
            getMoney -= 10;
            getMoneyText[0].text = $"{getMoney}";
            getMoneyText[1].text = $"{getMoney}";

            totalMoney += 10;
            totalMoneyText[0].text = $"{totalMoney}";
            totalMoneyText[1].text = $"{totalMoney}";
            yield return null;
        }
        while (getMoney > 0 && getMoney < 100)
        {
            getMoney -= 1;
            getMoneyText[0].text = $"{getMoney}";
            getMoneyText[1].text = $"{getMoney}";

            totalMoney += 1;
            totalMoneyText[0].text = $"{totalMoney}";
            totalMoneyText[1].text = $"{totalMoney}";
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);

        if (PlayerPrefs.GetInt(PlayerPrefabKeys.currentAdsMode).Equals(0))
        {
            //広告を表示する
            //TODO : GameObject.Find("GoogleAdo").GetComponent<GoogleMobileAdsDemoScript>().UserChoseToWatchAd();
            ShowButtons();
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
        //現在のステージを取得する
        var currentStageId = PlayerPrefs.GetString(PlayerPrefabKeys.currentStageId);
        var nextStageId = int.Parse(currentStageId) + 1;
        //現在のステージをクリアステージとして記録する
        PlayerPrefs.SetString(PlayerPrefabKeys.clearStageId, currentStageId);
        //次のステージを現在のステージとして記録する
        PlayerPrefs.SetString(PlayerPrefabKeys.currentStageId, nextStageId.ToString("000"));
    }
}