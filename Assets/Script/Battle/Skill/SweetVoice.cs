using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class SweetVoice : Skill
    {
        /// <summary>
        /// 味方の攻撃力を3倍
        /// </summary>
        /// <param name="target"></param>
        protected override void SkillActionforBuddy(GameObject target)
        {
            target.GetComponent<CharacterCore>().atkPower *= 3;
        }
    }
}