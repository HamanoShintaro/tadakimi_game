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
            WaitForSeconds wait = new WaitForSeconds(5);
            while (true)
            {
                foreach (GameObject target in buddyTargets)
                {
                    target.GetComponent<CharacterCore>().Recovery(GetStatus().attack * 5);
                }
                yield return wait;
            }
        }

        protected override void OnDisable()
        {
            StopCoroutine(Heal());
        }
    }
}