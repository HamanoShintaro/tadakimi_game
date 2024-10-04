using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// タイトル画面のテキストの挙動に関するスクリプト
public class TitleText : MonoBehaviour
{
    [Header("ブリンクの時間")]
    [SerializeField]
    private float blinkingTime;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private void OnEnable() 
    {
        StartCoroutine(BlinkImage());
    }

    private IEnumerator BlinkImage()
    {
        while (true)
        {
            for (float t = 0; t < 1; t += Time.deltaTime / blinkingTime)
            {
                canvasGroup.alpha = Mathf.Lerp(0, 1, t);
                yield return null;
            }
            for (float t = 0; t < 1; t += Time.deltaTime / blinkingTime)
            {
                canvasGroup.alpha = Mathf.Lerp(1, 0, t);
                yield return null;
            }
        }
    }

    private void OnDisable() 
    {
        StopAllCoroutines();
    }
}
