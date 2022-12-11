using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterButton : MonoBehaviour
{
    public string characterId;

    private CharacterBox characterBox;

    private void OnEnable()
    {
        characterBox = GameObject.Find("Canvas/Render/BaseMenu/CharacterBox").GetComponent<CharacterBox>();    
    }

    public void OnSelectedCharacter()
    {
        characterBox.OnSelectedCharacter(characterId);
        Debug.Log($"{characterId}をクリックした");
    }
}