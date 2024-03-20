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

    // スタート時にCSVを読み込む
    void Start()
    {
        LoadCSV();
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
                BattleStageSummonEnemy row = stageDataBase.battleStageSummonEnemies[i];
                //if (row.Times.Count != 0) return;
                // 全行に対して、タイム、エネミー、レベルがあるので、それを格納する
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
        }
    }
}



