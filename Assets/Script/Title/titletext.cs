using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class titletext : MonoBehaviour
{
    public float blinkingTime;
    private float time;
    private Image image;

    private Color white = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color transparent = new Color(1.0f, 1.0f, 1.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        time = time + Time.deltaTime;

        if (time > blinkingTime) {

            if (image.color == white)
            {
                image.color = transparent;
            }
            else
            {
                image.color = white;
            }
            time = 0.0f;
        }

    }
}
