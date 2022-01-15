using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject BaseMenu;
    public GameObject SenarioMenu;
    public GameObject CharacterMenu;
    public GameObject ButtonPanel;

    private string currentView;
    private bool switchingFlg;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString(PlayerPrefabKeys.currentMenuView, PlayerPrefabKeys.mainMenuView);
        currentView = PlayerPrefs.GetString(PlayerPrefabKeys.currentMenuView);
        switchingFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
