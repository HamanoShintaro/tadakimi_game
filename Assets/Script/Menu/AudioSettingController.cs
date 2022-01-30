using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingController : MonoBehaviour
{
    public GameObject BGM;
    private Slider BGMSlider;
    private float volumeBGM;
    public GameObject SE;
    private Slider SESlider;
    private float volumeSE;
    public GameObject CV;
    private Slider CVSlider;
    private float volumeCV;

    public GameObject Canvas;

    // Start is called before the first frame update
    void Start()
    {
        BGMSlider = BGM.GetComponent<Slider>();
        SESlider = SE.GetComponent<Slider>();
        CVSlider = CV.GetComponent<Slider>();

        BGMSlider.value = PlayerPrefs.GetFloat(PlayerPrefabKeys.volumeBGM);
        SESlider.value = PlayerPrefs.GetFloat(PlayerPrefabKeys.volumeSE);
        CVSlider.value = PlayerPrefs.GetFloat(PlayerPrefabKeys.volumeCV);

        volumeBGM = BGMSlider.value;
        volumeSE = SESlider.value;
        volumeCV = CVSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        if(BGMSlider.value != volumeBGM)
        {
            PlayerPrefs.SetFloat(PlayerPrefabKeys.volumeBGM, BGMSlider.value);
            volumeBGM = BGMSlider.value;
            Canvas.GetComponent<AudioSource>().volume = volumeBGM;
        }
        if (SESlider.value != volumeSE)
        {
            PlayerPrefs.SetFloat(PlayerPrefabKeys.volumeSE, SESlider.value);
            volumeSE = SESlider.value;
        }
        if (CVSlider.value != volumeCV)
        {
            PlayerPrefs.SetFloat(PlayerPrefabKeys.volumeCV, CVSlider.value);
            volumeCV = CVSlider.value;
        }
    }
}
