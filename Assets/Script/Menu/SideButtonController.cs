using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideButtonController : MonoBehaviour
{
    public GameObject Canvas;
    private MenuController menuController;

    public string buttonType;
    // Start is called before the first frame update
    void Start()
    {
        menuController = Canvas.GetComponent<MenuController>();
        
    }
    public void onClick() {
        if (buttonType == PlayerPrefabKeys.senarioMenuView) { 
        StartCoroutine(menuController.changeSenario());
        } else if (buttonType == PlayerPrefabKeys.characterMenuView)
        {
            StartCoroutine(menuController.changeCharacter());
        }
        else if (buttonType == PlayerPrefabKeys.settingMenuView)
        {
            StartCoroutine(menuController.changeSetting());
        }
        this.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat(PlayerPrefabKeys.volumeSE);
        this.GetComponent<AudioSource>().Play();
    }
}
