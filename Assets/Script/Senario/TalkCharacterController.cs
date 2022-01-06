using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkCharacterController : MonoBehaviour
{
    // ������L�Ȃ̂�R�Ȃ̂�
    public string LR;
    // ���O�̕ύX�������肷��̂Ɏg��
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

    // Start is called before the first frame update
    void Start()
    {
        // �����ʒu�̋L�^
        rect = GetComponent<RectTransform>();
        orgPosition = rect.position;
        // �o���ʒu�̍쐬
        if (LR == "L")
        {
            apperPosition = new Vector3(orgPosition.x - 40.0f, orgPosition.y, orgPosition.z);
        }
        else 
        {
            apperPosition = new Vector3(orgPosition.x + 40.0f, orgPosition.y, orgPosition.z);
        }
        currentCharacterName = "";
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f; // �ŏ��͓���
        nameText = nameTextObj.GetComponent<Text>();
        talkController = GameObject.Find("TalkController").GetComponent<TalkController>();
        num = talkController.num;
        image = characterImage.GetComponent<Image>();
        rectImage = characterImage.GetComponent<RectTransform>();
    }

    public IEnumerator Active() {

        senarioTalkScript = talkController.senarioTalkScript;
        num = talkController.num;
        // ������������̃L�����N�^�[��������ꍇ�Ƀt�F�[�h�A�E�g����
        if (senarioTalkScript.senarioTalks[num].name != currentCharacterName) { 
            while(canvasGroup.alpha > 0.0f)
            {
                canvasGroup.alpha -= Time.deltaTime * 5.0f;
                yield return new WaitForSeconds(0.01f);
            }
            // �|�W�V�������o���ʒu�ɕύX
            rect.position = apperPosition;
            // �L�����N�^�[���̂��w��̖��O�ɕύX
            currentCharacterName = senarioTalkScript.senarioTalks[num].name;
            // �\�����������ւ���
            nameText.text = talkController.characterBasicInfos[currentCharacterName].GetCharacterName();
        }
        StartCoroutine(toSpeakColor());
        // �C���[�W�������ւ���
        image.sprite = talkController.characterBasicInfos[currentCharacterName].GetSprite(senarioTalkScript.senarioTalks[num].expressions);
       
        // ������Ԃ̏ꍇ�͏o��������
        while(canvasGroup.alpha < 1.0f)
        {
            rect.position = new Vector3(rect.position.x + (orgPosition.x - apperPosition.x) * Time.deltaTime * 3.0f, rect.position.y, rect.position.z);
            canvasGroup.alpha += Time.deltaTime * 3.0f;
            yield return new WaitForSeconds(0.01f);
        }
        // �ꉞ�ŏ��̈ʒu�֖߂�
        rect.position = orgPosition;
    }
    /* �C���������̂Ŏg�p���Ȃ�
    private IEnumerator toSpeakSize() {
        Debug.Log("�g��Ăяo��");
        while (rectImage.localScale.x < GameSettingParams.ActiveScaleParam || rectImage.localScale.y < GameSettingParams.ActiveScaleParam)
        {
            float scale = rectImage.localScale.x + Time.deltaTime * 1.0f;
            rectImage.localScale = new Vector3(scale, scale, rect.localScale.z);
            yield return new WaitForSeconds(0.01f);
        } 
    }
    */
    private IEnumerator toSpeakColor()
    {
        Debug.Log("�����F���̌Ăяo��");
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
        Debug.Log("�񊈐��F���̌Ăяo��");
        Debug.Log(image.color.r);
        while (image.color.r > GameSettingParams.inActiveColorParam || image.color.g > GameSettingParams.inActiveColorParam || image.color.b > GameSettingParams.inActiveColorParam)
        {
            Debug.Log("�񊈐��F���̃��[�v");
            float colorParam = image.color.r - Time.deltaTime * 3.0f;
            image.color = new Color(colorParam, colorParam, colorParam);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
