using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// Volcus : 爆闇柱 : ダークネスエクスプロージョン
    /// </summary>
    public class DarknessExplosion : Skill
    {
        /// <summary>
        /// 5倍攻撃力にする
        /// </summary>
        /// <param name="target"></param>
        protected override void SkillActionforBuddy(GameObject target)
        {
            var rate = 5;
            var status = GetStatus();
            if (status == null)
            {
                Debug.LogError("Status is null.");
                return;
            }

            var attack = status.attack * rate;
            if (target == null)
            {
                Debug.Log("target is null");
                return;
            }

            var targetCore = target.GetComponent<CharacterCore>();
            if (targetCore == null)
            {
                Debug.LogError("CharacterCore component is missing on the target GameObject.");
                return;
            }

            targetCore.atkPower = attack;
        }
    }
}
