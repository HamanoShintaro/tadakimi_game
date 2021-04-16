using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titletext : MonoBehaviour
{
    public float blinkingTime;
    private float time;
    private SpriteRenderer spriteRender;

    private Color white = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color transparent = new Color(1.0f, 1.0f, 1.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        spriteRender = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        time = time + Time.deltaTime;

        if (time > blinkingTime) {

            if (spriteRender.color == white)
            {
                spriteRender.color = transparent;
            }
            else
            {
                spriteRender.color = white;
            }
            time = 0.0f;
        }

    }
}
