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
            float elapsedTime = 0f;
            while (elapsedTime < 30f)
            {
                foreach (GameObject target in buddyTargets)
                {
                    target.GetComponent<CharacterCore>().Recovery(GetStatus().attack * 5);
                }
                yield return wait;
                elapsedTime += 5f;
            }
            StopCoroutine(Heal());
            Destroy(this.gameObject);
        }

        protected override void OnDisable()
        {
            StopCoroutine(Heal());
        }
    }
}