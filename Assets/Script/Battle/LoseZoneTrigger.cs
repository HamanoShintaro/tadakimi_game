using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseZoneTrigger : MonoBehaviour
{
    private bool isStop = false;

    private void OnTriggerEnter2D(Collider2D t)
    {
        if (t.CompareTag("Enemy"))
        {
            if (IsLongRangeTrigger(t)) return;
            GameOver();
        }
    }

    private void GameOver()
    {
        if (isStop) return;
        GameObject.Find("Canvas_Dynamic").GetComponent<BattleController>().GameStop(TypeLeader.BuddyLeader);
        isStop = true;
    }
    

    private bool IsLongRangeTrigger(Collider2D t)
    {
        BoxCollider2D[] colliders = t.gameObject.GetComponents<BoxCollider2D>();

        // 2つ目のBoxCollider2D(攻撃範囲のCollider2D)が存在するか確認
        if (colliders.Length > 1 && t == colliders[1])
        {
            return true;
        }
        return false;
    }
}