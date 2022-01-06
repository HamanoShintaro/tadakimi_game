using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkTextController : MonoBehaviour
{
    // ���擾�p
    public TalkController talkController;

    // �e�L�X�g�`�ʗp
    private int num;
    public GameObject textObj;
    private Text text;
    private string script;

    // �����o���̕����ݒ�p
    private string LR;
    public GameObject BubbleObj;
    private RectTransform rectBubble;

    // �^�b�v���̈ړ��Ɋւ��鏈���p
    public float firstPosition;
    public float secondPosition;
    public float alpha;
    private CanvasGroup canvasGroup;
    private const float motionTime = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("�e�L�X�g�o�u�����Ăяo����܂���");
        talkController = GameObject.Find("TalkController").GetComponent<TalkController>();
        num = talkController.num;
        script = talkController.senarioTalkScript.GetSenarioTalks()[num].script;
        
        LR = talkController.senarioTalkScript.GetSenarioTalks()[num].LR;
        rectBubble = BubbleObj.GetComponent<RectTransform>();
        if (LR == "L")
        {
            rectBubble.rotation = Quaternion.Euler(180.0f, 180.0f, 0.0f);
        } else
        {
            rectBubble.rotation = Quaternion.Euler(180.0f, 0.0f, 0.0f);
        }

        canvasGroup = this.GetComponent<CanvasGroup>();
        text = textObj.GetComponent<Text>();
        text.text = "";
        StartCoroutine(Floating());
        StartCoroutine(textApear(script));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator textApear(string script)
    {
        int i = 0;

        while (script.Length > i)//���������ׂĕ\�����Ă��Ȃ��ꍇ���[�v
        {
            text.text += script[i];//�ꕶ���ǉ�
            i++;//���݂̕�����
            yield return new WaitForSeconds(0.04f);//�C�ӂ̎��ԑ҂�
        }
    }
    private IEnumerator Floating()
    {

        // �����ݒ�
        RectTransform rect = this.GetComponent<RectTransform>();
        Vector3 org = rect.position;
        Vector3 apper = new Vector3(org.x, org.y - 20, org.z);

        // ������Ƃ��炵�ē����ɂ��Ă���
        rect.position = apper;
        canvasGroup.alpha = 0.0f;

        int i = 1;

        while (i <= 20)
        {
            rect.position = new Vector3(org.x, rect.position.y + 1, org.z);
            rect.localScale = new Vector3(0.80f + 0.01f * i, 0.80f + 0.01f * i, 1.0f);
            canvasGroup.alpha += 0.05f;
            yield return new WaitForSeconds(motionTime);
            i++;
        }
    }

    // �Ăяo���ɉ����ăe�L�X�g�o�u����ω�������
    public void toSecondTalk() {
        StartCoroutine(slideNext());
    }

    public void toDeleteTalk()
    {
        StartCoroutine(deleteBubble());
    }


    private IEnumerator slideNext()
    {
        // �����ݒ�
        RectTransform rect = this.GetComponent<RectTransform>();
        Vector3 org = rect.position;

        int i = 1;

        while (i <= 20)
        {
            rect.position = new Vector3(org.x, rect.position.y + 6, org.z);
            rect.localScale = new Vector3(1.00f - 0.015f * i,  1.00f - 0.015f * i, 0.7f);
            canvasGroup.alpha -= 0.025f;

            yield return new WaitForSeconds(motionTime);
            i++;
        }
    }
    private IEnumerator deleteBubble()
    {

        // �����ݒ�
        RectTransform rect = this.GetComponent<RectTransform>();
        Vector3 org = rect.position;

        int i = 1;

        while (i <= 10)
        {
            rect.position = new Vector3(org.x, rect.position.y + 1, org.z);
            canvasGroup.alpha -= 0.05f;

            yield return new WaitForSeconds(motionTime);
            i++;
        }
        Destroy(this.gameObject);

    }

}
