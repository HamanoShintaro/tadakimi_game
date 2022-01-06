using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class summonCharacter : MonoBehaviour

{
    // Editor側で設定する値
    public string characterName;
    public GameObject characterPanel;
    public string prefabPath;
    public GameObject magicPower;
    public GameObject backgroud;

    // キャラクター設定
    public int cost;
    private float summonCoolDown;
    private int limit;

    // 制御用変数
    private float summonCoolTime;
    private string status; //skill summon wait

    // 処理用内部設定
    private GameObject characterPrefab;
    private MagicPowerController magicPowerController;
    private Image backgroudImage;
    private Animator animator;

    // Start is called before the first frame update
    private void Start()
    {
        // 召喚用のオブジェクト類のロード
        characterPrefab = Resources.Load<GameObject>(prefabPath);
        magicPowerController = magicPower.GetComponent<MagicPowerController>();
        backgroudImage = backgroud.GetComponent<Image>();
        animator = GetComponent<Animator>();

        // キャラクターデータから取り出す 
        summonCoolDown = 10.0f;
        limit = 1;

        // 召喚ボタンの制御
        summonCoolTime = 0.0f;
        status = "wait";

    }

    // Update is called once per frame
    void Update()
    {
        if (status == "wait") {
            if (summonCoolTime == 0.0f) {
                if(cost <= magicPowerController.magicPower) { 
                    // キャラクターの数を管理する要素も追加したい
                    status = "summon";
                    animator.SetBool("summon", true);
                    this.GetComponent<EventTrigger>().enabled = true;
                }
            }
            backgroudImage.fillAmount = (summonCoolDown - summonCoolTime) / summonCoolDown;
            summonCoolTime = Mathf.Max(0.0f, summonCoolTime - Time.deltaTime);
        }

        // 召喚可能状態でもパワーが少なければタッチできない
        if (status == "summon") {
            if (magicPowerController.magicPower < cost || 0.0f < summonCoolTime)
            {
                // キャラクターの数を管理する要素も追加したい
                status = "wait";
                animator.SetBool("summon", false);
                this.GetComponent<EventTrigger>().enabled = false;
            }

        }

    }

    public void OnClick() {
        if (magicPowerController.UseMagicPower(cost)) {
            Debug.Log("召喚に成功しました");

            GameObject characterClone = Instantiate(this.characterPrefab, this.transform);
            characterClone.transform.SetParent(characterPanel.transform, false);
            summonCoolTime = summonCoolDown;
        }
    }
}
