using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterButton : MonoBehaviour
{
    public string characterId;

    private CharacterBox characterBox;

    private void OnEnable()
    {
        characterBox = GameObject.Find("Canvas/Render/BaseMenu/CharaBox").GetComponent<CharacterBox>();    
    }

    /// <summary>
    /// キャラクター編成画面でボタンが選択されたら呼び出すメソッド
    /// </summary>
    public void OnSelectedCharacter()
    {
        characterBox.OnSelectedCharacter(characterId);
    }
}