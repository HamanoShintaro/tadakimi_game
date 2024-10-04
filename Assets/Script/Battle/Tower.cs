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

    [SerializeField]
    private AudioClip damagedSound;

    private AudioSource audioSource;

    public float hp;
    public float Hp
    {
        get { return hp; }
        set { hp = Mathf.Clamp(value, 0, maxHp); }
    }

    /// <summary>
    /// 初期化処理。タワーの画像と体力を設定する。
    /// </summary>
    private void Start()
    {
        var currentStageId = PlayerPrefs.GetString(PlayerPrefabKeys.currentStageId);
        var enemyTowerInfo = Resources.Load<BattleStageSummonEnemy>($"DataBase/Data/BattleStageSummonEnemy/{currentStageId}");

        GetComponent<Image>().sprite = enemyTowerInfo.GetEnemyTower();

        maxHp = enemyTowerInfo.GetTowerHp();
        Hp = maxHp;
        
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// ダメージ処理。タワーの体力を減少させ、体力が0以下になった場合ゲームをストップする。
    /// </summary>
    /// <param name="attackPower">攻撃力</param>
    /// <param name="kb">ノックバック値（未使用）</param>
    public void Damage(float attackPower = 0, float kb = 0)
    {
        audioSource.PlayOneShot(damagedSound);
        Hp -= attackPower;
        if (Hp <= 0)
        {
            battleController.GameStop(TypeLeader.EnemyLeader);
        }
    }
}
