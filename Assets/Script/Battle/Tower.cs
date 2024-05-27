using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タワーのクラス。タワーの体力管理とダメージ処理を行う。
/// </summary>
public class Tower : MonoBehaviour, IDamage
{
    [SerializeField]
    private float maxHp;

    [SerializeField]
    [HideInInspector]
    private GameObject performancePanel;

    [SerializeField]
    [HideInInspector]
    private BattleController battleController;

    public float hp; // 現在の体力
    public float Hp
    {
        get { return hp; }
        set { hp = Mathf.Clamp(value, 0, maxHp); } // 体力を0からmaxHpの範囲に制限
    }

    /// <summary>
    /// 初期化処理。タワーの画像と体力を設定する。
    /// </summary>
    private void Start()
    {
        var currentStageId = PlayerPrefs.GetString(PlayerPrefabKeys.currentStageId);
        var enemyTowerInfo = Resources.Load<BattleStageSummonEnemy>($"DataBase/Data/BattleStageSummonEnemy/{currentStageId}");

        // タワーの画像を取得
        GetComponent<Image>().sprite = enemyTowerInfo.GetEnemyTower();

        // タワーの最大体力を取得
        maxHp = enemyTowerInfo.GetTowerHp();
        Hp = maxHp;
    }

    /// <summary>
    /// ダメージ処理。タワーの体力を減少させ、体力が0以下になった場合ゲームをストップする。
    /// </summary>
    /// <param name="attackPower">攻撃力</param>
    /// <param name="kb">ノックバック値（未使用）</param>
    public void Damage(float attackPower = 0, float kb = 0)
    {
        Hp -= attackPower;
        if (Hp <= 0)
        {
            // ゲームをストップ
            battleController.GameStop(Battle.Dominator.TypeLeader.EnemyLeader);
        }
    }
}
