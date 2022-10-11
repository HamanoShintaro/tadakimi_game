using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonController : MonoBehaviour
{
    public GameObject characterMenuControllerObj;
    private CharacterMenuController characterMenuController;
    private bool isProcess;
    // Start is called before the first frame update
    void Start()
    {
        isProcess = false;
        characterMenuController = characterMenuControllerObj.GetComponent<CharacterMenuController>();
    }
    /*
    public void onClick()
    {
        if (!isProcess)
        {
            isProcess = true;
            if (characterMenuController.CharacterLevelUp())
            {

            }
            else
            {

            }
            isProcess = false;
        }
    }
    */
}
