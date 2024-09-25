using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CSVLoaderSenario : MonoBehaviour
{
    [Header("読み込むCSV")]
    public TextAsset[] senarioTalkScriptCsvFile;

    [Header("反映させるSenarioTalkScriptDateBase")]
    public SenarioTalkScriptDateBase senarioTalkScriptDateBase;

    [SerializeField]
    private bool canLoad = true;

    private void Start()
    {
        if (canLoad)
        {
            // CSVからシナリオテキストを読み込む
            LoadSenarioTalkScriptCsv();
        }
        Debug.Log("CSVファイルの数: " + senarioTalkScriptCsvFile.Length);
    }

    public void LoadSenarioTalkScriptCsv()
    {
        for (int i = 0; i < senarioTalkScriptCsvFile.Length; i++)
        {
            Debug.Log($"CSVファイル {i} を読み込み開始");
            StringReader reader = new StringReader(senarioTalkScriptCsvFile[i].text);

            for (int j = 0; j < 3; j++)
            {
                reader.ReadLine();
            }

            int index = 0;
            while (reader.Peek() > -1)
            {
                // シナリオテキストの(index + 3)行目を取得
                string line = reader.ReadLine();

                // シナリオテキストの(index + 3)行目の各列をvaluesに格納
                string[] values = line.Split(',');

                // 取得したデータをオブジェクトに格納
                SenarioTalkScript row = senarioTalkScriptDateBase.senarioTalkScripts[i];

                if (index < 0 || index >= row.senarioTalks.Count || row.senarioTalks[index] == null)
                {
                    Debug.LogWarning($"無効なインデックス: {index}");
                    continue;
                }
                
                var senarioTalkScript = row.senarioTalks[index];
                if (values.Length > 1 && values[1] != null) senarioTalkScript.name = values[1];
                if (values.Length > 2 && values[2] != null) senarioTalkScript.script_jp = values[2];
                if (values.Length > 3 && values[3] != null) senarioTalkScript.script_en = values[3];
                if (values.Length > 4 && values[4] != null) senarioTalkScript.script_ch = values[4];
                if (values.Length > 5 && values[5] != null) senarioTalkScript.LR = values[5];
                if (values.Length > 6 && values[6] != null) senarioTalkScript.expressions = values[6];
                if (values.Length > 7 && values[7] != null) senarioTalkScript.type = values[7];
                senarioTalkScript.script = senarioTalkScript.script_jp;
                senarioTalkScriptDateBase.senarioTalkScripts[i].senarioTalks[index] = senarioTalkScript;
                index++;
            }
        }
        Debug.Log("全てのCSVファイルの読み込みが完了しました");
    }
}
