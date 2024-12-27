using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backButtonController : MonoBehaviour
{
    [SerializeField]
    private CharacterBox CharacterBox;
    public GameObject Canvas;
    private MenuController menuController;

    private AudioSource audioSource;

    private void Start()
    {
        menuController = Canvas.GetComponent<MenuController>();
        audioSource = this.GetComponent<AudioSource>();
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
        CharacterBox.CloseCharacterFormation();
        audioSource.Play();
    }
}
