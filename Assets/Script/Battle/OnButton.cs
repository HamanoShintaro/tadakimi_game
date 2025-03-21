using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンを切り替えるクラス
/// </summary>
public class OnButton : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;
    /// <summary>
    /// メニューシーンに切り替えるメソッドf
    /// </summary>
    public void OnChangeMainMenu()
    {
        audioSource.PlayOneShot(clip);
        SceneManager.LoadScene("Menu");
    }
    /// <summary>
    /// シナリオシーンに切り替えるメソッド
    /// </summary>
    public void OnChangeSenario()
    {
        audioSource.PlayOneShot(clip);
        SceneManager.LoadScene("Senario");
    }

    /// <summary>
    /// バトルシーンに切り替えるメソッド
    /// </summary>
    public void OnChangeBattle()
    {
        // クリアステージIDが131以上の場合は処理を中断
        int clearStageId = int.Parse(PlayerPrefs.GetString(PlayerPrefabKeys.clearStageId));
        const int maxStageId = 131; // TODO: シナリオの最大ステージIDを定数で管理する
        if(clearStageId >= maxStageId) return;
        audioSource.PlayOneShot(clip);
        SceneManager.LoadScene("Battle");
    }

    /// <summary>
    /// 指定されたURLを開くメソッド
    /// </summary>
    /// <param name="url">開くURL</param>
    public void OpenURL(string url)
    {
        audioSource.PlayOneShot(clip);
        Application.OpenURL(url);
    }
}
