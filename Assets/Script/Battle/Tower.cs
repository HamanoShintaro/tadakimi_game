using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour, IDamage
{
    //maxHp
    [SerializeField]
    private float maxHp;

    [SerializeField]
    private GameObject performancePanel;

    [SerializeField]
    private BattleController battleController;

    //Hpプロパティ
    public float hp = 100;
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
        //maxHp読み込み TODOデータベースにステージ情報を入れる(初期位置/maxHp/タワー画像/背景画像/横幅)
        //maxHp = Resources.Load<PlayerInfo>($"DataBase/Data/PlayerInfo/{playerCharacterId}").MaxHp;
        Hp = maxHp;
        //Damage(100);
    }

    public void Damage(float attackPower = 0, float kb = 0)
    {
        Debug.Log("被ダメージ");
        Hp -= attackPower;
        if (Hp == 0)
        {
            //ゲームをストップ
            battleController.GameStop(Battle.Dominator.TypeLeader.EnemyLeader);
            //リザルト画面(勝利)を表示
            performancePanel.GetComponent<ResultController>().OnResultPanel(true);
        }
    }
}
