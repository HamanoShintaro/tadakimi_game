using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backButtonController : MonoBehaviour
{
    public GameObject Canvas;
    private MenuController menuController;
    // Start is called before the first frame update
    void Start()
    {
        menuController = Canvas.GetComponent<MenuController>();

    }
    public void onClick()
    {
        if (PlayerPrefs.GetString(PlayerPrefabKeys.currentMenuView) == PlayerPrefabKeys.senarioMenuView)
        {
            StartCoroutine(menuController.backMenuFromSenario());
        }
        else if (PlayerPrefs.GetString(PlayerPrefabKeys.currentMenuView) == PlayerPrefabKeys.characterMenuView) {
            StartCoroutine(menuController.backMenuFromCharacter());
        }
    }
}
