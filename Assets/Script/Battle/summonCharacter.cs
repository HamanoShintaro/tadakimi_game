using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class summonCharacter : MonoBehaviour

{
    // Editor���Őݒ肷��l
    public string characterName;
    public GameObject characterPanel;
    public string prefabPath;
    public GameObject magicPower;
    public GameObject backgroud;

    // �L�����N�^�[�ݒ�
    public int cost;
    private float summonCoolDown;
    private int limit;

    // ����p�ϐ�
    private float summonCoolTime;
    private string status; //skill summon wait

    // �����p�����ݒ�
    private GameObject characterPrefab;
    private MagicPowerController magicPowerController;
    private Image backgroudImage;
    private Animator animator;

    // Start is called before the first frame update
    private void Start()
    {
        // �����p�̃I�u�W�F�N�g�ނ̃��[�h
        characterPrefab = Resources.Load<GameObject>(prefabPath);
        magicPowerController = magicPower.GetComponent<MagicPowerController>();
        backgroudImage = backgroud.GetComponent<Image>();
        animator = GetComponent<Animator>();

        // �L�����N�^�[�f�[�^������o�� 
        summonCoolDown = 10.0f;
        limit = 1;

        // �����{�^���̐���
        summonCoolTime = 0.0f;
        status = "wait";

    }

    // Update is called once per frame
    void Update()
    {
        if (status == "wait") {
            if (summonCoolTime == 0.0f) {
                if(cost <= magicPowerController.magicPower) { 
                    // �L�����N�^�[�̐����Ǘ�����v�f���ǉ�������
                    status = "summon";
                    animator.SetBool("summon", true);
                    this.GetComponent<EventTrigger>().enabled = true;
                }
            }
            backgroudImage.fillAmount = (summonCoolDown - summonCoolTime) / summonCoolDown;
            summonCoolTime = Mathf.Max(0.0f, summonCoolTime - Time.deltaTime);
        }

        // �����\��Ԃł��p���[�����Ȃ���΃^�b�`�ł��Ȃ�
        if (status == "summon") {
            if (magicPowerController.magicPower < cost || 0.0f < summonCoolTime)
            {
                // �L�����N�^�[�̐����Ǘ�����v�f���ǉ�������
                status = "wait";
                animator.SetBool("summon", false);
                this.GetComponent<EventTrigger>().enabled = false;
            }

        }

    }

    public void OnClick() {
        if (magicPowerController.UseMagicPower(cost)) {
            Debug.Log("�����ɐ������܂���");

            GameObject characterClone = Instantiate(this.characterPrefab, this.transform);
            characterClone.transform.SetParent(characterPanel.transform, false);
            summonCoolTime = summonCoolDown;
        }
    }
}
