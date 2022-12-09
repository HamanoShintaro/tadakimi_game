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
    public int gameTimer = 0;

    [SerializeField]
    private Image backGround;

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
        var currentStageId = PlayerPrefs.GetInt(PlayerPrefabKeys.currentStageId).ToString("000");

        //ステージ情報(ステージ番号)が格納されたクラスを取得
        battleStageSummonEnemy = Resources.Load<BattleStageSummonEnemy>($"DataBase/Data/BattleStageSummonEnemy/{currentStageId}");

        //背景画像を設定
        backGround.sprite = battleStageSummonEnemy.GetBackGround();

        //タイマーをスタート
        StartCoroutine(StartTimer());

        //ゲームのプレイ時間をリセットする
        var playTime = PlayerPrefs.GetInt(PlayerPrefabKeys.playTime);
        PlayerPrefs.SetInt(PlayerPrefabKeys.playTime, playTime + gameTimer);
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
    /// 戦闘を終了するメソッド
    /// </summary>
    /// <param name="type">味方or敵</param>
    public void GameStop(TypeLeader type)
    {
        StartCoroutine(GameStopCoroutine(type));
    }

    /// <summary>
    /// 取得金額/トータル金額/プレイ時間/ステージ情報を計算後、リザルトパネルを表示するメソッド(味方or敵のリーダーのHPが0で呼び出す)
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private IEnumerator GameStopCoroutine(TypeLeader type)
    {
        if (type == TypeLeader.AllyLeader)
        {
            //ゲームのプレイ時間を記録
            var playTime = PlayerPrefs.GetInt(PlayerPrefabKeys.playTime);
            PlayerPrefs.SetInt(PlayerPrefabKeys.playTime, playTime + gameTimer);

            //取得した金額を計算
            UpdateMoneyUI();

            //リザルト画面を表示
            GameObject.Find("Canvas/Render/PerformancePanel").GetComponent<ResultController>().OnResultPanel(false);
        }
        else
        {
            //ゲームのプレイ時間をリセット
            PlayerPrefs.SetInt(PlayerPrefabKeys.playTime, 0);

            UpdateMoneyUI();
            NextStage();

            //リザルト画面を表示
            GameObject.Find("Canvas/Render/PerformancePanel").GetComponent<ResultController>().OnResultPanel(true);

            //TODO"広告を見る"パネルを表示
            //=>this.GetComponent<GoogleAdmobAd>().UserChoseToWatchAd();
        }
        /*TODO消していいかも
        yield return new WaitForSeconds(3f);
        Time.timeScale = 0;
        */
        //TODO動きを止める
        yield return new WaitForSeconds(0.5f);
        Debug.Log("終了");
    }

    /// <summary>
    /// 取得金額とトータル金額を表示する
    /// </summary>
    /// <param name="getMoney"></param>
    private void UpdateMoneyUI()
    {
        //取得した金額を計算
        var getMoney = 1 * gameTimer;

        getMoneyText[0].text = $"{getMoney}";
        getMoneyText[1].text = $"{getMoney}";
        //取得金額をセーブ
        PlayerPrefs.SetInt(PlayerPrefabKeys.playerGetMoney, getMoney);

        //トータル金額を計算
        int totalMoney = PlayerPrefs.GetInt(PlayerPrefabKeys.playerMoney) + getMoney;
        totalMoneyText[0].text = $"{totalMoney}";
        totalMoneyText[1].text = $"{totalMoney}";
    }

    /// <summary>
    /// 現在のステージとクリアステージを記録する
    /// </summary>
    private void NextStage()
    {
        //現在のステージを取得する
        int currentStageId = PlayerPrefs.GetInt(PlayerPrefabKeys.currentStageId);
        //現在のステージをクリアステージとして記録する
        PlayerPrefs.SetInt(PlayerPrefabKeys.clearStageId, currentStageId);
        //次のステージを現在のステージとして記録する
        PlayerPrefs.SetInt(PlayerPrefabKeys.currentStageId, currentStageId + 1);
    }
}
