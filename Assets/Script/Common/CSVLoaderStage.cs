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
    // 敵と仲間のタイプを選ぶ
    private CharacterType characterType;

    // 敵と仲間のタイプを選ぶEnum型
    private enum CharacterType
    {
        Enemy,
        Buddy
    }

    // スタート時にCSVを読み込む
    void Start()
    {
        InitializeLists();
        LoadCSV();
    }

    // ステージデータベースの3つのリストを初期化するメソッド
    void InitializeLists()
    {
        if (characterType == CharacterType.Enemy)
        {
            for (int i = 0; i < stageDataBase.battleStageSummonEnemies.Count; i++)
            {
                stageDataBase.battleStageSummonEnemies[i].Times.Clear();
                stageDataBase.battleStageSummonEnemies[i].Enemies.Clear();
                stageDataBase.battleStageSummonEnemies[i].Levels.Clear();
            }
        }
        else if (characterType == CharacterType.Buddy)
        {
            for (int i = 0; i < stageDataBase.battleStageSummonBuddies.Count; i++)
            {
                stageDataBase.battleStageSummonBuddies[i].Times.Clear();
                stageDataBase.battleStageSummonBuddies[i].Buddies.Clear();
                stageDataBase.battleStageSummonBuddies[i].Levels.Clear();
            }
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
                if (characterType == CharacterType.Enemy)
                {
                    BattleStageSummonEnemy row = stageDataBase.battleStageSummonEnemies[i];
                    for (int j = 0; j < values.Length / 4; j++)
                    {
                        Debug.Log(values[j * 4 + 1]);
                        if (values[j * 4 + 1] == "敵")
                        {
                            if (float.TryParse(values[j * 4], out float tempFloat))
                            {
                                Debug.Log("タイム: " + tempFloat);
                                row.Times.Add(tempFloat);
                            }
                            else
                            {
                                Debug.LogError("無効な浮動小数点値: " + values[j * 4] + " データ型: " + values[j * 4].GetType());
                            }
                            row.Enemies.Add(Resources.Load<GameObject>("Prefabs/Battle/Enemy/" + values[j * 4 + 2]));
                            if (float.TryParse(values[j * 4 + 3], out tempFloat))
                            {
                                Debug.Log("レベル: " + tempFloat);
                                row.Levels.Add((int)tempFloat);
                            }
                            else
                            {
                                Debug.LogError("無効な浮動小数点値: " + values[j * 4 + 2] + " データ型: " + values[j * 4 + 2].GetType());
                            }
                        }
                    }
                }
                else
                {
                    BattleStageSummonBuddy row = stageDataBase.battleStageSummonBuddies[i];
                    for (int j = 0; j < values.Length / 4; j++)
                    {
                        Debug.Log(values[j * 4 + 1]);
                        if (values[j * 4 + 1] == "味方")
                        {
                            if (float.TryParse(values[j * 4], out float tempFloat))
                            {
                                Debug.Log("タイム: " + tempFloat);
                                row.Times.Add(tempFloat);
                            }
                            else
                            {
                                Debug.LogError("無効な浮動小数点値: " + values[j * 4] + " データ型: " + values[j * 4].GetType());
                            }
                            row.Buddies.Add(Resources.Load<GameObject>("Prefabs/Battle/Buddy/" + values[j * 4 + 2]));
                            if (float.TryParse(values[j * 4 + 3], out tempFloat))
                            {
                                Debug.Log("レベル: " + tempFloat);
                                row.Levels.Add((int)tempFloat);
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
}
