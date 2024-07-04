using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
[RequireComponent(typeof(Camera))]
public class CameraStableAspect2 : MonoBehaviour
{
    [SerializeField]
    Camera refCamera;

    [SerializeField]
    int width = 1920;

    [SerializeField]
    int height = 1080;

    [SerializeField]
    float pixelPerUnit = 100f;

    int m_width = -1;
    int m_height = -1;

    void Awake()
    {
        if (refCamera == null)
        {
            refCamera = GetComponent<Camera>();
        }
        UpdateResolution();
        UpdateCamera();
        UpdateResolution();
        UpdateCameraWithCheck();
    }

    private void Update()
    {
        
    }

    void UpdateResolution()
    {
        // 現在の画面解像度を取得し、それに基づいてwidthとheightを更新
        width = Screen.width;
        height = Screen.height;
    }

    void UpdateCameraWithCheck()
    {
        if (m_width == Screen.width && m_height == Screen.height)
        {
            return;
        }
        UpdateCamera();
    }

    void UpdateCamera()
    {
        float screen_w = (float)Screen.width;
        float screen_h = (float)Screen.height;
        float target_w = (float)width;
        float target_h = (float)height;

        // アスペクト比
        float aspect = screen_w / screen_h;
        float targetAcpect = target_w / target_h;
        float orthographicSize = (target_h / 2f / pixelPerUnit);

        // 縦に長い場合
        if (aspect < targetAcpect)
        {
            float bgScale_w = target_w / screen_w;
            float camHeight = target_h / (screen_h * bgScale_w);
            refCamera.rect = new Rect(0f, (1f - camHeight) * 0.5f, 1f, camHeight);
        }
        // 横に長い場合
        else
        {
            // カメラのorthographicSizeを横の長さに合わせて設定しなおす
            float bgScale = aspect / targetAcpect;
            orthographicSize *= bgScale;

            float bgScale_h = target_h / screen_h;
            float camWidth = target_w / (screen_w * bgScale_h);
            refCamera.rect = new Rect((1f - camWidth) * 0.5f, 0f, camWidth, 1f);
        }

        refCamera.orthographicSize = orthographicSize;

        m_width = Screen.width;
        m_height = Screen.height;
    }
}
