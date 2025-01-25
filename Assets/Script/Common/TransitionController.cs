using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
    [SerializeField]
    private float transitionSpeed = 0.5f; // フェードの速度（1.0で1秒かけてフェードイン/アウトする）

    private CanvasGroup canvasGroup; // フェード効果を制御するためのCanvasGroup

    private void Awake()
    {
        // CanvasGroupコンポーネントを取得
        canvasGroup = GetComponent<CanvasGroup>();
        // 初期の透明度を0に設定（完全に透明）
        canvasGroup.alpha = 0.0f;
    }

    private void Start()
    {
        // シーン開始時のフェードインを実行
        StartCoroutine(StartTransition(canvasGroup));
    }

    /// <summary>
    /// シーン開始時のフェードイン処理
    /// </summary>
    /// <param name="canvasGroup">フェード対象のCanvasGroup</param>
    public IEnumerator StartTransition(CanvasGroup canvasGroup)
    {
        canvasGroup.interactable = false; // フェード中はUIの操作を無効化
        while (canvasGroup.alpha < 1.0f) // アルファ値が1.0になるまで繰り返し処理
        {
            // Time.deltaTimeを使用してフレームごとの変化を一定に
            canvasGroup.alpha += transitionSpeed * Time.deltaTime;
            yield return null; // 次のフレームまで待機
        }
        canvasGroup.interactable = true; // フェード完了後にUI操作を有効化
    }

    /// <summary>
    /// シーン切り替え時のフェードアウトとシーン変更処理
    /// </summary>
    /// <param name="canvasGroup">フェード対象のCanvasGroup</param>
    /// <param name="sceneName">遷移先のシーン名</param>
    /// <param name="delayTime">遅延時間（秒）</param>
    public IEnumerator ChangeScene(CanvasGroup canvasGroup, string sceneName, float delayTime = 0.0f)
    {
        // 遅延時間が指定されている場合は待機
        if (delayTime > 0.0f)
        {
            yield return new WaitForSeconds(delayTime);
        }
        while (canvasGroup.alpha > 0) // アルファ値が0になるまで繰り返し処理
        {
            // Time.deltaTimeを使用してフレームごとの変化を一定に
            canvasGroup.alpha -= transitionSpeed * Time.deltaTime;
            yield return null; // 次のフレームまで待機
        }
        // フェードアウト完了後にシーンを変更
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// メニューシーンに切り替える
    /// </summary>
    public void OnChangeMenu()
    {
        StartCoroutine(ChangeScene(canvasGroup, "Menu"));
    }

    /// <summary>
    /// シナリオシーンに切り替える
    /// </summary>
    public void OnChangeSenario()
    {
        StartCoroutine(ChangeScene(canvasGroup, "Senario"));
    }

    /// <summary>
    /// バトルシーンに切り替える
    /// </summary>
    public void OnChangeBattle()
    {
        StartCoroutine(ChangeScene(canvasGroup, "Battle"));
    }

    /// <summary>
    /// 指定されたURLをブラウザで開く
    /// </summary>
    /// <param name="url">開きたいURL</param>
    public void OpenURL(string url)
    {
        Application.OpenURL(url); // URLを開く
    }
}
