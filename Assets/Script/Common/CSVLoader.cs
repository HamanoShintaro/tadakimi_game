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
                    float tempFloat;
                    
                    // データ形式が正しいか確認しつつ変換・格納
                    if (float.TryParse(values[3 * (k + 2) + 1], out tempFloat))
                    {
                        status.attack = (int)tempFloat;
                    }
                    else
                    {
                        status.attack = 1;
                        Debug.LogError("攻撃力の無効な浮動小数点数値: " + values[3 * (k + 2)] + 1);
                    }

                    if (int.TryParse(values[3 * (k + 2) + 2], out tempInt))
                    {
                        status.hp = tempInt;
                    }
                    else
                    {
                        status.hp = 1;
                        Debug.LogError("HPの無効な整数値: " + values[3 * (k + 2) + 2]);
                    }

                    if (int.TryParse(values[3 * (k + 2) + 3], out tempInt))
                    {
                        status.growth = tempInt;
                    }
                    else
                    {
                        status.growth = 1;
                        Debug.LogError("成長の無効な整数値: " + values[3 * (k + 2) + 3]);
                    }

                    // キャラクタースキル情報の取得
                    if (float.TryParse(values[24], out tempFloat))
                    {
                        status.atkKB = tempFloat;
                    }
                    else
                    {
                        status.atkKB = 1;
                        Debug.LogError("atkKBの無効な浮動小数点数値: " + values[24]);
                    }

                    if (float.TryParse(values[25], out tempFloat))
                    {
                        status.defKB = tempFloat;
                    }
                    else
                    {
                        status.defKB = 1;
                        Debug.LogError("defKBの無効な浮動小数点数値: " + values[25]);
                    }

                    if (float.TryParse(values[26], out tempFloat))
                    {
                        status.speed = tempFloat;
                    }
                    else
                    {
                        status.speed = 1;
                        Debug.LogError("speedの無効な浮動小数点数値: " + values[26]);
                    }
                }
                
                // キャラクタースキル情報の取得
                if (int.TryParse(values[30], out tempInt))
                {
                    row.skill.cost = tempInt;
                }
                else
                {
                    Debug.LogError("スキルコストの無効な整数値: " + values[30]);
                }

                if (int.TryParse(values[31], out tempInt))
                {
                    row.skill.cd = tempInt;
                }
                else
                {
                    Debug.LogError("スキルCDの無効な整数値: " + values[31]);
                }

                if (int.TryParse(values[35], out tempInt))
                {
                    row.special.cost = tempInt;
                }
                else
                {
                    Debug.LogError("特別コストの無効な整数値: " + values[35]);
                }

                if (int.TryParse(values[36], out tempInt))
                {
                    row.special.cd = tempInt;
                }
                else
                {
                    Debug.LogError("特別CDの無効な整数値: " + values[36]);
                }

                // キャラクターの奥義情報の取得
                if (int.TryParse(values[35], out tempInt))
                {
                    row.special.cost = tempInt;
                }
                else
                {
                    Debug.LogError("特別コストの無効な整数値: " + values[35]);
                }

                if (int.TryParse(values[36], out tempInt))
                {
                    row.special.cd = tempInt;
                }
                else
                {
                    Debug.LogError("特別CDの無効な整数値: " + values[36]);
                }

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
