using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Net;
using System.Text;
using System.Globalization;
using System;
using System.Text.RegularExpressions;

// CSVファイルを読み込んでステージデータベースに格納するクラス
public class CSVLoaderStage : MonoBehaviour
{
    [Header("ステージデータ設定")]
    // ステージデータベース
    public StageDataBase stageDataBase;
    
    [Header("スプレッドシート設定")]
    [SerializeField]
    private string spreadsheetURL;
    
    [SerializeField]
    private List<string> stageNumbers = new List<string> {
        "101", "102", "103", "104", "105", "106", "107", "108", "109", "110",
        "111", "112", "113", "114", "115", "116", "117", "118", "119", "120",
        "121", "122", "123", "124", "125", "126", "127", "128", "129", "130", "131"
    };

    [Header("フェーズ設定")]
    [SerializeField]
    private float defaultPhaseTimeLimit = float.PositiveInfinity;
    
    [SerializeField]
    private int defaultDefeatedLimit = 1;

#if UNITY_EDITOR
    [CustomEditor(typeof(CSVLoaderStage))]
    public class CSVLoaderStageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            CSVLoaderStage loader = (CSVLoaderStage)target;
            
            if(GUILayout.Button("スプレッドシートからデータを読み込む"))
            {
                loader.InitializeLists();
                loader.LoadFromGoogleSheets();
                EditorUtility.SetDirty(loader.stageDataBase);
                AssetDatabase.SaveAssets();
                Debug.Log("スプレッドシートからデータを読み込みました");
            }
        }
    }
