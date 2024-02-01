using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

namespace Battle
{
    /// <summary>
    /// キャラクターの召喚を管理するメソッド
    /// </summary>
    public class SummonCharacter : MonoBehaviour
    {
        [SerializeField]
        [Header("敵キャラを生成する位置(appearTransform")]
        private int minY = -10, maxY = 10;

        [Space(10)]
        [SerializeField]
        private Transform appearTransform;

        [SerializeField]
        public GameObject characterPanel;

        [SerializeField]
        private int characterFormationIndex;

        [SerializeField]
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

        [SerializeField]
        private AudioClip summonSound;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            magicPowerController = magicPower.GetComponent<MagicPowerController>();

            //SaveControllerを生成する
            SaveController saveController = new SaveController();
            //characterFormationIndexに一致するcharacterFormationのキャラクターを取得する
            saveController.characterFormation.Load();
            try
            {
                string characterId = saveController.characterFormation.list[characterFormationIndex];
                //キャラクターが空の場合はSummonのUIを非表示にする
                if (characterId == "" || characterId == null)
                {
                    this.gameObject.SetActive(false);
                    return;
                }

                //召喚するキャラクターをリソースから取得
                characterPrefab = Resources.Load<GameObject>($"Prefabs/Battle/Buddy/{characterId}");

                //リストからcharacterIdに一致するデータのレベルを取得
                saveController.characterSave.Load();
                var list = saveController.characterSave.list;
                int level = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].id == characterId)
                    {
                        level = list[i].level;
                    }
                }
                //コストを取得
                cost = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/{characterId}").status[level].cost;
                
                backgroudImage = backgroud.GetComponent<Image>();
                animator = GetComponent<Animator>();
                transform.Find("character").GetComponent<Image>().sprite = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/{characterId}").image.icon;
                summonCoolDown = 10.0f;

                summonCoolTime = 0.0f;
                status = "wait";
            }
            catch
            {
            }
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
                }
            }
        }

        public void OnClick()
        {
            if (magicPowerController.UseMagicPower(cost))
            {
                //キャラクターを生成する
                var characterClone = Instantiate(characterPrefab);
                characterClone.transform.SetParent(characterPanel.transform, false);

                var pos = characterClone.transform.localPosition;;

                //生成位置を決定
                var random = Random.Range(minY, maxY);
                pos.x = appearTransform.localPosition.x;
                pos.y = appearTransform.localPosition.y+ random;
                pos.z = appearTransform.localPosition.z;
                characterClone.transform.localPosition = pos;
                characterClone.transform.SetAsFirstSibling();

                //召喚音を鳴らす
                audioSource.PlayOneShot(summonSound);

                //辞書式を宣言
                var characterDic = new Dictionary<GameObject, float>();
                //辞書式にキャラクターを代入(AppearとPlayerを除くため-2を入れている)
                for (int i = 0; i < characterPanel.transform.childCount - 3; i++)
                {
                    var character = characterPanel.transform.GetChild(i).gameObject;
                    characterDic.Add(character, character.GetComponent<RectTransform>().anchoredPosition.y);
                }
                //辞書式をvalueの大きい順に並べ替え
                var sortedKeys = characterDic.OrderBy(x => x.Value).Select(x => x.Key).ToList();
                //階層を並べ替え
                for (int i = 0; i < sortedKeys.Count; i++)
                {
                    sortedKeys[i].transform.SetAsFirstSibling();
                }
                summonCoolTime = summonCoolDown;
            }
        }
    }
}