using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenuController : MonoBehaviour
{
    public GameObject characterNameObj;
    public GameObject characterAiliasObj;
    public GameObject characterDescriptionObj;
    public GameObject characterSkillDescriptionObj;
    public GameObject characterBackground;
    public GameObject characterEffect;
    public GameObject menuControllerObj;

    private MenuController menuController;
    private CharacterInfoDataBase characterInfoDataBase;
    private SaveController saveController;

    // Start is called before the first frame update
    void Start()
    {
        menuController = menuControllerObj.GetComponent<MenuController>();
        characterInfoDataBase = menuController.characterInfoDataBase;
        saveController = menuController.saveController;
        setCharacter(CharacterIds.volcus_01);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setCharacter(string characterId)
    {
        CharacterInfo character = characterInfoDataBase.getCharacterInfoByID(characterId);
        Debug.Log(character.name);
        characterNameObj.GetComponent<Text>().text = character.name;
        characterAiliasObj.GetComponent<Text>().text = "ーー" + character.alias;
        characterDescriptionObj.GetComponent<Text>().text = character.detail;
        characterSkillDescriptionObj.GetComponent<Text>().text = character.skill.name + "　　消費魔力：" + character.skill.cost + "\n" + character.skill.Detail;
        characterBackground.GetComponent<Image>().sprite = character.image.backGround;
        characterEffect.GetComponent<Image>().sprite = character.image.effect;
    }
}
