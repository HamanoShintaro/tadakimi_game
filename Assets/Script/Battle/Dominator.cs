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

        //TODOダメージ判定時に呼び出す
        private void Update()
        {
            if (this.GetComponent<CharacterCore>().Hp == 0)
            {
                //ゲームをストップ
                GameObject.Find("Canvas").GetComponent<BattleController>().GameStop(type);
            }
        }

    }
}
