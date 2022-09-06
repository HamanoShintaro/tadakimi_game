using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class summonCharacter : MonoBehaviour
{
    public string characterName;
    public GameObject characterPanel;
    public string prefabPath;
    public GameObject magicPower;
    public GameObject backgroud;

    public int cost;
    private float summonCoolDown;
    private int limit;

    private float summonCoolTime;
    private string status; //skill summon wait

    private GameObject characterPrefab;
    private MagicPowerController magicPowerController;
    private Image backgroudImage;
    private Animator animator;

    private void Start()
    {
        characterPrefab = Resources.Load<GameObject>(prefabPath);
        magicPowerController = magicPower.GetComponent<MagicPowerController>();
        backgroudImage = backgroud.GetComponent<Image>();
        animator = GetComponent<Animator>();

        summonCoolDown = 10.0f;
        limit = 1;

        summonCoolTime = 0.0f;
        status = "wait";
    }

    void Update()
    {
        if (status == "wait")
        {
            if (summonCoolTime == 0.0f)
            {
                if (cost <= magicPowerController.magicPower)
                {
                    status = "summon";
                    animator.SetBool("summon", true);
                    this.GetComponent<EventTrigger>().enabled = true;
                }
            }
            backgroudImage.fillAmount = (summonCoolDown - summonCoolTime) / summonCoolDown;
            summonCoolTime = Mathf.Max(0.0f, summonCoolTime - Time.deltaTime);
        }

        if (status == "summon")
        {
            if (magicPowerController.magicPower < cost || 0.0f < summonCoolTime)
            {
                status = "wait";
                animator.SetBool("summon", false);
                this.GetComponent<EventTrigger>().enabled = false;
            }

        }

    }

    public void OnClick()
    {
        if (magicPowerController.UseMagicPower(cost))
        {
            GameObject characterClone = Instantiate(this.characterPrefab, this.transform);
            characterClone.transform.SetParent(characterPanel.transform, false);
            summonCoolTime = summonCoolDown;
        }
    }
}
