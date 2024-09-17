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
    }

    public void LoadSenarioTalkScriptCsv()
    {
        for (int i = 0; i < senarioTalkScriptCsvFile.Length; i++)
        {
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
                    return;
                }
                
                var senarioTalkScript = row.senarioTalks[index];
                senarioTalkScript.name = values[1];
                senarioTalkScript.script_jp = values[2];
                senarioTalkScript.script_en = values[3];
                senarioTalkScript.script_ch = values[4];
                senarioTalkScript.LR = values[5];
                senarioTalkScript.expressions = values[6];
                senarioTalkScript.type = values[7];
                senarioTalkScript.script = senarioTalkScript.script_jp;
                senarioTalkScriptDateBase.senarioTalkScripts[i].senarioTalks[index] = senarioTalkScript;
                index++;
            }
        }
    }
}
