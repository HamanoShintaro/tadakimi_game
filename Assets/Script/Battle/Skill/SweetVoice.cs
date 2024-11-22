using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class SweetVoice : Skill
    {
        [SerializeField]
        private int rate = 3;

        protected override void SkillActionforBuddy(GameObject target)
        {
            foreach (GameObject buddyTarget in buddyTargets)
            {
                buddyTarget.GetComponent<CharacterCore>().atkPower *= rate;
            }
        }

        protected override void SkillActionToEnemy(GameObject target)
        {   
            foreach (GameObject enemyTarget in enemyTargets)
            {
                enemyTarget.GetComponent<CharacterCore>().atkPower *= rate;
            }
        }
    }
}