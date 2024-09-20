using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{ 
    // 追従する対象のTransform（プレイヤーなど）
    [SerializeField] private Transform target;

    // カメラの移動速度
    [SerializeField] private float followSpeed = 5f;

    // カメラのX座標の制限範囲
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;

    // カメラのY座標の固定位置（高さ）
    [SerializeField] private float fixedY = 0f;

    // カメラのX座標の固定位置（プレイヤーからどのくらい横にずれるの位置）
    [SerializeField] private float fixedX = 0f;


    private void LateUpdate()
    {
        if (target != null)
        {
            // 目標位置を計算（対象のX座標と固定のY座標）
            Vector3 targetPosition = new Vector3(target.position.x + fixedX, fixedY, transform.position.z);

            // カメラのX座標を制限する
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);

            // 緩やかな追従のためにLerpを使用してカメラの位置を更新
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
