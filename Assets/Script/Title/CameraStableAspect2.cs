using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] // エディタモードでも実行されるように設定
[RequireComponent(typeof(Camera))] // このスクリプトがアタッチされているGameObjectにCameraコンポーネントが必須
public class CameraStableAspect2 : MonoBehaviour
{
    [SerializeField]
    Camera refCamera; // 使用するカメラの参照。インスペクタから設定可能

    [SerializeField]
    int width = 1920; // ターゲット解像度の幅（デフォルトは1920）

    [SerializeField]
    int height = 1080; // ターゲット解像度の高さ（デフォルトは1080）

    [SerializeField]
    float pixelPerUnit = 100f; // 1単位あたりのピクセル数（スプライトのサイズに関連）

    // 固定するカメラのサイズ
    [SerializeField] private float fixedSize = 591.8f; // カメラのオーソグラフィックサイズを固定する値

    int m_width = -1; // 前回のスクリーン幅を記録するための変数
    int m_height = -1; // 前回のスクリーン高さを記録するための変数

    void Awake()
    {
        if (refCamera == null)
        {
            refCamera = GetComponent<Camera>(); // カメラが設定されていない場合、自動的に取得
        }
        refCamera.orthographicSize = fixedSize; // カメラのオーソグラフィックサイズを固定値に設定
        UpdateResolution(); // 現在の解像度を更新
        UpdateCameraWithCheck(); // カメラ設定を確認して更新
    }

    void Update()
    {
        UpdateResolution(); // 現在の解像度を毎フレーム更新
        UpdateCameraWithCheck(); // カメラ設定を確認して必要なら更新
    }

    void UpdateResolution()
    {
        width = Screen.width; // 現在のスクリーン幅を取得して設定
        height = Screen.height; // 現在のスクリーン高さを取得して設定
    }

    void UpdateCameraWithCheck()
    {
        if (m_width != Screen.width || m_height != Screen.height)
        {
            UpdateCamera(); // スクリーンサイズが変更されている場合、カメラ設定を更新
        }
    }

    void UpdateCamera()
    {
        float screen_w = (float)Screen.width; // 現在のスクリーン幅をfloat型で取得
        float screen_h = (float)Screen.height; // 現在のスクリーン高さをfloat型で取得
        float target_w = (float)width; // ターゲット解像度の幅をfloat型で取得
        float target_h = (float)height; // ターゲット解像度の高さをfloat型で取得

        float aspect = screen_w / screen_h; // 現在のスクリーンアスペクト比を計算
        float targetAspect = target_w / target_h; // ターゲットアスペクト比を計算
        float orthographicSize = fixedSize; // オーソグラフィックサイズを固定値で設定

        // アスペクト比が縦に長い場合
        if (aspect < targetAspect)
        {
            float bgScale_w = target_w / screen_w; // 横方向のスケーリング比を計算
            float camHeight = target_h / (screen_h * bgScale_w); // カメラの高さを計算
            refCamera.rect = new Rect(0f, (1f - camHeight) * 0.5f, 1f, camHeight); // カメラの表示領域を縦に長い画面に合わせて調整
        }
        // アスペクト比が横に長い場合
        else
        {
            float bgScale = aspect / targetAspect; // 横方向のスケーリング比を計算
            orthographicSize *= bgScale; // 固定サイズを調整

            float bgScale_h = target_h / screen_h; // 縦方向のスケーリング比を計算
            float camWidth = target_w / (screen_w * bgScale_h); // カメラの幅を計算
            refCamera.rect = new Rect((1f - camWidth) * 0.5f, 0f, camWidth, 1f); // カメラの表示領域を横に長い画面に合わせて調整
        }

        refCamera.orthographicSize = orthographicSize; // 最終的なオーソグラフィックサイズを設定

        m_width = Screen.width; // 現在のスクリーン幅を記録
        m_height = Screen.height; // 現在のスクリーン高さを記録
    }
}
