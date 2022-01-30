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

    // Start is called before the first frame update
    void Start()
    {
        oneTimeFlg = true;
    }

    public void onClick()
    {
        if (oneTimeFlg)
            {
                oneTimeFlg = false;
                if (this.GetComponent<AudioSource>()) { 
                    this.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat(PlayerPrefabKeys.volumeSE);
                    this.GetComponent<AudioSource>().Play();
                }
                StartCoroutine(canvasGroupObjct.GetComponent<transitionController>().ChangeScene(canvasGroupObjct.GetComponent<CanvasGroup>(), sceneName));
            }

    }
}
