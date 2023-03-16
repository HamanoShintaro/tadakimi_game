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

    private Dictionary<int, int> level_up_cost = new Dictionary<int, int>();

    private bool status;
    private bool isMax;

    private Text currentLv;
    private Text cost;

    void Start()
    {
        battleController = canvas.GetComponent<BattleController>();
        magicPowerController = magicPower.GetComponent<MagicPowerController>();
        currentLv = transform.Find("currentLv").gameObject.GetComponent<Text>();
        cost = transform.Find("cost").gameObject.GetComponent<Text>();
        animator = GetComponent<Animator>();

        level_up_cost[0] = 0;
        level_up_cost[1] = 30;
        level_up_cost[2] = 60;
        level_up_cost[3] = 110;
        level_up_cost[4] = 150;
        level_up_cost[5] = 220;
        level_up_cost[6] = 300;

        currentLv.text = "1";
        cost.text = level_up_cost[1].ToString();

        status = false;
        isMax = false;
    }

    void Update()
    {
        if (!isMax)
        {
            if (status)
            {
                if (magicPowerController.magicPower < level_up_cost[battleController.magic_recovery_level])
                {
                    status = false;
                    this.GetComponent<EventTrigger>().enabled = false;
                    animator.SetBool("isValid", false);
                }
            }
            else
            {
                if (level_up_cost[battleController.magic_recovery_level] <= magicPowerController.magicPower)
                {
                    status = true;
                    this.GetComponent<EventTrigger>().enabled = true;
                    animator.SetBool("isValid", true);
                }
            }
        }
    }

    public void OnClick()
    {
        if (magicPowerController.UseMagicPower(level_up_cost[battleController.magic_recovery_level]))
        {
            if (battleController.magic_recovery_level == 6) isMax = true;
            status = false;
            this.GetComponent<EventTrigger>().enabled = false;
            animator.SetBool("isValid", false);

            battleController.UpMagicLevel();

            currentLv.text = battleController.magic_recovery_level.ToString();
            if (isMax) cost.text = "-";
            else cost.text = level_up_cost[battleController.magic_recovery_level].ToString();
        }
    }
}