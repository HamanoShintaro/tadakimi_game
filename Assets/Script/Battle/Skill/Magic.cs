using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Battle
{
    public class Magic : Skill
    {
        /// <summary>
        /// 2倍攻撃力
        /// </summary>
        /// <param name="target"></param>
        protected override void SkillActionToEnemy(GameObject target)
        {
            Heal(target);
        }

        private void Heal(GameObject target)
        {
            var rate = 2;
            var attack = GetStatus().attack * rate;
            target.GetComponent<IDamage>().Damage(attack);
        }
    }
}