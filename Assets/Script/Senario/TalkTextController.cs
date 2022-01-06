using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkTextController : MonoBehaviour
{
    // 情報取得用
    public TalkController talkController;

    // テキスト描写用
    private int num;
    public GameObject textObj;
    private Text text;
    private string script;

    // 吹き出しの方向設定用
    private string LR;
    public GameObject BubbleObj;
    private RectTransform rectBubble;

    // タップ時の移動に関する処理用
    public float firstPosition;
    public float secondPosition;
    public float alpha;
    private CanvasGroup canvasGroup;
    private const float motionTime = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("テキストバブルが呼び出されました");
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

        while (script.Length > i)//文字をすべて表示していない場合ループ
        {
            text.text += script[i];//一文字追加
            i++;//現在の文字数
            yield return new WaitForSeconds(0.04f);//任意の時間待つ
        }
    }
    private IEnumerator Floating()
    {

        // 初期設定
        RectTransform rect = this.GetComponent<RectTransform>();
        Vector3 org = rect.position;
        Vector3 apper = new Vector3(org.x, org.y - 20, org.z);

        // ちょっとずらして透明にしておく
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

    // 呼び出しに応じてテキストバブルを変化させる
    public void toSecondTalk() {
        StartCoroutine(slideNext());
    }

    public void toDeleteTalk()
    {
        StartCoroutine(deleteBubble());
    }


    private IEnumerator slideNext()
    {
        // 初期設定
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

        // 初期設定
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
