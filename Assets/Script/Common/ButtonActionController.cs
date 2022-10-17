using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActionController : MonoBehaviour
{
    /*
    // ボタン要素となるimageを持ったオブジェクトをアタッチする
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;

    private Image img1;
    private Image img2;
    private Image img3;

    private Color orgColor1;
    private Color orgColor2;
    private Color orgColor3;

    private RectTransform rect1;
    private RectTransform rect2;
    private RectTransform rect3;

    private bool active;

    private void Start()
    {
        active = true;

        if (obj1)
        {
            img1 = obj1.GetComponent<Image>();
            orgColor1 = img1.color;
            rect1 = obj1.GetComponent<RectTransform>();
        }
        if (obj2)
        {
            img2 = obj2.GetComponent<Image>();
            orgColor2 = img2.color;
            rect2 = obj2.GetComponent<RectTransform>();
        }
        if (obj3)
        {
            img3 = obj3.GetComponent<Image>();
            orgColor3 = img3.color;
            rect3 = obj3 .GetComponent<RectTransform>();
        }
    }

    public void pointerEnter()
    {
        if (obj1)
        {
            img1.color = new Color(orgColor1.r + 0.1f, orgColor1.g + 0.1f, orgColor1.b + 0.1f, orgColor1.a - 0.15f);
        }
        if (obj2)
        {
            img2.color = new Color(orgColor2.r + 0.1f, orgColor2.g + 0.1f, orgColor2.b + 0.1f, orgColor2.a - 0.15f);
        }
        if (obj3)
        {
            img3.color = new Color(orgColor3.r + 0.1f, orgColor3.g + 0.1f, orgColor3.b + 0.1f, orgColor3.a - 0.15f);
        }
    }

    public void pointerExit()
    {
        if (obj1)
        {
            img1.color = orgColor1;
        }
        if (obj2)
        {
            img2.color = orgColor2;
        }
        if (obj3)
        {
            img3.color = orgColor3;
        }
    }

    public void pointerDown()
    {
        if (obj1)
        {
            img1.color = new Color(orgColor1.r - 0.2f, orgColor1.g - 0.2f, orgColor1.b - 0.2f, orgColor1.a + 0.05f);
        }
        if (obj2)
        {
            img2.color = new Color(orgColor2.r - 0.2f, orgColor2.g - 0.2f, orgColor2.b - 0.2f, orgColor2.a + 0.05f);
        }
        if (obj3)
        {
            img3.color = new Color(orgColor3.r - 0.2f, orgColor3.g - 0.2f, orgColor3.b - 0.2f, orgColor3.a + 0.05f);
        }
    }

    public void pointerUp()
    {
        if (obj1)
        {
            img1.color = orgColor1;
        }
        if (obj2)
        {
            img2.color = orgColor2;
        }
        if (obj3)
        {
            img3.color = orgColor3;
        }
    }

    public void pointerClick()
    {
        StartCoroutine(push());
    }

    private IEnumerator push()
    {
        if (active) {
            active = false;
            Vector3 orgScale = rect1.localScale;
            float v = -2.0f;
            float a = 10.0f;
            while (v < 2.0f)
            {
                rect1.localScale = new Vector3(rect1.localScale.x + v * Time.deltaTime, rect1.localScale.y + v * Time.deltaTime, rect1.localScale.z);
                v = v + a * Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }
            rect1.localScale = orgScale;
            active = true;
        }
    }
    */
}   
