using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleLevelController : MonoBehaviour
{
    public GameObject canvas;
    public GameObject magicPower;

    private BattleController battleController;
    private MagicPowerController magicPowerController;
    private Animator animator;

    private Dictionary<int, int> levelUpCost = new Dictionary<int, int>();

    private bool status;
    private bool isMax;

    private Text currentLv;
    private Text cost;

    private void Start()
    {
        battleController = canvas.GetComponent<BattleController>();
        magicPowerController = magicPower.GetComponent<MagicPowerController>();
        currentLv = transform.Find("currentLv").gameObject.GetComponent<Text>();
        cost = transform.Find("cost").gameObject.GetComponent<Text>();
        animator = GetComponent<Animator>();

        levelUpCost[0] = 0;
        levelUpCost[1] = 30;
        levelUpCost[2] = 60;
        levelUpCost[3] = 110;
        levelUpCost[4] = 150;
        levelUpCost[5] = 220;
        levelUpCost[6] = 300;

        currentLv.text = "1";
        cost.text = levelUpCost[1].ToString();

        status = false;
        isMax = false;
    }

    private void Update()
    {
        if (!isMax)
        {
            if (status)
            {
                if (magicPowerController.magicPower < levelUpCost[battleController.magic_recovery_level])
                {
                    status = false;
                    this.GetComponent<EventTrigger>().enabled = false;
                    animator.SetBool("isValid", false);
                }
            }
            else
            {
                if (levelUpCost[battleController.magic_recovery_level] <= magicPowerController.magicPower)
                {
                    status = true;
                    this.GetComponent<EventTrigger>().enabled = true;
                    animator.SetBool("isValid", true);
                }
            }
        }
    }

    /// <summary>
    /// ユーザーがクリックしたときに呼び出されるメソッド。
    /// 魔力を消費してレベルアップを行う。
    /// 魔力が足りない場合は何も行わない。
    /// 最大レベルに達した場合、コスト表示を「-」に変更する。
    /// </summary>
    public void OnClick()
    {
        if (magicPowerController.UseMagicPower(levelUpCost[battleController.magic_recovery_level]))
        {
            if (battleController.magic_recovery_level == 6) isMax = true;
            status = false;
            this.GetComponent<EventTrigger>().enabled = false;
            animator.SetBool("isValid", false);

            battleController.UpMagicLevel();

            currentLv.text = battleController.magic_recovery_level.ToString();
            if (isMax) cost.text = "-";
            else cost.text = levelUpCost[battleController.magic_recovery_level].ToString();
        }
    }
}