using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// その命、次は大事にしてくださいね
    /// </summary>
    public class GuardWell : Skill
    {
        /// <summary>
        /// 通常の40倍の攻撃力を与える
        /// </summary>
        /// <param name="target"></param>
        protected override void SkillActionforBuddy(GameObject target)
        {
            var rate = 40;
            var attack = GetStatus().attack * rate;
            target.GetComponent<IDamage>().Damage(attack);
        }
    }
}