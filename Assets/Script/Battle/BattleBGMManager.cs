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
    private List<AudioClip> audioClips;

    [SerializeField]
    private AudioSource audioSource;

    private void Start()
    {
        // BattleManagerから現在のステージ番号を取得する想定
        int currentStage = int.Parse(PlayerPrefs.GetString(PlayerPrefabKeys.currentStageId));

        // ステージ番号の下二桁をインデックスとして利用
        // 例) 101なら 101 % 100 = 1, 102なら 2
        int listIndex = currentStage % 100;

        // リストの範囲内かチェック
        if (listIndex >= 0 && listIndex < audioClips.Count)
        {
            // AudioClipが設定されているなら再生
            if (audioClips[listIndex -1] != null)
            {

                audioSource.clip = audioClips[listIndex - 1];
                audioSource.Play();
                Debug.Log($"{currentStage}番目の{audioClips[listIndex - 1].name}再生");
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