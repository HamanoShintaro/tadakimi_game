using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class Obon : Skill
    {
        /// <summary>
        /// 攻撃力5倍+KB確定
        /// </summary>
        /// <param name="target"></param>
        protected override void SkillActionToEnemy(GameObject target)
        {
            var rate = 5;
            var attack = GetStatus().attack * rate;
            target.GetComponent<IDamage>().Damage(attack, Mathf.Infinity);
        }
    }
}