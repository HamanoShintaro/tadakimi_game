using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTower : MonoBehaviour, IDamage
{
    //Hp
    private int maxHp;
    //初期位置
    private int hp = 0;
    public int Hp
    {
        get { return hp; }
        set
        {
            if (hp < 0) hp = 0;
            else if (hp > maxHp) hp = maxHp;
            hp = value;
        }
    }
    private void Start()
    {
        //maxHp読み込み TODOデータベースにステージ情報を入れる(初期位置/maxHp/タワー画像/背景画像/横幅)
        //maxHp = Resources.Load<PlayerInfo>($"DataBase/Data/PlayerInfo/{playerCharacterId}").MaxHp;
        Hp = maxHp;
    }
    public void Damage(int attackPower = 0, int kb = 0)
    {
        //ダメージ計算TODO防御力も計算
        Hp -= attackPower;
        if (Hp == 0)
        {
            //ゲームをストップ
            GameObject.Find("Canvas").GetComponent<BattleController>().GameStop();
            //リザルト画面(勝利)を表示
            GameObject.Find("Canvas/Render/PerformancePanel/winningPanel").SetActive(true);
        }
    }
}
