using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class winningPanelController : MonoBehaviour
{
    public GameObject character;
    public GameObject main;
    public GameObject gold;
    public GameObject bagGold;
    public GameObject getGold;
    public GameObject leftButton;
    public GameObject rightButton;

    private int totalMoney;
    private int getMoney;

    //private float apperTextTime = 0.2f;
    private float sumGoldTime = 1.5f;

    private bool endFlg = false;
    private AudioSource audioSource;
    public AudioClip characterVoice;

    private void OnEnable()
    {
        getMoney = PlayerPrefs.GetInt(PlayerPrefabKeys.playerGetMoney);
        totalMoney = PlayerPrefs.GetInt(PlayerPrefabKeys.playerMoney);

        character.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        main.GetComponent<Text>().text = "";
        gold.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        bagGold.GetComponent<Text>().text = "";
        getGold.GetComponent<Text>().text = "";

        audioSource = GetComponent<AudioSource>();

        StartCoroutine(controllPanel());
    }

    private IEnumerator controllPanel()
    {
        StartCoroutine(Floating(character));
        yield return new WaitForSecondsRealtime(0.5f);
        audioSource.PlayOneShot(characterVoice);

        StartCoroutine(textApear(main));
        yield return new WaitForSecondsRealtime(1.0f);

        StartCoroutine(Floating(gold));
        yield return new WaitForSecondsRealtime(0.5f);

        StartCoroutine(sumGold(bagGold, getGold, totalMoney, getMoney));
    }

    private IEnumerator Floating(GameObject obj)
    {
        RectTransform rect = obj.GetComponent<RectTransform>();
        Vector3 org = rect.position;
        Vector3 apper = new Vector3(org.x - 40, org.y, org.z);
        Image image = obj.GetComponent<Image>();
        Color orgColor = image.color;

        rect.position = apper;
        image.color = new Color(orgColor.r, orgColor.g, orgColor.b, 0);

        int i = 1;

        while (i <= 10)
        {
            rect.position = new Vector3(rect.position.x + 4, org.y, org.z);
            image.color = new Color(orgColor.r, orgColor.g, orgColor.b, image.color.a + 0.1f);
            yield return new WaitForSecondsRealtime(0.05f);
            i++;
        }
    }

    private IEnumerator textApear(GameObject obj)
    {
        string clearText = "Stage Clear";
        Text target = obj.GetComponent<Text>();
        target.text = "";
        int i = 0;

        while (clearText.Length > i)
        {
            target.text += clearText[i];
            i++;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    private IEnumerator sumGold(GameObject bag,GameObject get, int bagGold, int getGold)
    {
        // ?e?L?X?g????????
        Text bagText = bag.GetComponent<Text>();
        bagText.text = "";
        Text getText = get.GetComponent<Text>();
        getText.text = "";

        // ?????\??
        bagText.text = bagGold.ToString();
        yield return new WaitForSecondsRealtime(0.3f);

        getText.text = getGold.ToString();
        yield return new WaitForSecondsRealtime(0.3f);

        // ???Z?\??
        while (getGold > 0)
        {
            getGold--;
            bagGold++;
            bagText.text = bagGold.ToString();
            getText.text = getGold.ToString();
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
}
