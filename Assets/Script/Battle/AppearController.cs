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
        private bool isGenerating = true; // 生成フラグを追加

        [SerializeField]
        [Header("敵キャラを生成する位置(appearTransform")]
        private int minY = -10, maxY = 10;

        [SerializeField]
        [Header("AppearTransformをセットする")]
        private Transform appearTransform;

        private void Start()
        {
            //現在のステージを取得する
            var stageId = PlayerPrefs.GetString(PlayerPrefabKeys.currentStageId);
            var battleStageSummonEnemy = Resources.Load<BattleStageSummonEnemy>($"DataBase/Data/BattleStageSummonEnemy/{stageId}");
            times = battleStageSummonEnemy.GetTimes();
            enemies = battleStageSummonEnemy.GetEnemies();
            levels = battleStageSummonEnemy.GetLevels();
        }

        private void FixedUpdate()
        {
            if (isGenerating) // 生成フラグをチェック
            {
                GeneratorCharacter();
            }
        }

        /// <summary>
        /// データベースからキャラクターの出撃情報(times:enemies)を取得して生成するメソッド
        /// </summary>
        private void GeneratorCharacter()
        {
            time += Time.deltaTime;
            try
            {
                if (time >= times[itemNumber] && itemNumber < times.Count)
                {
                    var characterClone = Instantiate(enemies[itemNumber], transform);
                    var pos = characterClone.transform.localPosition;

                    //生成位置を決定
                    var random = Random.Range(minY, maxY);
                    pos.x = appearTransform.localPosition.x;
                    pos.y = appearTransform.localPosition.y + random;
                    pos.z = appearTransform.localPosition.z;
                    characterClone.transform.localPosition = pos;
                    characterClone.transform.SetAsFirstSibling();

                    //辞書式を宣言
                    var characterDic = new Dictionary<GameObject, float>();
                    //辞書式にキャラクターを代入(AppearとPlayerを除くため-3を入れている)
                    for (int i = 0; i < transform.childCount - 3; i++)
                    {
                        var character = transform.GetChild(i).gameObject;
                        characterDic.Add(character, character.GetComponent<RectTransform>().anchoredPosition.y);
                    }
                    //辞書式をvalueの大きい順に並べ替え
                    var sortedKeys = characterDic.OrderBy(x => x.Value).Select(x => x.Key).ToList();
                    //階層を並べ替え
                    for (int i = 0; i < sortedKeys.Count; i++)
                    {
                        sortedKeys[i].transform.SetAsFirstSibling();
                    }
                    characterClone.GetComponent<CharacterCore>().level = levels[itemNumber];
                    itemNumber++;
                    if (itemNumber >= times.Count) // 最後の敵キャラが生成されたら
                    {
                        isGenerating = false; // 生成フラグをオフにする
                    }
                }
            }
            catch
            {
            }
        }
    }
}
