using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TODO : タイマーの統合や整理 + 敵の死亡をフラグで管理
namespace Battle
{
    public class AppearController : MonoBehaviour
    {
        // ステージデータ
        private BattleStageData currentStageData;
        
        // 敵キャラクター関連
        private List<float> enemyTimes = new List<float>();
        private List<GameObject> enemies = new List<GameObject>();
        private List<int> enemyLevels = new List<int>();
        private int enemyItemNumber = 0;
        
        // 味方キャラクター関連
        private List<float> buddyTimes = new List<float>();
        private List<GameObject> buddies = new List<GameObject>();
        private List<int> buddyLevels = new List<int>();
        private int buddyItemNumber = 0;
        
        // フェーズ関連
        private List<BattlePhaseData> enemyPhaseData;
        private List<BattlePhaseData> buddyPhaseData;
        private int currentEnemyPhaseIndex = 0;
        private int currentBuddyPhaseIndex = 0;
        private float enemyPhaseTimer = 0f;
        private float buddyPhaseTimer = 0f;
        private int defeatedEnemiesCount = 0;
        private int defeatedBuddiesCount = 0;

        // キャラクター生成設定
        [SerializeField]
        [Header("キャラを生成する位置Y範囲")]
        private int minY = -30, maxY = 30;

        [SerializeField]
        [Header("敵出現位置")]
        private Transform enemyAppearTransform;

        [SerializeField]
        [Header("味方出現位置")]
        private Transform buddyAppearTransform;

        private void Start()
        {
            // 現在のステージを取得
            var stageId = PlayerPrefs.GetString(PlayerPrefabKeys.currentStageId);
            currentStageData = Resources.Load<BattleStageData>($"DataBase/Data/BattleStageData/{stageId}");
            
            // プロパティを使用してアクセス
            enemyPhaseData = currentStageData.EnemyBattlePhases;
            buddyPhaseData = currentStageData.BuddyBattlePhases;

            Debug.Log($"現在のステージID: {stageId}");

            // フェーズベースの出撃データを初期化
            InitializePhaseData();
        }

        private void Update()
        {
            // フェーズの更新処理
            enemyPhaseTimer += Time.deltaTime;
            buddyPhaseTimer += Time.deltaTime;
            CheckPhaseConditions();
            GeneratorCharacter();
        }

        /// <summary>
        /// フェーズデータを初期化するメソッド
        /// </summary>
        private void InitializePhaseData()
        {
            // 敵フェーズの初期化
            if (enemyPhaseData != null && enemyPhaseData.Count > 0)
            {
                currentEnemyPhaseIndex = 0;
                LoadEnemyPhaseData(currentEnemyPhaseIndex);
            }

            // 味方フェーズの初期化
            if (buddyPhaseData != null && buddyPhaseData.Count > 0)
            {
                currentBuddyPhaseIndex = 0;
                LoadBuddyPhaseData(currentBuddyPhaseIndex);
            }

            enemyPhaseTimer = 0f;
            buddyPhaseTimer = 0f;
            defeatedEnemiesCount = 0;
            defeatedBuddiesCount = 0;
        }

        /// <summary>
        /// 敵フェーズデータをロードするメソッド
        /// </summary>
        private void LoadEnemyPhaseData(int phaseIndex)
        {
            if (enemyPhaseData == null || 
                phaseIndex >= enemyPhaseData.Count) return;

            enemyTimes.Clear();
            enemies.Clear();
            enemyLevels.Clear();
            enemyItemNumber = 0;

            var summonInfoList = enemyPhaseData[phaseIndex].SummonInfoList;
            foreach (var summonInfo in summonInfoList)
            {
                enemyTimes.Add(summonInfo.Time);
                enemies.Add(summonInfo.Character);
                enemyLevels.Add(summonInfo.Level);
            }

            Debug.Log($"敵フェーズ {phaseIndex + 1} のデータをロード: {enemyTimes.Count} 体のキャラクター");
        }

