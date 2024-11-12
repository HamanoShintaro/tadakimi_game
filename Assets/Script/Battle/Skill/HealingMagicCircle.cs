using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class HealingMagicCircle : Skill
    {
        protected override void SkillActionforBuddy(GameObject target)
        {
            HealforBuddy();
        }

        private void HealforBuddy()
        {
            foreach (GameObject target in buddyTargets)
            {
                target.GetComponent<CharacterCore>().Recovery(GetStatus().attack * 5);
            }
        }

        protected override void SkillActionToEnemy(GameObject target)
        {
            HealToEnemy();
        }

        private void HealToEnemy()
        {
            foreach (GameObject target in enemyTargets)
            {
                target.GetComponent<CharacterCore>().Recovery(GetStatus().attack * 5);
            }
        }

        public void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}