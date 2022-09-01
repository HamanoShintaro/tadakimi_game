using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearController : MonoBehaviour
{
    private List<float> times = new List<float>();
    private List<GameObject> enemies = new List<GameObject>();
    private float time = 0.0f;
    private int itemNumber = 0;

    private void Start()
    {
        //現在のステージを取得する
        var stageId = PlayerPrefs.GetInt(PlayerPrefabKeys.clearStageId).ToString("000");
        Debug.Log(stageId);
        var battleStageSummonEnemy = Resources.Load<BattleStageSummonEnemy>($"DataBase/Data/BattleStageSummonEnemy/001");
        times = battleStageSummonEnemy.GetTimes();
        enemies = battleStageSummonEnemy.GetEnemies();
    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;
        try
        {
            if (time >= times[itemNumber])
            {
                GameObject characterClone = Instantiate(enemies[itemNumber], this.transform);
                characterClone.transform.SetParent(this.gameObject.transform, false);
                itemNumber++;
            }
        }
        catch
        {

        }
    }
}
