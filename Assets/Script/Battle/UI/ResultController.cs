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

    [Header("キャラクター")]
    [SerializeField] 
    private GameObject character;

    [Header("メインUI要素")]
    [SerializeField] 
    private GameObject main;

    [SerializeField] 
    private GameObject gold;

    private bool endFlg = false;

    [SerializeField]
    private AudioSource audioSource;

    public AudioClip characterVoice;

    /// <summary>
    /// リザルトパネルを表示・非表示する
    /// </summary>
    /// <param name="isWinner">勝利 or 敗北</param>
    /// <param name="isOn">表示 or 非表示</param>
    public void OnResultPanel(bool isWinner = true, bool isOn = true)
    {
        if (isWinner) 
        {
            winningPanel.SetActive(isOn);
            InitializeUIElements();
            StartCoroutine(ControllPanel());
        }
        else 
        {
            loserPanel.SetActive(isOn);
        }
    }

    private void InitializeUIElements()
    {
        character.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        main.GetComponent<Text>().text = "";
        gold.GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }

    private IEnumerator ControllPanel()
    {
        yield return StartCoroutine(Floating(character));
        yield return new WaitForSecondsRealtime(0.5f);
        audioSource.PlayOneShot(characterVoice);

        yield return StartCoroutine(TextApear(main));
        yield return new WaitForSecondsRealtime(1.0f);

        yield return StartCoroutine(Floating(gold));
        yield return new WaitForSecondsRealtime(0.5f);
    }

    private IEnumerator Floating(GameObject obj)
    {
        RectTransform rect = obj.GetComponent<RectTransform>();
        Vector3 org = rect.position;
        Vector3 apper = new Vector3(org.x - 40, org.y, org.z);
        Image image = obj.GetComponent<Image>();
        Color orgColor = image.color;

        rect.position = apper;
        image.color = new Color(orgColor.r, orgColor.g, orgColor.b, 0);

        for (int i = 1; i <= 10; i++)
        {
            rect.position = new Vector3(rect.position.x + 4, org.y, org.z);
            image.color = new Color(orgColor.r, orgColor.g, orgColor.b, image.color.a + 0.1f);
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    private IEnumerator TextApear(GameObject obj)
    {
        string clearText = "Stage Clear";
        Text target = obj.GetComponent<Text>();
        target.text = "";

        for (int i = 0; i < clearText.Length; i++)
        {
            target.text += clearText[i];
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}