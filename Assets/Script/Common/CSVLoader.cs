using UnityEngine;
using System.IO;
using static SenarioTalkScript;

public class CSVLoader : MonoBehaviour
{
    [Header("読み込むCSV")]
    public TextAsset characterDataBaseCsvFile, senarioTalkScriptCsvFile;

    [Header("反映させるCharacterInfoDataBase")]
    public CharacterInfoDataBase characterDataBase;

    [Header("反映させるSenarioTalkScriptDateBase")]
    public SenarioTalkScriptDateBase senarioTalkScriptDateBase;

    [Header("読み込みを始める行")]
    public int startRow;

    private void Start()
    {
        LoadCharacterInfoDataBaseCsv();
        LoadSenarioTalkScriptCsv();
    }

    public void LoadCharacterInfoDataBaseCsv()
    {
        // CSVファイルを読み込むためのStringReaderを作成
        StringReader reader = new StringReader(characterDataBaseCsvFile.text);
        // startRow行をスキップ
        for (int i = 0; i < startRow; i++)
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

            //ステータス
            for (int k = 0; k < 5; k++)
            {
                var status = row.status[k];
                try
                {
                    status.attack = (int)float.Parse(values[3 * (k + 2)]);
                    status.hp = int.Parse(values[3 * (k + 2) + 1]);
                    status.growth = int.Parse(values[3 * (k + 2) + 2]);
                }
                catch
                {
                    status.attack = 1;
                    status.hp = 1;
                    status.growth = 1;
                }

                status.cost = int.Parse(values[21]);
                //22クールタイム
                status.atkKB = float.Parse(values[23]);
                status.defKB = float.Parse(values[24]);
                status.speed = float.Parse(values[25]);
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

    public void LoadSenarioTalkScriptCsv()
    {
        StringReader reader = new StringReader(senarioTalkScriptCsvFile.text);

        for (int i = 0; i < 3; i++)
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
            SenarioTalkScript row = senarioTalkScriptDateBase.senarioTalkScripts[0];

            //名前は3列目から取得する
            //row.name = values[3];
            try
            {
                var senarioTalkScript = row.senarioTalks[index];
                senarioTalkScript.script_jp = values[4];
                senarioTalkScriptDateBase.senarioTalkScripts[0].senarioTalks[index] = senarioTalkScript;
            }
            catch
            {

            }
            index++;
        }
    }
}
