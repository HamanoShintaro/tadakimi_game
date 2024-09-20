using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class HealingMagicCircle : Skill
    {
        protected override void SkillActionforBuddy(GameObject target)
        {
            Heal();
        }

        private void Heal()
        {
            foreach (GameObject target in buddyTargets)
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