using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// ゲームを終了するメソッド(プレイヤーキャラクターor敵タワーに追加)
    /// </summary>
    public class Dominator : MonoBehaviour
    {
        [SerializeField]
        public TypeLeader type;

        public enum TypeLeader
        {
            BuddyLeader,
            EnemyLeader
        }

        private void Update()
        {
            if (GetComponent<CharacterCore>().Hp == 0)
            {
                //ゲームをストップ
                GameObject.Find("Canvas_Dynamic").GetComponent<BattleController>().GameStop(type);
            }
        }
    }
}
