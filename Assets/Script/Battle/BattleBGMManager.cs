using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// バトルシーンのBGMを管理するクラス。
/// ステージ番号の下二桁をリストのインデックスとして使用し、
/// そのままAudioSourceを再生する実装例です。
/// </summary>
public class BattleBGMManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private StageDataBase stageDataBase;

    private void Start()
    {
        // BattleManagerから現在のステージ番号を取得する想定
        int currentStage = int.Parse(PlayerPrefs.GetString(PlayerPrefabKeys.currentStageId));

        // ステージ番号の下二桁をインデックスとして利用
        int listIndex = currentStage % 100 - 1;

        // リストの範囲内かチェック
        if (listIndex >= 0 && listIndex < stageDataBase.summonData.Count)
        {
            // AudioClipが設定されているなら再生
            if (stageDataBase.summonData[listIndex].GetStageBGM() != null)
            {
                audioSource.clip = stageDataBase.summonData[listIndex].GetStageBGM();
                audioSource.Play();
                Debug.Log($"{currentStage}番目の{stageDataBase.summonData[listIndex].GetStageBGM().name}再生");
            }
            else
            {
                Debug.LogWarning($"AudioClipが設定されていません。ステージ番号: {currentStage}, リスト番号: {listIndex}");
            }
        }
        else
        {
            Debug.LogWarning($"AudioClipリストの範囲外です。ステージ番号: {currentStage}, リスト番号: {listIndex}");
        }
    }
}