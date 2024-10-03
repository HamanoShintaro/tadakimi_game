using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class TheaterSupport : Skill
    {
        protected override void SkillActionforBuddy(GameObject target)
        {
            Heal(target);
        }

        private void Heal(GameObject target)
        {
            var attack = GetStatus().attack;
            target.GetComponent<CharacterCore>().Recovery(attack);
        }
    }
}