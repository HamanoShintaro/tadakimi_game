using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicPowerController : MonoBehaviour
{

    // ���@�͂Ɋւ���Q�[�W�����̎���
    public float magicPower;
    public int maxMagicPower;
    public float recoverMagicPower;

    public GameObject guageObject;
    public GameObject valueObject;

    private Image guage;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        magicPower = 0.0f;
        guage = guageObject.GetComponent<Image>();
        text = valueObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Floor(magicPower) != maxMagicPower) {
            magicPower = Math.Min(
                magicPower + recoverMagicPower * Time.deltaTime,
                maxMagicPower
            );
            text.text = Math.Floor(magicPower).ToString();
            guage.fillAmount = magicPower / maxMagicPower;
        }
    }

    // ���@�͂�����邱�Ƃ��o���邩�A������ꍇ��true��ԋp����
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
