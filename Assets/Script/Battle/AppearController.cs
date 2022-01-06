using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearController : MonoBehaviour
{
    private BattleStageSummonEnemy battleStageSummonEnemy;
    private string stageId;
    private string dataBasePath = "DataBase/Data/BattleStageSummonEnemy/";

    private List<float> times = new List<float>();
    private List<GameObject> enemies = new List<GameObject>();
    private float time;
    private int itemNumber;
    private int loopInt;
    private bool endFlg;

    // Start is called before the first frame update
    void Start()
    {
        //‚Ç‚±‚©‚©‚ç‚Æ‚è‚½‚¢
        stageId = "001";
        battleStageSummonEnemy = Resources.Load<BattleStageSummonEnemy>(dataBasePath + stageId);
        times = battleStageSummonEnemy.GetTimes();
        enemies = battleStageSummonEnemy.GetEnemies();
        time = 0.0f;
        itemNumber = 0;
        loopInt = 0;
        endFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(!endFlg){
            if (loopInt == times.Count)
            {
                endFlg = true;
            }
            else if (time >= times[loopInt])
            {
                loopInt++;
                GameObject characterClone = Instantiate(enemies[itemNumber], this.transform);
                characterClone.transform.SetParent(this.gameObject.transform, false);
                itemNumber++;
            }
        }
    }
}
