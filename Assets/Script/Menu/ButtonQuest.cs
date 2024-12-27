using System.Collections;
using System.Collections.Generic;
using StateManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonQuest : MonoBehaviour
{
    private bool oneTimeFlg;
    public GameObject canvasGroupObjct;
    public string sceneName;

    private AudioSource audioSource;

    void Start()
    {
        oneTimeFlg = true;
        audioSource = this.GetComponent<AudioSource>();
    }

    public void OnClick()
    {
        if (oneTimeFlg)
        {
            oneTimeFlg = false;
            /*
            if (this.GetComponent<AudioSource>())
            {
                this.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat(PlayerPrefabKeys.volumeSE);
                this.GetComponent<AudioSource>().Play();
            }
            */
            StartCoroutine(canvasGroupObjct.GetComponent<TransitionController>().ChangeScene(canvasGroupObjct.GetComponent<CanvasGroup>(), sceneName));
        }
        audioSource.Play();
    }
}