        /// <summary>
        /// 味方フェーズデータをロードするメソッド
        /// </summary>
        private void LoadBuddyPhaseData(int phaseIndex)
        {
            if (buddyPhaseData == null || 
                phaseIndex >= buddyPhaseData.Count) return;

            buddyTimes.Clear();
            buddies.Clear();
            buddyLevels.Clear();
            buddyItemNumber = 0;

            var summonInfoList = buddyPhaseData[phaseIndex].SummonInfoList;
            foreach (var summonInfo in summonInfoList)
            {
                buddyTimes.Add(summonInfo.Time);
                buddies.Add(summonInfo.Character);
                buddyLevels.Add(summonInfo.Level);
            }

            Debug.Log($"味方フェーズ {phaseIndex + 1} のデータをロード: {buddyTimes.Count} 体のキャラクター");
        }

        /// <summary>
        /// フェーズの条件をチェックするメソッド
        /// </summary>
        private void CheckPhaseConditions()
        {
            bool shouldAdvanceEnemyPhase = false;
            bool shouldAdvanceBuddyPhase = false;

            // 敵フェーズの条件チェック
            if (enemyPhaseData != null && currentEnemyPhaseIndex < enemyPhaseData.Count)
            {
                // 倒した敵の数による条件
                int requiredDefeatedEnemies = enemyPhaseData[currentEnemyPhaseIndex].DefeatedCharactersForNextPhase;
                if (defeatedEnemiesCount >= requiredDefeatedEnemies)
                {
                    shouldAdvanceEnemyPhase = true;
                    Debug.Log($"敵を{defeatedEnemiesCount}体倒したため、次の敵フェーズへ移行");
                }
                
                // 敵フェーズの時間経過による条件
                if (enemyPhaseTimer >= enemyPhaseData[currentEnemyPhaseIndex].PhaseTimeLimit)
                {
                    shouldAdvanceEnemyPhase = true;
                    Debug.Log("敵フェーズの制限時間経過により次のフェーズへ移行");
                    enemyPhaseTimer = 0f;
                }
            }

            // 味方フェーズの条件チェック
            if (buddyPhaseData != null && currentBuddyPhaseIndex < buddyPhaseData.Count)
            {
                // 味方フェーズの時間経過による条件
                if (buddyPhaseTimer >= buddyPhaseData[currentBuddyPhaseIndex].PhaseTimeLimit)
                {
                    shouldAdvanceBuddyPhase = true;
                    Debug.Log("味方フェーズの制限時間経過により次のフェーズへ移行");
                    buddyPhaseTimer = 0f;
                }
            }

            // フェーズ進行
            if (shouldAdvanceEnemyPhase)
            {
                AdvanceEnemyPhase();
            }

            if (shouldAdvanceBuddyPhase)
            {
                AdvanceBuddyPhase();
            }
        }

        /// <summary>
        /// 敵の次のフェーズに進むメソッド
        /// </summary>
        private void AdvanceEnemyPhase()
        {
            currentEnemyPhaseIndex++;
            defeatedEnemiesCount = 0;
            enemyPhaseTimer = 0f;

            if (currentEnemyPhaseIndex < enemyPhaseData.Count)
            {
                LoadEnemyPhaseData(currentEnemyPhaseIndex);
                Debug.Log($"敵フェーズ {currentEnemyPhaseIndex + 1} を開始");
            }
            else
            {
                Debug.Log("全ての敵フェーズが完了しました");
            }
        }

        /// <summary>
        /// 味方の次のフェーズに進むメソッド
        /// </summary>
        private void AdvanceBuddyPhase()
        {
            currentBuddyPhaseIndex++;
            buddyPhaseTimer = 0f;
            // 必要に応じて味方の撃破数もリセット可能
            defeatedBuddiesCount = 0;

            if (currentBuddyPhaseIndex < buddyPhaseData.Count)
            {
                LoadBuddyPhaseData(currentBuddyPhaseIndex);
                Debug.Log($"味方フェーズ {currentBuddyPhaseIndex + 1} を開始");
            }
            else
            {
                Debug.Log("全ての味方フェーズが完了しました");
            }
        }

