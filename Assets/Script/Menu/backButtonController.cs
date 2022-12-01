using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backButtonController : MonoBehaviour
{
    [SerializeField]
    private CharacterBox CharacterBox;
    public GameObject Canvas;
    private MenuController menuController;

    private void Start()
    {
        menuController = Canvas.GetComponent<MenuController>();
    }

    public void OnClick()
    {
        if (PlayerPrefs.GetString(PlayerPrefabKeys.currentMenuView) == PlayerPrefabKeys.senarioMenuView)
        {
            StartCoroutine(menuController.backMenuFromSenario());
        }
        else if (PlayerPrefs.GetString(PlayerPrefabKeys.currentMenuView) == PlayerPrefabKeys.characterMenuView)
        {
            StartCoroutine(menuController.backMenuFromCharacter());
        }
        else if (PlayerPrefs.GetString(PlayerPrefabKeys.currentMenuView) == PlayerPrefabKeys.settingMenuView)
        {
            StartCoroutine(menuController.backMenuFromSetting());
        }
        else if (PlayerPrefs.GetString(PlayerPrefabKeys.currentMenuView) == PlayerPrefabKeys.characterFormationMenuView)
        {
            CharacterBox.CloseCharacterFormation();
        }
        this.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat(PlayerPrefabKeys.volumeSE);
        this.GetComponent<AudioSource>().Play();
    }
}
