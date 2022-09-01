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

    private MagicPowerController magicPowerController;

    [Header("ゲームタイマー")]
    public int gameTimer = 0;

    void Start()
    {
        magic_level = 1;

        magic_recovery_level = 0;
        magic_recovery_adjust = 1 + magic_level * 0.2f;
        
        recovery_magic[1] = 5.0f * magic_recovery_adjust;
        recovery_magic[2] = 7.0f * magic_recovery_adjust;
        recovery_magic[3] = 10.0f * magic_recovery_adjust;
        recovery_magic[4] = 15.0f * magic_recovery_adjust;
        recovery_magic[5] = 22.5f * magic_recovery_adjust;
        recovery_magic[6] = 30.0f * magic_recovery_adjust;
        recovery_magic[7] = 40.0f * magic_recovery_adjust;

        max_magic[1] = 50 + (magic_level - 1) * 5;
        max_magic[2] = 100 + (magic_level - 1) * 10;
        max_magic[3] = 150 + (magic_level - 1) * 15;
        max_magic[4] = 200 + (magic_level - 1) * 20;
        max_magic[5] = 300 + (magic_level - 1) * 30;
        max_magic[6] = 400 + (magic_level - 1) * 40;
        max_magic[7] = 500 + (magic_level - 1) * 50;

        magicPowerController = magicPower.GetComponent<MagicPowerController>();

        UpMagicLevel();

        //タイマーをスタート
        StartCoroutine(StartTimer());

        //ゲームのプレイ時間をリセットする
        var playTime = PlayerPrefs.GetInt(PlayerPrefabKeys.playTime);
        PlayerPrefs.SetInt(PlayerPrefabKeys.playTime, playTime + gameTimer);
    }

    private IEnumerator StartTimer()
    {
        var wait = new WaitForSeconds(1f);
        while(true)
        {
            yield return wait;
            gameTimer++;
            //Debug.Log(gameTimer);
        }
    }

    public void UpMagicLevel()
    {
        magic_recovery_level = magic_recovery_level + 1;
        magicPowerController.maxMagicPower = max_magic[magic_recovery_level];
        magicPowerController.recoverMagicPower = recovery_magic[magic_recovery_level];
    }

    public void GameStop(TypeLeader type)
    {
        StartCoroutine(GameStopCoroutine(type));
    }

    /// <summary>
    /// バトルシーンを止める処理(味方or敵のリーダーのHPが0で呼び出す)
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
            var getMoney = 1 * gameTimer;//TODO50のマジックナンバー
            UpdateUI(getMoney);

            //リザルト画面を表示
            GameObject.Find("Canvas/Render/PerformancePanel").GetComponent<ResultController>().OnResultPanel(false);
        }
        else
        {
            //ゲームのプレイ時間をリセット
            PlayerPrefs.SetInt(PlayerPrefabKeys.playTime, 0);

            //取得した金額を計算
            var getMoney = 1 * gameTimer;//TODO50のマジックナンバー
            UpdateUI(getMoney);

            //現在のステージを取得する
            int clearStageId = PlayerPrefs.GetInt(PlayerPrefabKeys.clearStageId);
            //次のステージを記録する
            PlayerPrefs.SetInt(PlayerPrefabKeys.clearStageId, clearStageId + 1);
            //リザルト画面を表示
            GameObject.Find("Canvas/Render/PerformancePanel").GetComponent<ResultController>().OnResultPanel(true);
        }

       

        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 0;
        Debug.Log("終了");
    }

    private void UpdateUI(int getMoney)
    {
        getMoneyText[0].text = $"{getMoney}";
        getMoneyText[1].text = $"{getMoney}";
        //取得金額をセーブ
        PlayerPrefs.SetInt(PlayerPrefabKeys.playerGetMoney, getMoney);

        //トータル金額を計算
        int totalMoney = PlayerPrefs.GetInt(PlayerPrefabKeys.playerMoney) + getMoney;
        totalMoneyText[0].text = $"{totalMoney}";
        totalMoneyText[1].text = $"{totalMoney}";
    }
}
