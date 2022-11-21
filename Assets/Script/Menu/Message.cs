using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[SerializeField]
[CreateAssetMenu(fileName = "ChatBox", menuName = "Massage")]
public class Message : ScriptableObject
{
    [SerializeField]
    [Multiline(5)]
    private string[] japaneseMessageArray;

    [SerializeField]
    [Multiline(5)]
    private string[] englishMessageArray;

    [SerializeField]
    [Multiline(5)]
    private string[] chineseMessageArray;

    /// <summary>
    /// 番号に対応した言語のメッセージを返すメソッド
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string[] GetMessage(int index)
    {
        var array = japaneseMessageArray;
        switch(index)
        {
            case 0:
                array = japaneseMessageArray;
                break;
            case 1:
                array = englishMessageArray;
                break;
            case 2:
                array = chineseMessageArray;
                break;
        }
        return array;
    }
}
