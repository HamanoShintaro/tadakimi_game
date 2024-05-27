using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// マジックがたまるオブジェクトにアタッチするスクリプト
/// </summary>
public class MagicPowerController : MonoBehaviour
{
    public float magicPower;
    public int maxMagicPower;
    public float recoverMagicPower;

    public GameObject guageObject;
    public GameObject valueObject;

    private Image guage;
    private Text text;

    private void Start()
    {
        magicPower = 0.0f;
        guage = guageObject.GetComponent<Image>();
        text = valueObject.GetComponent<Text>();
    }
    private void Update()
    {
        if (Math.Floor(magicPower) != maxMagicPower)
        {
            magicPower = Math.Min(magicPower + recoverMagicPower * Time.deltaTime, maxMagicPower);
            text.text = Math.Floor(magicPower).ToString();
            guage.fillAmount = magicPower / maxMagicPower;
        }
        else
        {
        }
    }

    /// <summary>
    /// マジックパワーを回復するメソッド
    /// </summary>
    public void RecoverMagicPower(float amount)
    {
        magicPower = Math.Min(magicPower + amount, maxMagicPower);
        text.text = Math.Floor(magicPower).ToString();
        guage.fillAmount = magicPower / maxMagicPower;
    }

    /// <summary>
    /// マジックパワーを消費するメソッド
    /// </summary>
    /// <param name="usePower">消費するマジックパワーの量</param>
    /// <returns>マジックパワーが十分にあればtrue、そうでなければfalse</returns>
    public bool UseMagicPower(int usePower)
    {
        if (magicPower >= usePower)
        {
            magicPower -= usePower;
            return true;
        }
        else
        {
            return false;
        }
    }
}
