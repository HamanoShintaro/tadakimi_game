using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class AppearController : MonoBehaviour
    {
        private List<float> times = new List<float>();
        private List<GameObject> enemies = new List<GameObject>();
        private List<int> levels = new List<int>();
        private float time = 0.0f;
        private int itemNumber = 0;

        private void Start()
        {
            PlayerPrefs.SetInt(PlayerPrefabKeys.currentStageId, 101);
            //現在のステージを取得する
            var stageId = PlayerPrefs.GetInt(PlayerPrefabKeys.currentStageId).ToString("000");
            //TODOstageIdに変更する
            var battleStageSummonEnemy = Resources.Load<BattleStageSummonEnemy>($"DataBase/Data/BattleStageSummonEnemy/{stageId}");
            times = battleStageSummonEnemy.GetTimes();
            enemies = battleStageSummonEnemy.GetEnemies();
            levels = battleStageSummonEnemy.GetLevels();
        }

        private void FixedUpdate()
        {
            GeneratorCharacter();
        }

        /// <summary>
        /// データベースからキャラクターの出撃情報(times:enemies)を取得して生成するメソッド
        /// </summary>
        private void GeneratorCharacter()
        {
            time += Time.deltaTime;
            try
            {
                if (time >= times[itemNumber])
                {
                    GameObject characterClone = Instantiate(enemies[itemNumber], this.transform);
                    characterClone.transform.SetParent(this.gameObject.transform, false);
                    characterClone.GetComponent<CharacterCore>().level = levels[itemNumber];
                    itemNumber++;
                }
            }
            catch
            {

            }
        }
    }
}
