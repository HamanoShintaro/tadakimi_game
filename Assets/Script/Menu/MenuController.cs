using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject BaseMenu;
    public GameObject SenarioMenu;
    public GameObject CharacterMenu;
    public GameObject SettingMenu;
    public GameObject ButtonPanel;
    public GameObject statusBar;

    private string currentView;
    private bool switchingFlg;

    private void Start()
    {
        PlayerPrefs.SetString(PlayerPrefabKeys.currentMenuView, PlayerPrefabKeys.mainMenuView);
        currentView = PlayerPrefs.GetString(PlayerPrefabKeys.currentMenuView);
        switchingFlg = false;
    }

    // SenarioMenu
    public IEnumerator changeSenario()
    {
        if (!switchingFlg)
        {
            switchingFlg = true;
            PlayerPrefs.SetString(PlayerPrefabKeys.currentMenuView, PlayerPrefabKeys.senarioMenuView);
            BaseMenu.GetComponent<Animator>().SetBool(PlayerPrefabKeys.mainMenuView, false);
            BaseMenu.GetComponent<Animator>().SetBool(PlayerPrefabKeys.senarioMenuView, true);
            yield return new WaitForSeconds(0.7f);
            SenarioMenu.SetActive(true);
            Debug.Log("シナリオを開く");
            SenarioMenu.GetComponent<Animator>().SetBool("active", true);
            switchingFlg = false;
        }
    }

    public IEnumerator backMenuFromSenario()
    {
        if (!switchingFlg)
        {
            switchingFlg = true;
            PlayerPrefs.SetString(PlayerPrefabKeys.currentMenuView, PlayerPrefabKeys.mainMenuView);
            SenarioMenu.GetComponent<Animator>().SetBool("active", false);
            yield return new WaitForSeconds(0.5f);
            SenarioMenu.SetActive(false);

            BaseMenu.GetComponent<Animator>().SetBool(PlayerPrefabKeys.senarioMenuView, false);
            BaseMenu.GetComponent<Animator>().SetBool(PlayerPrefabKeys.mainMenuView, true);
            switchingFlg = false;
        }
    }

    // CharacterMenu
    public IEnumerator changeCharacter()
    {
        if (!switchingFlg)
        {
            switchingFlg = true;
            PlayerPrefs.SetString(PlayerPrefabKeys.currentMenuView, PlayerPrefabKeys.characterMenuView);
            BaseMenu.GetComponent<Animator>().SetBool(PlayerPrefabKeys.mainMenuView, false);
            BaseMenu.GetComponent<Animator>().SetBool(PlayerPrefabKeys.characterMenuView, true);
            yield return new WaitForSeconds(0.7f);
            Debug.Log("キャラクター画面を開く");
            CharacterMenu.SetActive(true);
            CharacterMenu.GetComponent<Animator>().SetBool("active", true);
            switchingFlg = false;
        }
    }

    public IEnumerator backMenuFromCharacter()
    {
        if (!switchingFlg)
        {
            switchingFlg = true;
            PlayerPrefs.SetString(PlayerPrefabKeys.currentMenuView, PlayerPrefabKeys.mainMenuView);
            CharacterMenu.GetComponent<Animator>().SetBool("active", false);
            yield return new WaitForSeconds(0.5f);
            CharacterMenu.SetActive(false);

            BaseMenu.GetComponent<Animator>().SetBool(PlayerPrefabKeys.characterMenuView, false);
            BaseMenu.GetComponent<Animator>().SetBool(PlayerPrefabKeys.mainMenuView, true);
            switchingFlg = false;
        }
    }

    // SettingMenu
    public IEnumerator changeSetting()
    {
        if (!switchingFlg)
        {
            switchingFlg = true;
            PlayerPrefs.SetString(PlayerPrefabKeys.currentMenuView, PlayerPrefabKeys.settingMenuView);
            BaseMenu.GetComponent<Animator>().SetBool(PlayerPrefabKeys.mainMenuView, false);
            BaseMenu.GetComponent<Animator>().SetBool(PlayerPrefabKeys.settingMenuView, true);
            yield return new WaitForSeconds(0.5f);
            SettingMenu.SetActive(true);
            SettingMenu.GetComponent<Animator>().SetBool("active", true);
            switchingFlg = false;
        }
    }

    public IEnumerator backMenuFromSetting()
    {
        if (!switchingFlg)
        {
            switchingFlg = true;
            PlayerPrefs.SetString(PlayerPrefabKeys.currentMenuView, PlayerPrefabKeys.mainMenuView);
            SettingMenu.GetComponent<Animator>().SetBool("active", false);
            BaseMenu.GetComponent<Animator>().SetBool(PlayerPrefabKeys.settingMenuView, false);
            BaseMenu.GetComponent<Animator>().SetBool(PlayerPrefabKeys.mainMenuView, true);
            yield return new WaitForSeconds(1.0f);
            SettingMenu.SetActive(false);
            switchingFlg = false;
        }
    }
}
