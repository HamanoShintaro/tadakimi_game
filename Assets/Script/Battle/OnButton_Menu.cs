using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnButton_Menu : OnButton,IPointerClickHandler, IPointerDownHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("クリックされたよ");
        OnChangeMainMenu();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 処理必要なし（ただし関数の記載は必要）
    }

}
