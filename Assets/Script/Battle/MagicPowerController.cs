using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if (Math.Floor(magicPower) != maxMagicPower) {
            magicPower = Math.Min(magicPower + recoverMagicPower * Time.deltaTime, maxMagicPower);
            text.text = Math.Floor(magicPower).ToString();
            guage.fillAmount = magicPower / maxMagicPower;
        }
    }
    //使用可能=>魔力を引いてtrue / 使用不可能=>false
    public bool UseMagicPower(int usePower)
    {
        bool result = false;

        if(magicPower >= usePower){
            magicPower -= usePower;
            result = true;
        }
        return (result);
    }
}
