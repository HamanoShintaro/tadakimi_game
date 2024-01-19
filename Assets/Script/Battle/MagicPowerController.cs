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

    void Start()
    {
        magicPower = 0.0f;
        guage = guageObject.GetComponent<Image>();
        text = valueObject.GetComponent<Text>();
    }
    void Update()
    {
        if (Math.Floor(magicPower) != maxMagicPower)
        {
            magicPower = Math.Min(magicPower + recoverMagicPower * Time.deltaTime, maxMagicPower);
            text.text = Math.Floor(magicPower).ToString();
            guage.fillAmount = magicPower / maxMagicPower;
        }
        else
        {
            Debug.Log("魔力が最大です。");
        }
    }
    //使用可能=>魔力を引いてtrue / 使用不可能=>false
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
