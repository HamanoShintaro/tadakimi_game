using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class transitionController : MonoBehaviour
{
    private float transitionSpeed = 0.01f;
    private float alpha;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public IEnumerator StartTransition(CanvasGroup canvasGroup)
    {
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0.0f;
        while (canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha = canvasGroup.alpha + transitionSpeed;
            yield return new WaitForSeconds(0.01f);
        }
        canvasGroup.interactable = true;
    }

    public IEnumerator ChangeScene(CanvasGroup canvasGroup, string sceneName)
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha = canvasGroup.alpha - transitionSpeed * 2;
            yield return new WaitForSeconds(0.01f);
        }
        SceneManager.LoadScene(sceneName);
    }
}
