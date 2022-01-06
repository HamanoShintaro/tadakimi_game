using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkCharacterController : MonoBehaviour
{
    // 自分がLなのかRなのか
    public string LR;
    // 名前の変更をしたりするのに使う
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
        // 初期位置の記録
        rect = GetComponent<RectTransform>();
        orgPosition = rect.position;
        // 出現位置の作成
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
        canvasGroup.alpha = 0.0f; // 最初は透明
        nameText = nameTextObj.GetComponent<Text>();
        talkController = GameObject.Find("TalkController").GetComponent<TalkController>();
        num = talkController.num;
        image = characterImage.GetComponent<Image>();
        rectImage = characterImage.GetComponent<RectTransform>();
    }

    public IEnumerator Active() {

        senarioTalkScript = talkController.senarioTalkScript;
        num = talkController.num;
        // 自分がいる方のキャラクターが違った場合にフェードアウトする
        if (senarioTalkScript.senarioTalks[num].name != currentCharacterName) { 
            while(canvasGroup.alpha > 0.0f)
            {
                canvasGroup.alpha -= Time.deltaTime * 5.0f;
                yield return new WaitForSeconds(0.01f);
            }
            // ポジションを出現位置に変更
            rect.position = apperPosition;
            // キャラクター名称を指定の名前に変更
            currentCharacterName = senarioTalkScript.senarioTalks[num].name;
            // 表示名を差し替える
            nameText.text = talkController.characterBasicInfos[currentCharacterName].GetCharacterName();
        }
        StartCoroutine(toSpeakColor());
        // イメージを差し替える
        image.sprite = talkController.characterBasicInfos[currentCharacterName].GetSprite(senarioTalkScript.senarioTalks[num].expressions);
       
        // 透明状態の場合は出現させる
        while(canvasGroup.alpha < 1.0f)
        {
            rect.position = new Vector3(rect.position.x + (orgPosition.x - apperPosition.x) * Time.deltaTime * 3.0f, rect.position.y, rect.position.z);
            canvasGroup.alpha += Time.deltaTime * 3.0f;
            yield return new WaitForSeconds(0.01f);
        }
        // 一応最初の位置へ戻す
        rect.position = orgPosition;
    }
    /* 気持ち悪いので使用しない
    private IEnumerator toSpeakSize() {
        Debug.Log("拡大呼び出し");
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
        Debug.Log("活性色化の呼び出し");
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
        Debug.Log("非活性色化の呼び出し");
        Debug.Log(image.color.r);
        while (image.color.r > GameSettingParams.inActiveColorParam || image.color.g > GameSettingParams.inActiveColorParam || image.color.b > GameSettingParams.inActiveColorParam)
        {
            Debug.Log("非活性色化のループ");
            float colorParam = image.color.r - Time.deltaTime * 3.0f;
            image.color = new Color(colorParam, colorParam, colorParam);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
