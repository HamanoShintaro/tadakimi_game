using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleAdsSwitcher : MonoBehaviour
{
    [SerializeField]
    private Toggle[] toggles;

    private void Start()
    {
        if (PlayerPrefs.GetInt(PlayerPrefabKeys.currentAdsMode).Equals(0))
        {
            foreach (Toggle toggle in toggles)
            {
                toggle.isOn = true;
            }
        }
        else
        {
            foreach (Toggle toggle in toggles)
            {
                toggle.isOn = false;
            }
        }
    }

    /// <summary>
    /// トグルによって広告表示モードの切り替えを行うメソッド
    /// </summary>
    /// <param name="toggle"></param>
    public void OnChangeAdsMode(Toggle toggle)
    {
        int index;
        if (toggle.isOn) index = 0;
        else index = 1;
        PlayerPrefs.SetInt(PlayerPrefabKeys.currentAdsMode, index);
    }
}
