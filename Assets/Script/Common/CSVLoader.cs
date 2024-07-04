using UnityEngine;
using System.IO;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class CSVLoader : MonoBehaviour
{
    [Header("読み込むCSV")]
    public TextAsset[] characterDataBaseCsvFile, senarioTalkScriptCsvFile;

    [Header("反映させるCharacterInfoDataBase")]
    public CharacterInfoDataBase characterDataBase;

    [Header("反映させるSenarioTalkScriptDateBase")]
    public SenarioTalkScriptDateBase senarioTalkScriptDateBase;

    [Header("CharacterInfoDateBase : 読み込みを始める行")]
    private int startRow = 0;

    private void Start()
    {
        // CSVからキャラクターデータを読み込む
        LoadCharacterInfoDataBaseCsv();
        // CSVからシナリオテキストを読み込む
        LoadSenarioTalkScriptCsv();
#if UNITY_EDITOR
        // エディタの場合、プロジェクトを更新し直す
        AssetDatabase.Refresh();
#endif
    }

    public void LoadCharacterInfoDataBaseCsv()
    {
        for (int i = 0; i < characterDataBaseCsvFile.Length; i++)
        {
            // CSVの内容を文字列として読み込む
            StringReader reader = new StringReader(characterDataBaseCsvFile[i].text);
            // 設定された開始行まで読み進める
            for (int j = 0; j < startRow; j++)
            {
                reader.ReadLine();
            }

            int index = 0;
            // まだ読み込む行がある場合は続ける
            while (reader.Peek() > -1)
            {
                if (index >= characterDataBase.characterInfoList.Count) return;

                // CSVから1行取得
                string line = reader.ReadLine();
                // カンマ区切りで分割
                string[] values = line.Split(',');

                // 分割されたデータをオブジェクトに格納
                CharacterInfo row = characterDataBase.characterInfoList[index];

                //基本情報を格納
                row.id = values[1];
                row.name = values[2];
                row.alias = values[3];
                row.detail = values[4];
                row.type = values[5];

                // 各キャラクターステータスの取得
                for (int k = 0; k < 5; k++)
                {
                    var status = row.status[k];

                    status.attack = int.Parse(values[3 * (k + 2) + 1]);
                    status.hp = int.Parse(values[3 * (k + 2) + 2]);
                    status.growth = int.Parse(values[3 * (k + 2) + 3]);
                    status.cost = int.Parse(values[22]);

                    // キャラクタースキル情報の取得
                    status.atkKB = int.Parse(values[24]);
                    status.defKB = int.Parse(values[25]);
                    status.speed = int.Parse(values[26]);
                }

                // スキルに関連する情報を取得
                if (!string.IsNullOrEmpty(values[33]))
                {
                    row.skill.name = values[33];
                    row.skill.Detail = values[34];
                    row.skill.cost = int.Parse(values[31]);
                    row.skill.cd = int.Parse(values[32]);
                }
                else
                {
                    row.skill.name = "";
                    row.skill.Detail = "";
                    row.skill.cost = 0;
                    row.skill.cd = 0;
                }

                // スペシャルに関連する情報を取得
                if (!string.IsNullOrEmpty(values[38]))
                {
                    row.special.name = values[38];
                    row.special.Detail = values[39];
                    row.special.cost = int.Parse(values[36]);
                    row.special.cd = int.Parse(values[37]);
                }
                else
                {
                    row.special.name = "";
                    row.special.Detail = "";
                    row.special.cost = 0;
                    row.special.cd = 0;
                }

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
                // シナリオテキストの(index + 3)行目を取得
                string line = reader.ReadLine();

                // シナリオテキストの(index + 3)行目の各列をvaluesに格納
                string[] values = line.Split(',');

                // 取得したデータをオブジェクトに格納
                SenarioTalkScript row = senarioTalkScriptDateBase.senarioTalkScripts[i];

                try
                {
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
                }
                catch (FormatException ex)
                {
                    Debug.LogError("データの形式が正しくありません: " + ex.Message);
                }
                catch (IndexOutOfRangeException ex)
                {
                    Debug.LogError("配列やリストのインデックスが範囲外です: " + ex.Message);
                }
                catch (ArgumentNullException ex)
                {
                    Debug.LogError("不正な null 値が渡されました: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Debug.LogError("予期しないエラーが発生しました: " + ex.Message);
                }
                index++;
            }
        }
    }
}

