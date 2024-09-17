using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleText : MonoBehaviour
{
    [Header("ブリンクの時間")]
    [SerializeField]
    private float blinkingTime;
    private Image image;

    private readonly Color transparent = new Color(1.0f, 1.0f, 1.0f, 0.0f);

    void Start()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable() 
    {
        StartCoroutine(BlinkImage());
    }

    private IEnumerator BlinkImage()
    {
        while (true)
        {
            image.color = (image.color == Color.white) ? transparent : Color.white;
            yield return new WaitForSeconds(blinkingTime);
        }
    }

    private void OnDisable() 
    {
        StopAllCoroutines();
    }
}
