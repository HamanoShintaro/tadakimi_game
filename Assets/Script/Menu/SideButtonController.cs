using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideButtonController : MonoBehaviour
{
    public GameObject Canvas;

    [SerializeField]
    private MenuController menuController;

    public string buttonType;
    public void OnClick()
    {
        if (buttonType == PlayerPrefabKeys.senarioMenuView)
        {
            StartCoroutine(menuController.changeSenario());
        }
        else if (buttonType == PlayerPrefabKeys.characterMenuView)
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
    /*
    public void OnClick()
    {
        // ダイアログを追加する親のCanvas
        [SerializeField] private Canvas parent = default;
        // 表示するダイアログ
        [SerializeField] private OkCancelDialog dialog = default;
        var _dialog = Instantiate(dialog);
        _dialog.transform.SetParent(parent.transform, false);
        _dialog.SetText("指定したテキストを表示します");

        // ボタンが押されたときのイベント処理
        _dialog.FixDialog = result => ChangePage();
    }

    public void ChangePage()
    {
        if (buttonType == PlayerPrefabKeys.senarioMenuView)
        {
            StartCoroutine(menuController.changeSenario());
        }
        else if (buttonType == PlayerPrefabKeys.characterMenuView)
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
    */
}
