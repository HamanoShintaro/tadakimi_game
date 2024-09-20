using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideButtonController : MonoBehaviour
{
    public GameObject Canvas;

    [SerializeField]
    private MenuController menuController;

    public string buttonType;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    public void OnClick()
    {
        switch (buttonType)
        {
            case PlayerPrefabKeys.senarioMenuView:
                StartCoroutine(menuController.changeSenario());
                break;
            case PlayerPrefabKeys.characterMenuView:
                StartCoroutine(menuController.changeCharacter());
                break;
            case PlayerPrefabKeys.settingMenuView:
                StartCoroutine(menuController.changeSetting());
                break;
        }
        audioSource.Play();
    }
}
