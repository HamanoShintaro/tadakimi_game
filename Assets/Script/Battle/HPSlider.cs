using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour
{
    [SerializeField] 
    private Slider slider;

    [SerializeField]
    private CharacterCore characterCore;

    private void Start()
    {
        slider.value = characterCore.Hp;
    }

    private void Update()
    {
        slider.value = characterCore.Hp;
    }
}