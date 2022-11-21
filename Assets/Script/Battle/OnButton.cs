using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンを切り替えるクラス
/// </summary>
public class OnButton : MonoBehaviour
{
    /// <summary>
    /// メニューシーンに切り替えるメソッドf
    /// </summary>
    public void OnChangeMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    /// <summary>
    /// シナリオシーンに切り替えるメソッド
    /// </summary>
    public void OnChangeSenario()
    {
        SceneManager.LoadScene("Senario");
    }

    /// <summary>
    /// バトルシーンに切り替えるメソッド
    /// </summary>
    public void OnChangeBattle()
    {
        SceneManager.LoadScene("Battle");
    }

    public void OpenURL()
    {
        Application.OpenURL("https://docs.google.com/spreadsheets/d/1FdHyXh00HW1Pn_1x1Mt_zSQkY_k6aPVW0v0CO2fe6Jo/edit#gid=0");
    }
}
