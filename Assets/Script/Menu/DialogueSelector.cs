using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSelector : MonoBehaviour
{
    [Serializable]
    public class Dialogue
    {
        public string text;      // セリフの内容
        public int weight;       // 比重
        public int startHour;    // 開始時間
        public int endHour;      // 終了時間
    }

    public List<Dialogue> dialogues = new List<Dialogue>(); // セリフリスト

    private void Awake()
    {
        dialogues = new List<Dialogue>
        {
            new Dialogue { text = "むにゃむにゃ...ふぁぁ...ん、おはよー？もう朝…？むにゃむにゃ...", weight = 15, startHour = 5, endHour = 12 },
            new Dialogue { text = "戻ってきた！今日も一緒に冒険しようね！", weight = 14, startHour = 12, endHour = 18 },
            new Dialogue { text = "あっ！おかえりなさい。もう夜だけどこれから冒険行くの？", weight = 13, startHour = 18, endHour = 0 },
            new Dialogue { text = "…くー…すー…むにゃむにゃ…", weight = 12, startHour = 0, endHour = 5 },
            new Dialogue { text = "よしっ、今日こそは次の街を目指そうね！", weight = 11, startHour = 5, endHour = 12 },
            new Dialogue { text = "ご飯は食べた？よーし、今日も頑張ろうね！", weight = 10, startHour = 12, endHour = 18 },
            new Dialogue { text = "もう夜だね。もうちょっと冒険するの？", weight = 9, startHour = 18, endHour = 0 },
            new Dialogue { text = "むにゃむにゃ…眠いよー…まだやるの？がんば…ろーね…", weight = 8, startHour = 0, endHour = 5 },
            new Dialogue { text = "私のお話は読んでくれてるかな？楽しいって思ってくれたら嬉しいな。", weight = 7, startHour = 0, endHour = 12 },
            new Dialogue { text = "お話を読んでくれてありがとう！良い評価をくれたら嬉しいな。", weight = 6, startHour = 12, endHour = 0 },
            new Dialogue { text = "広告はオプションから消せるから試してみてね。", weight = 5, startHour = 12, endHour = 0 },
            new Dialogue { text = "あれ？ヴェルはどこに行ったんだろう？知ってる？？", weight = 4, startHour = 6, endHour = 12 },
            new Dialogue { text = "むぅ〜…オレンドにまたおちびって言われた…", weight = 3, startHour = 12, endHour = 18 },
            new Dialogue { text = "ねぇねぇ、お仕事ってどんなことしてるの？？", weight = 2, startHour = 18, endHour = 0 },
            new Dialogue { text = "んー…ん？眠れないの？んー…一緒に寝る？", weight = 1, startHour = 2, endHour = 5 },
            new Dialogue { text = "んちゅ…ん…はぁ…んちゅ…ん…もう…。", weight = 0, startHour = 2, endHour = 5 }
        };
    }

    public string GetRandomDialogue()
    {
        // 現在の時間帯を取得
        int currentHour = DateTime.Now.Hour;

        // 現在の時間帯に合うセリフをフィルタ
        List<Dialogue> filteredDialogues = dialogues.FindAll(dialogue => IsWithinTimeRange(dialogue.startHour, dialogue.endHour, currentHour));

        // ランダム値を生成
        float randomValue = UnityEngine.Random.Range(0, 20);
        
        Dialogue closestDialogue = null;
        float closestDifference = float.MaxValue;

        foreach (var dialogue in filteredDialogues)
        {
            float difference = Math.Abs(dialogue.weight - randomValue);
            if (difference < closestDifference)
            {
                closestDifference = difference;
                closestDialogue = dialogue;
            }
        }

        if (closestDialogue != null)
        {
            return closestDialogue.text;
        }

        return "No dialogue available.";
    }

    private bool IsWithinTimeRange(int startHour, int endHour, int currentHour)
    {
        // 開始時間と終了時間をまたぐ場合（例：18:00～5:00）
        if (startHour > endHour)
        {
            return currentHour >= startHour || currentHour < endHour;
        }

        // 通常の時間帯
        return currentHour >= startHour && currentHour < endHour;
    }
}