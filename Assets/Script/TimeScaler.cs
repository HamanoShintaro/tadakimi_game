using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    private float defaultTimeScale = 1.0f;

    /// <summary>
    /// 指定された倍率で時間スケールを設定します。
    /// </summary>
    /// <param name="scale">時間スケールの倍率</param>
    private void SetTimeScale(float scale)
    {
        if (scale < 0)
        {
            Debug.LogWarning("時間スケールは0以上でなければなりません。");
            return;
        }
        Time.timeScale = scale;
        Debug.Log("時間スケールが " + scale + " 倍に設定されました。");
    }

    /// <summary>
    /// 時間スケールを通常の速度にリセットします。
    /// </summary>
    private void ResetTimeScale()
    {
        Time.timeScale = defaultTimeScale;
        Debug.Log("時間スケールが通常の速度にリセットされました。");
    }

    private void OnGUI()
    {
        // 2倍速に設定するボタン
        if (GUI.Button(new Rect(10, 70, 200, 30), "2倍速"))
        {
            SetTimeScale(2.0f);
        }

        // 3倍速に設定するボタン
        if (GUI.Button(new Rect(10, 100, 200, 30), "3倍速"))
        {
            SetTimeScale(3.0f);
        }

        // 時間スケールをリセットするボタン
        if (GUI.Button(new Rect(10, 130, 200, 30), "等倍速"))
        {
            ResetTimeScale();
        }
    }
}