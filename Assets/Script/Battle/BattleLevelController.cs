using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleLevelController : MonoBehaviour
{
    // 関連ゲームオブジェクトの設定
    public GameObject canvas;
    public GameObject magicPower;

    // 参照先格納用変数
    private BattleController battleController;
    private MagicPowerController magicPowerController;
    private Animator animator;

    // レベルアップ時のコスト設定
    private Dictionary<int, int> level_up_cost = new Dictionary<int, int>();

    // 制御用フラグ
    private bool status;
    private bool isMax;

    // 表示修正先宣言
    private Text currentLv;
    private Text cost;

    // Start is called before the first frame update
    void Start()
    {
        // 処理のためのgameObject取得
        battleController = canvas.GetComponent<BattleController>();
        magicPowerController = magicPower.GetComponent<MagicPowerController>();
        currentLv = transform.Find("currentLv").gameObject.GetComponent<Text>();
        cost = transform.Find("cost").gameObject.GetComponent<Text>();
        animator = GetComponent<Animator>();

        // レベルアップコストの宣言
        level_up_cost[0] = 0;
        level_up_cost[1] = 30;
        level_up_cost[2] = 60;
        level_up_cost[3] = 110;
        level_up_cost[4] = 150;
        level_up_cost[5] = 220;
        level_up_cost[6] = 300;

        // レベル表示 初期設定
        currentLv.text = "1";
        cost.text = level_up_cost[1].ToString();

        // ステータスに初期値設定
        status = false;
        isMax = false;

    }

    // ステータス判定
    void Update()
    {
        // レベルマックスの場合はステータス更新処理には入らない
        if (!isMax) { 
            // レベルアップ可能状態の場合、召喚などでコストが支払えなくなったら不可状態へ変更
            if (status) {
                if (magicPowerController.magicPower < level_up_cost[battleController.magic_recovery_level]) {
                    status = false;
                    this.GetComponent<EventTrigger>().enabled = false;
                    animator.SetBool("isValid", false);
                }
            } else
            // レベルアップ不可状態の場合、召喚コストを上回ったらクリック可能状態
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

    public void OnClick() {
        if (magicPowerController.UseMagicPower(level_up_cost[battleController.magic_recovery_level])) {
            Debug.Log("レベルアップに成功しました");

            // レベルマックスになる場合はMaxフラグを立てる
            if (battleController.magic_recovery_level == 6)
            {
                isMax = true;
            }

            // ボタンを押せなくする
            status = false;
            this.GetComponent<EventTrigger>().enabled = false;
            animator.SetBool("isValid", false);

            // レベルアップ処理実行
            battleController.UpMagicLevel();

            // 表示の切り替えを行う
            currentLv.text = battleController.magic_recovery_level.ToString();
            if (isMax) {
                cost.text = "-";
            } else {
                cost.text = level_up_cost[battleController.magic_recovery_level].ToString();
            }
        }
    }
}
