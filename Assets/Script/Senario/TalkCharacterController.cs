using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkCharacterController : MonoBehaviour
{
    public string LR;
    public GameObject nameTextObj;
    public GameObject characterImage;

    private CanvasGroup canvasGroup;
    private Text nameText;
    private Image image;
    private RectTransform rectImage;
    private TalkController talkController;
    private SenarioTalkScript senarioTalkScript;

    private int num = 0;
    private string currentCharacterName;
    private RectTransform rect;
    private Vector3 orgPosition;
    private Vector3 apperPosition;

    private Coroutine activeCoroutine;
    private Coroutine inActiveCoroutine;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        orgPosition = rect.position;
        if (LR == "L") apperPosition = new Vector3(orgPosition.x - 40.0f, orgPosition.y, orgPosition.z);
        else apperPosition = new Vector3(orgPosition.x + 40.0f, orgPosition.y, orgPosition.z);
        currentCharacterName = "";
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
        nameText = nameTextObj.GetComponent<Text>();
        talkController = GameObject.Find("TalkController").GetComponent<TalkController>();
        num = talkController.num;
        image = characterImage.GetComponent<Image>();
        rectImage = characterImage.GetComponent<RectTransform>();
        activeCoroutine = null;
        inActiveCoroutine = null;
    }

    public IEnumerator Active()
    {
        senarioTalkScript = talkController.senarioTalkScript;
        num = talkController.num;
        if (senarioTalkScript.GetSenarioTalks()[num].name != currentCharacterName)
        {
            while (canvasGroup.alpha > 0.0f)
            {
                canvasGroup.alpha -= Time.deltaTime * 30.0f;
                yield return new WaitForSeconds(0.01f);
            }
            rect.position = apperPosition;
            currentCharacterName = senarioTalkScript.GetSenarioTalks()[num].name;
            nameText.text = talkController.characterBasicInfos[currentCharacterName].GetCharacterName();
        }
        StartCoroutine(ToSpeakColor());
        image.sprite = talkController.characterBasicInfos[currentCharacterName].GetSprite(senarioTalkScript.GetSenarioTalks()[num].expressions);

        while (canvasGroup.alpha < 1.0f)
        {
            rect.position = new Vector3(rect.position.x + (orgPosition.x - apperPosition.x) * Time.deltaTime * 15.0f, rect.position.y, rect.position.z);
            canvasGroup.alpha += Time.deltaTime * 15.0f;
            yield return new WaitForSeconds(0.01f);
        }
        rect.position = orgPosition;
    }

    private IEnumerator ToSpeakColor()
    {
        while (image.color.r < GameSettingParams.ActiveColorParam || image.color.g < GameSettingParams.ActiveColorParam || image.color.b < GameSettingParams.ActiveColorParam)
        {
            float colorParam = image.color.r + Time.deltaTime * 15.0f;
            image.color = new Color(colorParam, colorParam, colorParam);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator inActive()
    {
        image = characterImage.GetComponent<Image>();
        while (image.color.r > GameSettingParams.inActiveColorParam || image.color.g > GameSettingParams.inActiveColorParam || image.color.b > GameSettingParams.inActiveColorParam)
        {
            float colorParam = image.color.r - Time.deltaTime * 10.0f;
            image.color = new Color(colorParam, colorParam, colorParam);
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    public void CallActive() {
        if (activeCoroutine != null) {
            StopCoroutine(activeCoroutine);
        }
        activeCoroutine = StartCoroutine(Active());
        return;
    }

    public void CallInActive()
    {
        if (inActiveCoroutine != null)
        {
            StopCoroutine(inActiveCoroutine);
        }
        inActiveCoroutine = StartCoroutine(inActive());
        return;
    }
}
