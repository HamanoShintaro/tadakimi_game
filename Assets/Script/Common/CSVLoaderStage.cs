using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// CSVファイルを読み込んでステージデータベースに格納するクラス
public class CSVLoaderStage : MonoBehaviour
{
    // ステージデータベース
    public StageDataBase stageDataBase;
    // 読み込むCSVファイルのリスト
    public List<TextAsset> csvFiles;


    [SerializeField]
    private bool canLoad = true;

    private void Start()
    {
        if (canLoad)
        {
            InitializeLists();
            LoadCSV();
        }
    }

    // ステージデータベースの3つのリストを初期化するメソッド
    void InitializeLists()
    {
        for (int i = 0; i < stageDataBase.summonData.Count; i++)
        {
            stageDataBase.summonData[i].EnemyData.Times.Clear();
            stageDataBase.summonData[i].EnemyData.Characters.Clear();
            stageDataBase.summonData[i].EnemyData.Levels.Clear();
            stageDataBase.summonData[i].BuddyData.Times.Clear();
            stageDataBase.summonData[i].BuddyData.Characters.Clear();
            stageDataBase.summonData[i].BuddyData.Levels.Clear();
        }
    }

    // CSVを読み込むメソッド
    void LoadCSV()
    {
        // 全てのCSVファイルに対して繰り返す
        for (int i = 0; i < csvFiles.Count; i++)
        {
            // CSVファイルをStringReaderに読み込む
            StringReader reader = new StringReader(csvFiles[i].text);

            // 1行目の読み込みを飛ばす
            reader.ReadLine();

            // CSVファイルの終端まで繰り返す
            while (reader.Peek() > -1)
            {
                // 1行読み込む
                string line = reader.ReadLine();

                string[] values = line.Split(',');

                // ステージデータベースの対応する行を取得
                var enemyRow = stageDataBase.summonData[i];
                for (int j = 0; j < values.Length / 4; j++)
                {
                    if (values[j * 4 + 1] == "敵")
                    {
                        if (float.TryParse(values[j * 4], out float tempFloat))
                        {
                            enemyRow.EnemyData.Times.Add(tempFloat);
                        }
                        else
                        {
                            Debug.LogError("無効な浮動小数点値: " + values[j * 4] + " データ型: " + values[j * 4].GetType());
                        }
                        enemyRow.EnemyData.Characters.Add(Resources.Load<GameObject>("Prefabs/Battle/Enemy/" + values[j * 4 + 2]));
                        if (float.TryParse(values[j * 4 + 3], out tempFloat))
                        {
                            enemyRow.EnemyData.Levels.Add((int)tempFloat);
                        }
                        else
                        {
                            Debug.LogError("無効な浮動小数点値: " + values[j * 4 + 2] + " データ型: " + values[j * 4 + 2].GetType());
                        }
                    }
                }
                var buddyRow = stageDataBase.summonData[i];
                for (int j = 0; j < values.Length / 4; j++)
                {
                    if (values[j * 4 + 1] == "味方")
                    {
                        if (float.TryParse(values[j * 4], out float tempFloat))
                        {
                            buddyRow.BuddyData.Times.Add(tempFloat);
                        }
                        else
                        {
                            Debug.LogError("無効な浮動小数点値: " + values[j * 4] + " データ型: " + values[j * 4].GetType());
                        }
                        buddyRow.BuddyData.Characters.Add(Resources.Load<GameObject>("Prefabs/Battle/Buddy/" + values[j * 4 + 2]));
                        if (float.TryParse(values[j * 4 + 3], out tempFloat))
                        {
                            buddyRow.BuddyData.Levels.Add((int)tempFloat);
                        }
                        else
                        {
                            Debug.LogError("無効な浮動小数点値: " + values[j * 4 + 2] + " データ型: " + values[j * 4 + 2].GetType());
                        }
                    }
                }
            }
        }
    }
}