        /// <summary>
        /// キャラクター生成メソッド
        /// </summary>
        private void GeneratorCharacter()
        {
            // 敵キャラクター生成
            if (enemyItemNumber < enemyTimes.Count && enemyPhaseTimer >= enemyTimes[enemyItemNumber])
            {
                var characterClone = Instantiate(enemies[enemyItemNumber], transform);
                var pos = characterClone.transform.localPosition;

                // 生成位置を決定
                var random = Random.Range(minY, maxY);
                pos.x = enemyAppearTransform.localPosition.x;
                pos.y = enemyAppearTransform.localPosition.y + random;
                pos.z = enemyAppearTransform.localPosition.z;
                characterClone.transform.localPosition = pos;
                characterClone.transform.SetAsFirstSibling();

                // キャラクターの表示順を整理
                var characterDic = new Dictionary<GameObject, float>();
                for (int i = 0; i < transform.childCount - 3; i++)
                {
                    var character = transform.GetChild(i).gameObject;
                    var rectTransform = character.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        characterDic.Add(character, rectTransform.anchoredPosition.y);
                    }
                }
                
                // Y座標順に並べ替え
                var sortedKeys = characterDic.OrderBy(x => x.Value).Select(x => x.Key).ToList();
                for (int i = 0; i < sortedKeys.Count; i++)
                {
                    sortedKeys[i].transform.SetAsFirstSibling();
                }
                
                // レベル設定
                characterClone.GetComponent<CharacterCore>().level = enemyLevels[enemyItemNumber];
                
                // 死亡時のコールバックを設定
                characterClone.GetComponent<CharacterCore>().OnDeath = OnEnemyDefeated;
                
                enemyItemNumber++;
                
                if (enemyItemNumber >= enemyTimes.Count)
                {
                    Debug.Log("全ての敵キャラが生成されました");
                }
            }

            // 味方キャラクター生成
            if (buddyItemNumber < buddyTimes.Count && buddyPhaseTimer >= buddyTimes[buddyItemNumber])
            {
                var characterClone = Instantiate(buddies[buddyItemNumber], transform);
                var pos = characterClone.transform.localPosition;

                // 生成位置を決定
                var random = Random.Range(minY, maxY);
                pos.x = buddyAppearTransform.localPosition.x;
                pos.y = buddyAppearTransform.localPosition.y + random;
                pos.z = buddyAppearTransform.localPosition.z;
                characterClone.transform.localPosition = pos;
                characterClone.transform.SetAsFirstSibling();

                // キャラクターの表示順を整理
                var buddyDic = new Dictionary<GameObject, float>();
                for (int i = 0; i < transform.childCount - 3; i++)
                {
                    var buddy = transform.GetChild(i).gameObject;
                    var rectTransform = buddy.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        buddyDic.Add(buddy, rectTransform.anchoredPosition.y);
                    }
                }
                
                // Y座標順に並べ替え
                var sortedKeys = buddyDic.OrderBy(x => x.Value).Select(x => x.Key).ToList();
                for (int i = 0; i < sortedKeys.Count; i++)
                {
                    sortedKeys[i].transform.SetAsFirstSibling();
                }
                
                // レベル設定
                characterClone.GetComponent<CharacterCore>().level = buddyLevels[buddyItemNumber];
                
                // 死亡時のコールバックを設定
                characterClone.GetComponent<CharacterCore>().OnDeath = OnBuddyDefeated;
                
                buddyItemNumber++;
                
                if (buddyItemNumber >= buddyTimes.Count)
                {
                    Debug.Log("全ての味方キャラが生成されました");
                }
            }
        }

        /// <summary>
        /// 敵キャラクターが倒された時の処理
        /// </summary>
        private void OnEnemyDefeated(CharacterCore enemy)
        {
            defeatedEnemiesCount++;
            Debug.Log($"敵 {enemy.GetCharacterId()} が倒されました (合計: {defeatedEnemiesCount}体)");
        }

        /// <summary>
        /// 味方キャラクターが倒された時の処理
        /// </summary>
        private void OnBuddyDefeated(CharacterCore buddy)
        {
            defeatedBuddiesCount++;
            Debug.Log($"味方 {buddy.GetCharacterId()} が倒されました (合計: {defeatedBuddiesCount}体)");
        }
    }
}