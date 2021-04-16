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

    // Start is called before the first frame update
    void Start()
    {
        buttonTransform = GetComponent<Transform>();
        this._touch_manager = new TouchManager();
    }

    // Update is called once per frame
    void Update()
    {
        buttonTransform.Rotate(0.0f,0.0f,Mathf.Sin(Time.time * 5)*spinSpeed);
    }

    public void onClick()
    {
        Debug.Log("クリックされました");
        SceneManager.LoadScene("Battle");
    }
}
