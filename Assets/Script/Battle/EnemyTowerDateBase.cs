using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
[CreateAssetMenu(fileName = "000", menuName = "EnemyTowerInfo")]
public class EnemyTowerDateBase : ScriptableObject
{
    [SerializeField]
    [Tooltip("最大体力")]
    public float maxHp = 500;
    [SerializeField]
    [Tooltip("敵タワーの画像")]
    public Sprite sprite;
}