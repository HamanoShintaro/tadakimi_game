// CSVファイルの読み込みとScriptableObjectへのデータ代入
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CSVLoader : MonoBehaviour
{
    // Unityエディタ上でCSVファイル（TextAsset）をアサインするための変数
    public TextAsset csvFile;
    // Unityエディタ上でCSVDataのインスタンス（ScriptableObject）をアサインするための変数
    public CharacterInfoDataBase csvData;

    public int startRow;

    // シーン開始時にCSVファイルを読み込む
    private void Start()
    {
        LoadCSVData();
    }

    // CSVファイルを読み込み、ScriptableObjectへデータを代入する関数
    public void LoadCSVData()
    {
        // CSVファイルを読み込むためのStringReaderを作成
        StringReader reader = new StringReader(csvFile.text);
        // ヘッダー行をスキップ
        for (int i = 0; i < startRow; i++)
        {
            reader.ReadLine();
        }

        int index = 0;
        // 読み込む行がある間繰り返す
        while (reader.Peek() > -1)
        {
            if (index >= csvData.characterInfoList.Count) return;

            // CSVファイルから1行読み込む
            string line = reader.ReadLine();
            // カンマで区切って各フィールドに分割
            string[] values = line.Split(',');

            // 分割したフィールドをCSVRowオブジェクトに代入
            CharacterInfo row = csvData.characterInfoList[index];

            //基本情報
            row.id = values[1];
            row.name = values[2];
            row.alias = values[3];
            row.detail = values[4];
            row.type = values[5];

            //ステータス
            for (int k = 0; k < 5; k++)
            {
                var status = row.status[k];
                try
                {
                    status.attack = (int)float.Parse(values[3 * (k + 2)]);
                    status.hp = int.Parse(values[3 * (k + 2) + 1]);
                }
                catch
                {
                    status.attack = 1;
                    status.hp = 1;
                }

                //status.growth = int.Parse(values[3 * (k + 2) + 2]);

                status.cost = int.Parse(values[21]);
                //22クールタイム
                status.atkKB = float.Parse(values[23]);
                status.defKB = float.Parse(values[24]);
                status.speed = float.Parse(values[25]);
            }

            csvData.characterInfoList[index] = row;

            index++;
        }
    }
}
