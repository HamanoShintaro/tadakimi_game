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
        SceneManager.LoadScene("Menu");
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

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
