using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Battle
{
    public class SummonCharacter : MonoBehaviour
    {
        [HideInInspector]
        public GameObject characterPanel;

        [SerializeField, Header("キャラクターID")]
        private CharacterCore.CharacterId characterId;

        [HideInInspector]
        public GameObject magicPower;

        [HideInInspector]
        public GameObject backgroud;

        private int cost;
        private float summonCoolDown;

        private float summonCoolTime;
        private string status;

        private GameObject characterPrefab;
        private MagicPowerController magicPowerController;
        private Image backgroudImage;
        private Animator animator;

        private void Start()
        {
            //キャラクターをリソースから取得
            characterPrefab = Resources.Load<GameObject>($"Prefabs/Battle/Buddy/{characterId}");
            //コストを取得
            cost = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/{characterId}").status[0].cost;//0をレベルに変更する

            magicPowerController = magicPower.GetComponent<MagicPowerController>();
            backgroudImage = backgroud.GetComponent<Image>();
            animator = GetComponent<Animator>();

            summonCoolDown = 10.0f;

            summonCoolTime = 0.0f;
            status = "wait";
        }

        private void Update()
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
                //キャラクターを生成する
                GameObject characterClone = Instantiate(this.characterPrefab, this.transform);
                characterClone.transform.SetParent(characterPanel.transform, false);
                summonCoolTime = summonCoolDown;
            }
        }
    }
}
