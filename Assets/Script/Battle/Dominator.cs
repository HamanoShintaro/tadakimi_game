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

        //一度しか呼び出されないようにする
        private bool isStop = false;
        private CharacterCore characterCore;

        public enum TypeLeader
        {
            BuddyLeader,
            EnemyLeader
        }

        private void Start()
        {
            characterCore = GetComponent<CharacterCore>();
        }

        private void Update()
        {
            if (isStop) return;
            if (characterCore.Hp == 0)
            {
                //ゲームをストップ
                GameObject.Find("Canvas_Dynamic").GetComponent<BattleController>().GameStop(type);
                isStop = true;
            }
        }
    }
}
