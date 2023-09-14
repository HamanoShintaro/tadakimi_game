using UnityEngine;
using System.IO;
//using static SenarioTalkScript;

public class CSVLoader : MonoBehaviour
{
    [Header("読み込むCSV")]
    public TextAsset[] characterDataBaseCsvFile, senarioTalkScriptCsvFile;

    [Header("反映させるCharacterInfoDataBase")]
    public CharacterInfoDataBase characterDataBase;

    [Header("反映させるSenarioTalkScriptDateBase")]
    public SenarioTalkScriptDateBase senarioTalkScriptDateBase;

    [Header("CharacterInfoDateBase : 読み込みを始める行")]
    public int startRow;

    private void Start()
    {
        LoadCharacterInfoDataBaseCsv();
        //LoadSenarioTalkScriptCsv();
    }

    public void LoadCharacterInfoDataBaseCsv()
    {
        for (int i = 0; i < characterDataBaseCsvFile.Length; i++)
        {
            // CSVファイルを読み込むためのStringReaderを作成
            StringReader reader = new StringReader(characterDataBaseCsvFile[i].text);
            // startRow行をスキップ
            for (int j = 0; j < startRow; j++)
            {
                reader.ReadLine();
            }

            int index = 0;
            // 読み込む行がある間繰り返す
            while (reader.Peek() > -1)
            {
                if (index >= characterDataBase.characterInfoList.Count) return;

                // CSVファイルから1行読み込む
                string line = reader.ReadLine();
                // カンマで区切って各フィールドに分割
                string[] values = line.Split(',');

                // 分割したフィールドをCSVRowオブジェクトに代入
                CharacterInfo row = characterDataBase.characterInfoList[index];

                //基本情報
                row.id = values[1];
                row.name = values[2];
                row.alias = values[3];
                row.detail = values[4];
                row.type = values[5];
                //TODOrow.price = int.Parse(values[]);

                for (int k = 0; k < 5; k++)
                {
                    var status = row.status[k];
                    float tempFloat;
                    int tempInt;

                    if (float.TryParse(values[3 * (k + 2)], out tempFloat))
                    {
                        status.attack = (int)tempFloat;
                    }
                    else
                    {
                        status.attack = 1;
                        Debug.LogError("Invalid float value for attack: " + values[3 * (k + 2)]);
                    }

                    if (int.TryParse(values[3 * (k + 2) + 1], out tempInt))
                    {
                        status.hp = tempInt;
                    }
                    else
                    {
                        status.hp = 1;
                        Debug.LogError("Invalid integer value for hp: " + values[3 * (k + 2) + 1]);
                    }

                    if (int.TryParse(values[3 * (k + 2) + 2], out tempInt))
                    {
                        status.growth = tempInt;
                    }
                    else
                    {
                        status.growth = 1;
                        Debug.LogError("Invalid integer value for growth: " + values[3 * (k + 2) + 2]);
                    }
                }

                //スキル
                row.skill.cost = int.Parse(values[30]);
                row.skill.cd = int.Parse(values[31]);
                row.skill.name = values[32];
                row.skill.Detail = values[33];

                //奥義
                row.special.cost = int.Parse(values[35]);
                row.special.cd = int.Parse(values[36]);
                row.special.name = values[37];
                row.special.Detail = values[38];

                characterDataBase.characterInfoList[index] = row;

                index++;
            }
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
                // CSVファイルから1行読み込む
                string line = reader.ReadLine();
                // カンマで区切って各フィールドに分割
                string[] values = line.Split(',');

                // 分割したフィールドをCSVRowオブジェクトに代入
                SenarioTalkScript row = senarioTalkScriptDateBase.senarioTalkScripts[i];

                //row.name = values[2];
                try
                {
                    var senarioTalkScript = row.senarioTalks[index];
                    senarioTalkScript.name = values[2];
                    senarioTalkScript.script_jp = values[3];
                    senarioTalkScript.script_en = values[4];
                    senarioTalkScript.script_ch = values[5];
                    senarioTalkScript.LR = values[6];
                    senarioTalkScript.expressions = values[7];

                    senarioTalkScriptDateBase.senarioTalkScripts[i].senarioTalks[index] = senarioTalkScript;
                }
                catch
                {
                }
                index++;
            }
        }
    }
}