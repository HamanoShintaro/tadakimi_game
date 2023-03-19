using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField]
        [Header("敵キャラを生成する位置(appearTransform ± y")]
        private float minY = 0, maxY = 0;

        [SerializeField]
        [Header("AppearTransformをセットする")]
        private Transform appearTransform;

        private void Start()
        {
            //TODO消す
            PlayerPrefs.SetInt(PlayerPrefabKeys.currentStageId, 101);
            //現在のステージを取得する
            var stageId = PlayerPrefs.GetInt(PlayerPrefabKeys.currentStageId).ToString("000");
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
                    GameObject characterClone = Instantiate(enemies[itemNumber], transform);

                    //生成したキャラは後面に出す
                    var pos = characterClone.GetComponent<RectTransform>().anchoredPosition;

                    //生成位置を決定
                    var random = Random.Range(-10, 10);
                    pos.y = 450 + random;
                    characterClone.GetComponent<RectTransform>().anchoredPosition = pos;
                    characterClone.transform.SetAsFirstSibling();

                    //辞書式を宣言
                    var characterDic = new Dictionary<GameObject, float>();
                    //辞書式にキャラクターを代入(AppearとPlayerを除くため-2を入れている)
                    for (int i = 0; i < transform.childCount - 2; i++)
                    {
                        var character = transform.GetChild(i).gameObject;
                        characterDic.Add(character, character.GetComponent<RectTransform>().anchoredPosition.y);
                    }
                    //辞書式をvalueの大きい順に並べ替え
                    var sortedKeys = characterDic.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();
                    //階層を並べ替え
                    for (int i = 0; i < sortedKeys.Count; i++)
                    {
                        sortedKeys[i].transform.SetAsFirstSibling();
                        Debug.Log("調整");
                    }

                    var range = Random.Range(minY, maxY);
                    characterClone.transform.localPosition = new Vector2(appearTransform.localPosition.x, appearTransform.localPosition.y + range);
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
