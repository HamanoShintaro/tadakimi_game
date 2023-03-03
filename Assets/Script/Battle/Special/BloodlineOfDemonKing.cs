using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BloodlineOfDemonKing : Skill
    {
        /// <summary>
        /// 回復(攻撃力の3倍)+攻撃力2倍
        /// </summary>
        /// <param name="target"></param>
        protected override void SkillActionforBuddy(GameObject target)
        {
            var characterInfo = target.GetComponent<CharacterCore>();
            characterInfo.atkPower *= 2;
            characterInfo.Recovery(GetStatus().attack * 3);
        }
    }
}
