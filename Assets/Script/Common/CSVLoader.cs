using UnityEngine;
using System.IO;
using System;

public class CSVLoader : MonoBehaviour
{
    [Header("読み込むCSV")]
    public TextAsset[] characterDataBaseCsvFile, senarioTalkScriptCsvFile;

    [Header("反映させるCharacterInfoDataBase")]
    public CharacterInfoDataBase characterDataBase;

    [Header("反映させるSenarioTalkScriptDateBase")]
    public SenarioTalkScriptDateBase senarioTalkScriptDateBase;

    private void Start()
    {
        // CSVからキャラクターデータを読み込む
        LoadCharacterInfoDataBaseCsv();
        // CSVからシナリオテキストを読み込む
        LoadSenarioTalkScriptCsv();
#if UNITY_EDITOR
        SaveData();
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    #if UNITY_EDITOR
    // データを保存する関数
    private void SaveData()
    {
        // CharacterInfoDataBaseを保存
        UnityEditor.EditorUtility.SetDirty(characterDataBase);
        // SenarioTalkScriptDateBaseを保存
        UnityEditor.EditorUtility.SetDirty(senarioTalkScriptDateBase);
        // プロジェクトファイルを保存
        UnityEditor.AssetDatabase.SaveAssets();
    }
    #endif

    public void LoadCharacterInfoDataBaseCsv()
    {
        for (int i = 0; i < characterDataBaseCsvFile.Length; i++)
        {
            // CSVの内容を文字列として読み込む
            StringReader reader = new StringReader(characterDataBaseCsvFile[i].text);

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

                try
                {
                    //基本情報を格納
                    row.id = values[1];
                    row.name = values[2];
                    row.alias = values[3];
                    row.detail = values[4];
                    row.type = values[5];
                }
                catch (Exception ex)
                {
                    Debug.LogError("基本情報の取得に失敗しました: " + ex.Message + " 取得したデータ: " + string.Join(",", values));
                }

                try
                {
                    // 各キャラクターステータスの取得
                    for (int k = 0; k < 5; k++)
                    {
                        var status = row.status[k];

                        try
                        {
                            status.attack = int.Parse(values[3 * (k + 2) + 1]);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("攻撃力の取得に失敗しました: " + ex.Message + " 取得したデータ: " + values[3 * (k + 2) + 1]);
                        }

                        try
                        {
                            status.hp = int.Parse(values[3 * (k + 2) + 2]);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("HPの取得に失敗しました: " + ex.Message + " 取得したデータ: " + values[3 * (k + 2) + 2]);
                        }

                        try
                        {
                            status.growth = string.IsNullOrEmpty(values[3 * (k + 2) + 3]) ? 0 : int.Parse(values[3 * (k + 2) + 3]);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("成長値の取得に失敗しました: " + ex.Message + " 取得したデータ: " + values[3 * (k + 2) + 3]);
                        }

                        try
                        {
                            status.cost = string.IsNullOrEmpty(values[22]) ? 0 : int.Parse(values[22]);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("コストの取得に失敗しました: " + ex.Message + " 取得したデータ: " + values[22]);
                        }

                        try
                        {
                            status.atkKB = int.Parse(values[24]);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("攻撃ノックバック値の取得に失敗しました: " + ex.Message + " 取得したデータ: " + values[24]);
                        }

                        try
                        {
                            status.defKB = int.Parse(values[25]);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("防御ノックバック値の取得に失敗しました: " + ex.Message + " 取得したデータ: " + values[25]);
                        }

                        try
                        {
                            status.speed = int.Parse(values[26]);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("速度の取得に失敗しました: " + ex.Message + " 取得したデータ: " + string.Join(",", values[26]));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("キャラクターステータスの取得に失敗しました: " + ex.Message + " 取得したデータ: " + string.Join(",", values));
                }

                try
                {
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
                }
                catch (Exception ex)
                {
                    Debug.LogError("スキル情報の取得に失敗しました: " + ex.Message + " 取得したデータ: " + string.Join(",", values));
                }

                try
                {
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
                }
                catch (Exception ex)
                {
                    Debug.LogError("スペシャル情報の取得に失敗しました: " + ex.Message + " 取得したデータ: " + string.Join(",", values));
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
