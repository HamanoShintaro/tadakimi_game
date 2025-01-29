using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルト画面に変更を加える処理
/// </summary>
public class ResultController : MonoBehaviour
{
    [Header("パネル")]
    [SerializeField] 
    private GameObject winningPanel;

    [SerializeField] 
    private GameObject loserPanel;

    [Header("勝利キャラクター関連")]
    [SerializeField] 
    private GameObject winCharacter;      // 勝利キャラ画像など
    [SerializeField] 
    private GameObject winMain;           // 勝利時のメインテキスト
    [SerializeField] 
    private GameObject winGold;           // 勝利時のゴールド表示

    [Header("敗北キャラクター関連")]
    [SerializeField] 
    private GameObject loseCharacter;     // 敗北キャラ画像など
    [SerializeField] 
    private GameObject loseMain;          // 敗北時のメインテキスト
    [SerializeField] 
    private GameObject loseGold;          // 敗北時のゴールド(必要な場合のみ)

    [Header("サウンド関連")]
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip winCharacterVoice;      // 勝利時ボイス
    [SerializeField]
    private AudioClip loseCharacterVoice;  // 敗北時ボイス（必要であれば追加）

    private bool endFlg = false;

    /// <summary>
    /// リザルトパネルを表示・非表示する
    /// </summary>
    /// <param name="isWinner">勝利 or 敗北</param>
    /// <param name="isOn">表示 or 非表示</param>
    public void OnResultPanel(bool isWinner = true, bool isOn = true)
    {
        if (isWinner) 
        {
            // 勝利パネルをアクティブ
            winningPanel.SetActive(isOn);
            // 勝利用UI要素初期化
            InitializeWinUIElements();
            // 演出コルーチン開始
            StartCoroutine(ControlWinPanel());
        }
        else 
        {
            // 敗北パネルをアクティブ
            loserPanel.SetActive(isOn);
            // 敗北用UI要素初期化
            InitializeLoseUIElements();
            // 演出コルーチン開始
            StartCoroutine(ControlLosePanel());
        }
    }

    /// <summary>
    /// 勝利時のUI要素を初期化
    /// </summary>
    private void InitializeWinUIElements()
    {
        // キャラクター画像の透明度リセット
        winCharacter.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        // メインテキストの内容リセット
        winMain.GetComponent<Text>().text = "";
        // ゴールドの透明度リセット
        winGold.GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }

    /// <summary>
    /// 敗北時のUI要素を初期化
    /// </summary>
    private void InitializeLoseUIElements()
    {
        // キャラクター画像の透明度リセット
        loseCharacter.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        // メインテキストの内容リセット
        loseMain.GetComponent<Text>().text = "";
        // ゴールドなどが必要な場合のみ。不要なら削除または無視。
        loseGold.GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }

    /// <summary>
    /// 勝利パネルのアニメーション管理
    /// </summary>
    private IEnumerator ControlWinPanel()
    {
        // キャラクター画像をスライドイン
        yield return StartCoroutine(Floating(winCharacter));

        // ボイス再生
        yield return new WaitForSecondsRealtime(0.5f);
        if (winCharacterVoice != null)
        {
            audioSource.PlayOneShot(winCharacterVoice);
        }

        // テキスト表示
        StartCoroutine(TextAppear(winMain, "Stage Clear"));

        // ゴールドスライドイン
        yield return StartCoroutine(Floating(winGold));
        yield return new WaitForSecondsRealtime(0.5f);
    }

    /// <summary>
    /// 敗北パネルのアニメーション管理
    /// </summary>
    private IEnumerator ControlLosePanel()
    {
        // キャラクター画像をスライドイン
        yield return StartCoroutine(Floating(loseCharacter));

        // ボイス再生
        yield return new WaitForSecondsRealtime(0.5f);
        if (loseCharacterVoice != null)
        {
            audioSource.PlayOneShot(loseCharacterVoice);
        }

        // テキスト表示(例: "Stage Failed" など)
        StartCoroutine(TextAppear(loseMain, "You Lose"));

        // 必要であればゴールドや他のオブジェクトもスライドイン
        yield return StartCoroutine(Floating(loseGold));
        yield return new WaitForSecondsRealtime(0.5f);
    }

    /// <summary>
    /// オブジェクトをスライドインしながらフェードインする
    /// </summary>
    private IEnumerator Floating(GameObject obj)
    {
        RectTransform rect = obj.GetComponent<RectTransform>();
        Vector3 org = rect.position;
        Vector3 appear = new Vector3(org.x - 40, org.y, org.z);
        Image image = obj.GetComponent<Image>();
        Color orgColor = image.color;

        // 初期位置と初期色設定
        rect.position = appear;
        image.color = new Color(orgColor.r, orgColor.g, orgColor.b, 0);

        // 10フレーム(10回)かけて移動＆フェードイン
        for (int i = 1; i <= 10; i++)
        {
            rect.position = new Vector3(rect.position.x + 4, org.y, org.z);
            image.color = new Color(orgColor.r, orgColor.g, orgColor.b, image.color.a + 0.1f);
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    /// <summary>
    /// テキストを一文字ずつ表示する
    /// </summary>
    private IEnumerator TextAppear(GameObject obj, string displayText)
    {
        Text target = obj.GetComponent<Text>();
        target.text = "";

        for (int i = 0; i < displayText.Length; i++)
        {
            target.text += displayText[i];
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}