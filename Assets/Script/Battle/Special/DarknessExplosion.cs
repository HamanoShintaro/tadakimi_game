using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class DarknessExplosion : Skill
    {
        /// <summary>
        /// 5倍攻撃力
        /// </summary>
        /// <param name="target"></param>
        protected override void SkillActionToEnemy(GameObject target)
        {
            var rate = 5;
            var attack = GetStatus().attack * rate;
            target.GetComponent<IDamage>().Damage(attack);
        }
    }
}
