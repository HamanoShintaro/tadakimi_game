using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// キャラクターの召喚を管理するメソッド
/// </summary>
public class SummonCharacter : MonoBehaviour
{
    [SerializeField]
    [Header("敵キャラを生成する位置(appearTransform)")]
    private int minY = -30, maxY = 30;

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

    private float summonCoolTime;
    private float maxSummonCoolTime;

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

        // SaveControllerを生成する
        SaveController saveController = new SaveController();
        // characterFormationIndexに一致するcharacterFormationのキャラクターを取得する
        saveController.characterFormation.Load();
        try
        {
            string characterId = saveController.characterFormation.list[characterFormationIndex];
            // キャラクターが空の場合はSummonのUIを非表示にする
            if (string.IsNullOrEmpty(characterId))
            {
                this.gameObject.SetActive(false);
                return;
            }

            // 召喚するキャラクターをリソースから取得
            characterPrefab = Resources.Load<GameObject>($"Prefabs/Battle/Buddy/{characterId}");

            // リストからcharacterIdに一致するデータのレベルを取得
            saveController.characterSave.Load();
            var list = saveController.characterSave.list;
            int level = list.FirstOrDefault(x => x.id == characterId)?.level ?? 0;

            // コストとクールタイムを取得
            var characterInfo = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/{characterId}");
            cost = characterInfo.status[level].cost;
            maxSummonCoolTime = characterInfo.status[level].summoncd;
            summonCoolTime = 0.0f; // 初期値はカウントしない

            backgroudImage = backgroud.GetComponent<Image>();
            animator = GetComponent<Animator>();
            transform.Find("character").GetComponent<Image>().sprite = characterInfo.image.icon;

            status = "wait";
        }
        catch
        {
            Debug.LogError("キャラクター情報のロードに失敗しました。");
        }
    }

    private void Update()
    {
        // 召喚できる状態
        if (status == "wait")
        {
            if (summonCoolTime <= 0.0f && cost <= magicPowerController.magicPower)
            {
                status = "summon";
                animator.SetBool("summon", true);
            }
        }

        // 召喚できない状態
        if (status == "summon")
        {
            if (summonCoolTime > 0.0f || magicPowerController.magicPower < cost)
            {
                status = "wait";
                animator.SetBool("summon", false);
            }
        }

        // クールタイムの進行をUIに反映
        float coolDownProgress = (maxSummonCoolTime - summonCoolTime) / maxSummonCoolTime;
        backgroudImage.fillAmount = Mathf.Clamp01(coolDownProgress);

        if (summonCoolTime > 0.0f)
        {
            summonCoolTime = Mathf.Max(0.0f, summonCoolTime - Time.deltaTime);
        }
    }

    /// <summary>
    /// ボタンがクリックされたときに呼び出されるメソッド。
    /// 魔力を使用してキャラクターを召喚し、召喚音を再生し、キャラクターの順序を再配置します。
    /// </summary>
    public void OnClick()
    {
        if (summonCoolTime > 0.0f) return;
        if (magicPowerController.UseMagicPower(cost))
        {
            SummonCharacterInstance();
            PlaySummonSound();
            ReorderCharacters();
            summonCoolTime = maxSummonCoolTime; // 最初の召喚時からカウント開始
        }
    }

    /// <summary>
    /// キャラクターを召喚するメソッド。
    /// キャラクターを指定された位置にランダムに配置します。
    /// </summary>
    private void SummonCharacterInstance()
    {
        if (appearTransform == null)
        {
            Debug.LogError("appearTransformが設定されていません。インスペクターで設定してください。");
            return;
        }

        var characterClone = Instantiate(characterPrefab);
        characterClone.transform.SetParent(characterPanel.transform, false);

        var pos = characterClone.transform.localPosition;
        var random = Random.Range(minY, maxY);
        pos.x = appearTransform.localPosition.x;
        pos.y = appearTransform.localPosition.y + random;
        pos.z = appearTransform.localPosition.z;
        characterClone.transform.localPosition = pos;
        characterClone.transform.SetAsFirstSibling();
    }

    /// <summary>
    /// 召喚音を再生するメソッド。
    /// </summary>
    private void PlaySummonSound()
    {
        audioSource.PlayOneShot(summonSound);
    }

    /// <summary>
    /// キャラクターの順序を再配置するメソッド。
    /// キャラクターのY座標に基づいて順序を決定します。
    /// </summary>
    private void ReorderCharacters()
    {
        var characterDic = new Dictionary<GameObject, float>();
        for (int i = 0; i < characterPanel.transform.childCount - 3; i++)
        {
            var character = characterPanel.transform.GetChild(i).gameObject;
            if (character.GetComponent<RectTransform>() != null)
            {
                characterDic.Add(character, character.GetComponent<RectTransform>().anchoredPosition.y);
            }
        }
        var sortedKeys = characterDic.OrderBy(x => x.Value).Select(x => x.Key).ToList();
        for (int i = 0; i < sortedKeys.Count; i++)
        {
            sortedKeys[i].transform.SetAsFirstSibling();
        }
    }
}
