using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultController : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public bool winFlg;
    public GameObject winnerPanel;
    public GameObject loserPanel;

    private float transitionSpeed;

    public GameObject gameController;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        AudioSource audioSource = gameController.GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.PlayOneShot(clip);


        transitionSpeed = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        if (canvasGroup.alpha < 1.0f) {
            canvasGroup.alpha += transitionSpeed * Time.unscaledDeltaTime;
            if(canvasGroup.alpha >= 1.0f)
            {
                if (winFlg)
                {
                    winnerPanel.SetActive(true);
                }
                else
                {
                    loserPanel.SetActive(true);
                }
            }
        }
    }
}
