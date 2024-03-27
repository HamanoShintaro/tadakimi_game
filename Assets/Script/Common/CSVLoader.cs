using UnityEngine;
using System.IO;
using System;

public class CSVLoader : MonoBehaviour
{
    [Header("読み込むCSV")]
    // キャラクターとシナリオのCSVファイルを読み込むための配列
    public TextAsset[] characterDataBaseCsvFile, senarioTalkScriptCsvFile;

    [Header("反映させるCharacterInfoDataBase")]
    // キャラクターの情報を格納するデータベース
    public CharacterInfoDataBase characterDataBase;

    [Header("反映させるSenarioTalkScriptDateBase")]
    // シナリオのテキストを格納するデータベース
    public SenarioTalkScriptDateBase senarioTalkScriptDateBase;

    [Header("CharacterInfoDateBase : 読み込みを始める行")]
    // CSV読み込み開始行
    public int startRow;

    private void Start()
    {
        // CSVからキャラクターデータを読み込む
        LoadCharacterInfoDataBaseCsv();
        // CSVからシナリオテキストを読み込む
        LoadSenarioTalkScriptCsv();
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

                int tempInt;
                // 各キャラクターステータスの取得
                for (int k = 0; k < 5; k++)
                {
                    var status = row.status[k];
                    
                    // データ形式が正しいか確認しつつ変換・格納
                    if (int.TryParse(values[3 * (k + 2) + 1], out tempInt))
                    {
                        status.attack = tempInt;
                    }
                    else
                    {
                        status.attack = 100;
                        Debug.LogError("攻撃力の無効な整数値: " + values[3 * (k + 2)] + 1 + " データ型: " + values[3 * (k + 2) + 1].GetType());
                    }

                    if (int.TryParse(values[3 * (k + 2) + 2], out tempInt))
                    {
                        status.hp = tempInt;
                    }
                    else
                    {
                        status.hp = 100;
                        Debug.LogError("HPの無効な整数値: " + values[3 * (k + 2) + 2] + " データ型: " + values[3 * (k + 2) + 2].GetType());
                    }

                    if (int.TryParse(values[3 * (k + 2) + 3], out tempInt))
                    {
                        status.growth = tempInt;
                    }
                    else
                    {
                        status.growth = 100;
                        Debug.LogError("成長の無効な整数値: " + values[3 * (k + 2) + 3] + " データ型: " + values[3 * (k + 2) + 3].GetType());
                    }

                    if (int.TryParse(values[22], out tempInt))
                    {
                        status.cost = tempInt;
                    }
                    else
                    {
                        status.cost = 100;
                        Debug.LogError("costの無効な整数値: " + values[22] + " データ型: " + values[22].GetType());
                    }

                    // キャラクタースキル情報の取得
                    if (int.TryParse(values[24], out tempInt))
                    {
                        status.atkKB = tempInt;
                    }
                    else
                    {
                        status.atkKB = 100;
                        Debug.LogError("atkKBの無効な整数値: " + values[24] + " データ型: " + values[24].GetType());
                    }

                    if (int.TryParse(values[25], out tempInt))
                    {
                        status.defKB = tempInt;
                    }
                    else
                    {
                        status.defKB = 100;
                        Debug.LogError("defKBの無効な整数値: " + values[25] + " データ型: " + values[25].GetType());
                    }

                    if (int.TryParse(values[26], out tempInt))
                    {
                        status.speed = tempInt;
                    }
                    else
                    {
                        status.speed = 100;
                        Debug.LogError("speedの無効な整数値: " + values[26] + " データ型: " + values[26].GetType());
                    }
                }

                row.skill.name = values[33];
                row.skill.Detail = values[34];
                
                // キャラクタースキル情報の取得
                if (int.TryParse(values[31], out tempInt))
                {
                    row.skill.cost = tempInt;
                }
                else
                {
                    Debug.LogError("スキルコストの無効な整数値: " + values[31] + " データ型: " + values[31].GetType());
                }

                if (int.TryParse(values[32], out tempInt))
                {
                    row.skill.cd = tempInt;
                }
                else
                {
                    Debug.LogError("スキルCDの無効な整数値: " + values[32] + " データ型: " + values[32].GetType());
                }

                row.special.name = values[38];
                row.special.Detail = values[39];

                if (int.TryParse(values[36], out tempInt))
                {
                    row.special.cost = tempInt;
                }
                else
                {
                    Debug.LogError("特別コストの無効な整数値: " + values[36] + " データ型: " + values[36].GetType());
                }

                if (int.TryParse(values[37], out tempInt))
                {
                    row.special.cd = tempInt;
                }
                else
                {
                    Debug.LogError("特別CDの無効な整数値: " + values[37] + " データ型: " + values[37].GetType());
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
