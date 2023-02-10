using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class HealingMagicCircle : Skill
    {
        protected override void SkillActionforBuddy(GameObject target)
        {
            StartCoroutine(Heal());
        }
        
        private IEnumerator Heal()
        {
            while (true)
            {
                foreach (GameObject target in buddyTargets)
                {
                    target.GetComponent<CharacterCore>().Recovery(GetStatus().attack * 5);
                }
                yield return new WaitForSeconds(5);
            }
        }

        protected override void OnDisable()
        {
            StopCoroutine(Heal());
        }
    }
}