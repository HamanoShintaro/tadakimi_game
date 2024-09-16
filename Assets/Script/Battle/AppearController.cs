using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AppearController : MonoBehaviour
{
    private List<float> enemyTimes = new List<float>();
    private List<GameObject> enemies = new List<GameObject>();
    private List<int> enemyLevels = new List<int>();
    private int enemyItemNumber = 0;
    private List<float> buddyTimes = new List<float>();
    private List<GameObject> buddies = new List<GameObject>();
    private List<int> buddyLevels = new List<int>();
    private int buddyItemNumber = 0;
    private float time = 0.0f;
    private bool isGenerating = true;

    [SerializeField]
    [Header("キャラを生成する位置(appearTransform")]
    private int minY = -10, maxY = 10;

    [SerializeField]
    [Header("enemyAppearTransformをセットする")]
    private Transform enemyAppearTransform;

    [SerializeField]
    [Header("buddyAppearTransformをセットする")]
    private Transform buddyAppearTransform;

    private void Start()
    {
        //現在のステージを取得する
        var stageId = PlayerPrefs.GetString(PlayerPrefabKeys.currentStageId);

        var battleStageSummonEnemy = Resources.Load<BattleStageSummonEnemy>($"DataBase/Data/BattleStageSummonEnemy/{stageId}");
        enemyTimes = battleStageSummonEnemy.GetTimes();
        enemies = battleStageSummonEnemy.GetEnemies();
        enemyLevels = battleStageSummonEnemy.GetLevels();

        var battleStageSummonBuddy = Resources.Load<BattleStageSummonBuddy>($"DataBase/Data/BattleStageSummonBuddy/{stageId}");
        buddyTimes = battleStageSummonBuddy.GetTimes();
        buddies = battleStageSummonBuddy.GetBuddies();
        buddyLevels = battleStageSummonBuddy.GetLevels();
    }

    private void FixedUpdate()
    {
        if (isGenerating)
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
            if (time >= enemyTimes[enemyItemNumber] && enemyItemNumber < enemyTimes.Count)
            {
                var characterClone = Instantiate(enemies[enemyItemNumber], transform);
                var pos = characterClone.transform.localPosition;

                //生成位置を決定
                var random = Random.Range(minY, maxY);
                pos.x = enemyAppearTransform.localPosition.x;
                pos.y = enemyAppearTransform.localPosition.y + random;
                pos.z = enemyAppearTransform.localPosition.z;
                characterClone.transform.localPosition = pos;
                characterClone.transform.SetAsFirstSibling();

                //辞書式を宣言
                var characterDic = new Dictionary<GameObject, float>();
                //辞書式にキャラクターを代入(AppearとPlayerを除くため-3を入れている)
                for (int i = 0; i < transform.childCount - 3; i++)
                {
                    var character = transform.GetChild(i).gameObject;
                    var rectTransform = character.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        characterDic.Add(character, rectTransform.anchoredPosition.y);
                    }
                }
                //辞書式をvalueの大きい順に並べ替え
                var sortedKeys = characterDic.OrderBy(x => x.Value).Select(x => x.Key).ToList();
                //階層を並べ替え
                for (int i = 0; i < sortedKeys.Count; i++)
                {
                    sortedKeys[i].transform.SetAsFirstSibling();
                }
                characterClone.GetComponent<CharacterCore>().level = enemyLevels[enemyItemNumber];
                enemyItemNumber++;
                if (enemyItemNumber >= enemyTimes.Count) // 最後の敵キャラが生成されたら
                {
                    isGenerating = false; // 生成フラグをオフにする
                }
            }
        }
        catch
        {
        }

        try
        {
            if (time >= buddyTimes[buddyItemNumber] && buddyItemNumber < buddyTimes.Count)
            {
                var characterClone = Instantiate(buddies[buddyItemNumber], transform);
                var pos = characterClone.transform.localPosition;

                //生成位置を決定
                var random = Random.Range(minY, maxY);
                pos.x = buddyAppearTransform.localPosition.x;
                pos.y = buddyAppearTransform.localPosition.y + random;
                pos.z = buddyAppearTransform.localPosition.z;
                characterClone.transform.localPosition = pos;
                characterClone.transform.SetAsFirstSibling();

                //辞書式を宣言
                var buddyDic = new Dictionary<GameObject, float>();
                //辞書式にキャラクターを代入(AppearとPlayerを除くため-3を入れている)
                for (int i = 0; i < transform.childCount - 3; i++)
                {
                    var buddy = transform.GetChild(i).gameObject;
                    var rectTransform = buddy.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        buddyDic.Add(buddy, rectTransform.anchoredPosition.y);
                    }
                }
                //辞書式をvalueの大きい順に並べ替え
                var sortedKeys = buddyDic.OrderBy(x => x.Value).Select(x => x.Key).ToList();
                //階層を並べ替え
                for (int i = 0; i < sortedKeys.Count; i++)
                {
                    sortedKeys[i].transform.SetAsFirstSibling();
                }
                characterClone.GetComponent<CharacterCore>().level = buddyLevels[buddyItemNumber];
                buddyItemNumber++;
                if (buddyItemNumber >= buddyTimes.Count) // 最後の味方キャラが生成されたら
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