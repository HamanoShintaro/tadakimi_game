using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
    [SerializeField]
    private float transitionSpeed = 0.005f;

    private float alpha;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
    }

    private void Start()
    {
        StartCoroutine(StartTransition(canvasGroup));
    }

    public IEnumerator StartTransition(CanvasGroup canvasGroup)
    {
        canvasGroup.interactable = false;
        while (canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha = canvasGroup.alpha + transitionSpeed;
            yield return null;
        }
        canvasGroup.interactable = true;
    }

    public IEnumerator ChangeScene(CanvasGroup canvasGroup, string sceneName, float delayTime = 0.0f)
    {
        if (delayTime > 0.0f)
        {
            yield return new WaitForSeconds(delayTime);
        }
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha = canvasGroup.alpha - transitionSpeed;
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// メニューシーンに切り替えるメソッド
    /// </summary>
    public void OnChangeMenu()
    {
        StartCoroutine(ChangeScene(canvasGroup, "Menu"));
    }
    /// <summary>
    /// シナリオシーンに切り替えるメソッド
    /// </summary>
    public void OnChangeSenario()
    {
        StartCoroutine(ChangeScene(canvasGroup, "Senario"));
    }

    /// <summary>
    /// バトルシーンに切り替えるメソッド
    /// </summary>
    public void OnChangeBattle()
    {
        StartCoroutine(ChangeScene(canvasGroup, "Battle"));
    }

    /// <summary>
    /// 指定されたURLを開くメソッド
    /// </summary>
    /// <param name="url">開くURL</param>
    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}