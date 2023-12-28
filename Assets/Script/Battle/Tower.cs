using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public float hp;
    public float Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp < 0) hp = 0;
            else if (hp > maxHp) hp = maxHp;
        }
    }

    private void Start()
    {
        var currentStageId = PlayerPrefs.GetString(PlayerPrefabKeys.currentStageId);
        var enemyTowerInfo = Resources.Load<EnemyTowerDateBase>($"DataBase/Data/EnemyTowerInfo/{currentStageId}");

        //タワーの画像を取得
        GetComponent<Image>().sprite = enemyTowerInfo.sprite;

        //タワーの最大体力を取得
        maxHp = enemyTowerInfo.maxHp;
        Hp = maxHp;
    }
    public void Damage(float attackPower = 0, float kb = 0)
    {
        Hp -= attackPower;
        if (Hp <= 0)
        {
            //ゲームをストップ
            battleController.GameStop(Battle.Dominator.TypeLeader.EnemyLeader);
        }
    }
}
