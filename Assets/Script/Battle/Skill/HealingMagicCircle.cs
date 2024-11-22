using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// Shandy : ヒーリングマジックサークル
    /// </summary>
    public class HealingMagicCircle : Skill
    {
        public int atkPower;

        protected override void SkillActionforBuddy(GameObject target)
        {
            foreach (GameObject buddyTarget in buddyTargets)
            {
                buddyTarget.GetComponent<CharacterCore>().Recovery(atkPower * 2);
                Debug.Log("<color=green>ヒーリングマジックサークル</color>");
            }
        }

        protected override void SkillActionToEnemy(GameObject target)
        {
            foreach (GameObject enemyTarget in enemyTargets)
            {
                enemyTarget.GetComponent<CharacterCore>().Recovery(atkPower * 10);
                Debug.Log("<color=red>ヒーリングマジックサークル</color>");
            }
        }

        public void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}