#endif

    // ステージデータベースのリストを初期化するメソッド
    void InitializeLists()
    {
        for (int i = 0; i < stageDataBase.summonData.Count; i++)
        {
            var stageData = stageDataBase.summonData[i];
            
            // 空のリストで初期化（クリア）
            stageData.EnemyBattlePhases = new List<BattlePhaseData>();
            stageData.BuddyBattlePhases = new List<BattlePhaseData>();
        }
        
        Debug.Log($"リスト初期化完了 (ステージ数: {stageDataBase.summonData.Count})");
    }

    // 複数のGoogleスプレッドシートからデータを読み込むメソッド
    void LoadFromGoogleSheets()
    {
        for (int i = 0; i < stageNumbers.Count; i++)
        {
            string stageNumber = stageNumbers[i];
            
            // ステージデータベースのインデックスと対応させる
            if (i >= stageDataBase.summonData.Count)
            {
                Debug.LogError($"ステージ {stageNumber} に対応するBattleStageDataがありません");
                continue;
            }
            
            string csvData = DownloadCSVFromGoogleSheet(spreadsheetURL, stageNumber);
            if (string.IsNullOrEmpty(csvData))
            {
                Debug.LogError($"ステージ {stageNumber} からデータを取得できませんでした");
                continue;
            }
            
            LoadStageData(csvData, i, stageNumber);
        }
        
        // 強制的にDirtyマークを設定
#if UNITY_EDITOR
        for (int i = 0; i < stageDataBase.summonData.Count; i++)
        {
            UnityEditor.EditorUtility.SetDirty(stageDataBase.summonData[i]);
        }
        UnityEditor.EditorUtility.SetDirty(stageDataBase);

        // 強制保存
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    // 特定のステージのデータを読み込むメソッド
    void LoadStageData(string csvData, int stageIndex, string stageNumber)
    {
        StringReader reader = new StringReader(csvData);

        var stageData = stageDataBase.summonData[stageIndex];
        int lineCount = 0;
        
        int currentPhaseNumber = -1;
        BattlePhaseData currentEnemyPhase = null;
        BattlePhaseData currentBuddyPhase = null;
        
        // CSVファイルの終端まで繰り返す
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            lineCount++;
            
            if (string.IsNullOrEmpty(line)) continue;

            // CSVの行を正規表現で分解
            List<string> valuesList = new List<string>();
            Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");
            foreach (Match match in csvSplit.Matches(line))
            {
                string value = match.Value;
                if (value.StartsWith(",")) value = value.Substring(1);
                if (value.StartsWith("\"") && value.EndsWith("\"")) value = value.Substring(1, value.Length - 2);
                value = value.Replace("\"\"", "\"");
                valuesList.Add(value);
            }
            string[] values = valuesList.ToArray();

            if (values.Length == 0) continue;

            // フェーズヘッダーの検出（例: "フェーズ1", "フェーズ2"）
            string firstColumn = values[0].Trim();
            if (firstColumn.StartsWith("フェーズ"))
            {
                // フェーズ番号を抽出
                string phaseNumberStr = firstColumn.Replace("フェーズ", "").Trim();
                if (int.TryParse(phaseNumberStr, out int phaseNum))
                {
                    currentPhaseNumber = phaseNum;

                    // フェーズヘッダー行から制限撃破数を取得（6列目の「制限時間X」）
                    float phaseTimeLimit = defaultPhaseTimeLimit;
                    if (values.Length > 5 && !string.IsNullOrWhiteSpace(values[5]))
                    {
                        string timeLimitStr = values[5].Trim();
                        if (timeLimitStr.StartsWith("制限時間"))
                        {
                            string numberStr = timeLimitStr.Replace("制限時間", "").Trim();
                            if (float.TryParse(numberStr, out float parsedTimeLimit))
                            {
                                phaseTimeLimit = parsedTimeLimit;
                            }
                        }
                    }
                    // フェーズヘッダー行から制限撃破数を取得（7列目の「撃破数X」）
                    int phaseDefeatedLimit = defaultDefeatedLimit;
                    if (values.Length > 6 && !string.IsNullOrWhiteSpace(values[6]))
                    {
                        string defeatedStr = values[6].Trim();
                        if (defeatedStr.StartsWith("撃破数"))
                        {
                            string numberStr = defeatedStr.Replace("撃破数", "").Trim();
                            if (int.TryParse(numberStr, out int headerDefeatedLimit))
                            {
                                phaseDefeatedLimit = headerDefeatedLimit;
                            }
                        }
                    }
                    
                    // 新しいフェーズを作成
                    BattlePhaseData newEnemyPhase = new BattlePhaseData();
                    newEnemyPhase.SummonInfoList = new List<SummonInfo>();
                    newEnemyPhase.PhaseTimeLimit = defaultPhaseTimeLimit;
                    newEnemyPhase.DefeatedCharactersForNextPhase = phaseDefeatedLimit;
                    newEnemyPhase.PhaseTimeLimit = phaseTimeLimit;
                    newEnemyPhase.DefeatedCharactersForNextPhase = phaseDefeatedLimit;

                    BattlePhaseData newBuddyPhase = new BattlePhaseData();
                    newBuddyPhase.SummonInfoList = new List<SummonInfo>();
                    newBuddyPhase.PhaseTimeLimit = defaultPhaseTimeLimit;
                    newBuddyPhase.DefeatedCharactersForNextPhase = phaseDefeatedLimit;
                    newBuddyPhase.PhaseTimeLimit = phaseTimeLimit;
                    newBuddyPhase.DefeatedCharactersForNextPhase = phaseDefeatedLimit;

                    // リストに新しいフェーズを追加
                    stageData.EnemyBattlePhases.Add(newEnemyPhase);
                    stageData.BuddyBattlePhases.Add(newBuddyPhase);
                    
                    // 現在のフェーズ参照を更新
                    currentEnemyPhase = newEnemyPhase;
                    currentBuddyPhase = newBuddyPhase;
                    
                    Debug.Log($"フェーズ{currentPhaseNumber} を作成 (制限撃破数: {phaseDefeatedLimit}体)");
                    continue;
                }
            }

            // キャラクターデータの処理（フェーズが設定されている場合のみ）
            if (currentPhaseNumber > 0 && currentEnemyPhase != null && values.Length >= 5)
            {
                // 列データの取得（1列目は空白、2-5列目にデータ）
                string timeStr = values.Length > 1 ? values[1].Trim() : "";
                string type = values.Length > 2 ? values[2].Trim() : "";
                string characterName = values.Length > 3 ? values[3].Trim() : "";
                string levelStr = values.Length > 4 ? values[4].Trim() : "";

                // キャラクターデータの処理
                if (!string.IsNullOrWhiteSpace(timeStr) && 
                    !string.IsNullOrWhiteSpace(type) && 
                    !string.IsNullOrWhiteSpace(characterName) && 
                    !string.IsNullOrWhiteSpace(levelStr))
                {
                    if (float.TryParse(timeStr, out float time) && 
                        int.TryParse(levelStr, out int level))
                    {
                        if (type == "敵")
                        {
                            var enemyPrefab = Resources.Load<GameObject>($"Prefabs/Battle/Enemy/{characterName}");
                            if (enemyPrefab != null)
                            {
                                var summonInfo = new SummonInfo();
                                summonInfo.Time = time;
                                summonInfo.Character = enemyPrefab;
                                summonInfo.Level = level;
                                currentEnemyPhase.SummonInfoList.Add(summonInfo);
                                Debug.Log($"フェーズ {currentPhaseNumber}: 敵 {characterName} レベル{level} を時間{time}秒で追加");
                            }
                            else
                            {
                                Debug.LogError($"敵プレハブが見つかりません: {characterName}");
                            }
                        }
                        else if (type == "味方")
                        {
                            var buddyPrefab = Resources.Load<GameObject>($"Prefabs/Battle/Buddy/{characterName}");
                            if (buddyPrefab != null)
                            {
                                var summonInfo = new SummonInfo();
                                summonInfo.Time = time;
                                summonInfo.Character = buddyPrefab;
                                summonInfo.Level = level;
                                currentBuddyPhase.SummonInfoList.Add(summonInfo);
                                Debug.Log($"フェーズ {currentPhaseNumber}: 味方 {characterName} レベル{level} を時間{time}秒で追加");
                            }
                            else
                            {
                                Debug.LogError($"味方プレハブが見つかりません: {characterName}");
                            }
                        }
                    }
                }
            }
        }
        
        Debug.Log($"ステージ {stageNumber} 読み込み完了 (フェーズ数: {stageData.EnemyBattlePhases.Count}, 行数: {lineCount})");
    }
    
    // Googleスプレッドシートからデータをダウンロードするメソッド
    private string DownloadCSVFromGoogleSheet(string url, string sheetName)
    {
        try
        {
            // スプレッドシートIDを抽出
            string sheetId = "";
            if (url.Contains("/d/"))
            {
                int startIndex = url.IndexOf("/d/") + 3;
                int endIndex = url.IndexOf("/", startIndex);
                if (endIndex == -1) endIndex = url.Length;
                sheetId = url.Substring(startIndex, endIndex - startIndex);
            }
            else
            {
                sheetId = url; // URLがIDそのものの場合
            }
            
            // CSVエクスポートURL
            string exportUrl = $"https://docs.google.com/spreadsheets/d/{sheetId}/gviz/tq?tqx=out:csv&sheet={sheetName}";
            
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                string csvData = client.DownloadString(exportUrl);
                Debug.Log($"ステージ {sheetName} のデータダウンロード完了");
                return csvData;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"ステージ {sheetName} のスプレッドシートからのダウンロード中にエラーが発生しました: {e.Message}");
            Debug.LogException(e);
            return null;
        }
    }
}