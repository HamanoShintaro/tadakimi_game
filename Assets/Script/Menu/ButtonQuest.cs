using System.Collections;
using System.Collections.Generic;
using StateManager;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonQuest : MonoBehaviour
{
    public float spinSpeed;
    private Transform buttonTransform;
    private TouchManager _touch_manager;
    private bool oneTimeFlg;
    public GameObject canvasGroupObjct;

    // Start is called before the first frame update
    void Start()
    {
        buttonTransform = GetComponent<Transform>();
        oneTimeFlg = true;
        StartCoroutine(canvasGroupObjct.GetComponent<transitionController>().StartTransition(canvasGroupObjct.GetComponent<CanvasGroup>()));
    }

    // Update is called once per frame
    void Update()
    {
        buttonTransform.Rotate(0.0f,0.0f,Mathf.Sin(Time.time * 5)*spinSpeed);
    }

    public void onClick()
    {
        if (oneTimeFlg)
            {
                oneTimeFlg = false;
                StartCoroutine(canvasGroupObjct.GetComponent<transitionController>().ChangeScene(canvasGroupObjct.GetComponent<CanvasGroup>(), "Battle"));
            }

    }
}
