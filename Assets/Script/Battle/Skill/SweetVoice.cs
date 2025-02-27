using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class SweetVoice : Skill
    {
        protected override void SkillActionforBuddy(GameObject target)
        {
            var atkPower = GetComponent<CharacterCore>().atkPower;
            foreach (GameObject buddyTarget in buddyTargets)
            {
                buddyTarget.GetComponent<CharacterCore>().atkPower += atkPower;
            }
        }

        protected override void SkillActionToEnemy(GameObject target)
        {
            foreach (GameObject enemyTarget in enemyTargets)
            {
                var attack = GetStatus().attack;
                enemyTarget.GetComponent<CharacterCore>().Recovery(attack * 5);
            }
        }
    }
}