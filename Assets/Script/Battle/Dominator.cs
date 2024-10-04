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
        public TypeLeader type;

        private bool isStop = false;
        private CharacterCore characterCore;

        private void Start()
        {
            characterCore = GetComponent<CharacterCore>();
        }

        private void Update()
        {
            if (isStop) return;
            if (characterCore.Hp == 0)
            {
                GameObject.Find("Canvas_Dynamic").GetComponent<BattleController>().GameStop(type);
                isStop = true;
            }
        }
    }
}
