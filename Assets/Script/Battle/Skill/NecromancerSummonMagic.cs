using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class NecromancerSummonMagic : Skill
    {
        [SerializeField]
        private float probability;

        [SerializeField]
        private GameObject necromancer;

        [SerializeField]
        private GameObject necromancer2;

        [SerializeField]
        private Transform summonPosition;

        protected override void SkillActionforBuddy(GameObject target)
        {
            StartCoroutine(SummonNecromancer(target));
        }

        /// <summary>
        /// このインスタンスがある限り、味方ユニットが死んだ場合に死霊兵または強化死霊兵を召喚
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private IEnumerator SummonNecromancer(GameObject target = null)
        {
            if (target.Equals(null)) yield break;
            var characterCore = target.GetComponent<CharacterCore>();
            while(true)
            {
                yield return null;
                if (characterCore.Hp.Equals(0))
                {
                    if (Random.value < probability) Instantiate(necromancer2, summonPosition.position, Quaternion.identity);
                    else Instantiate(necromancer, summonPosition.position, Quaternion.identity);
                }
            }
        }

        protected override void OnDisable()
        {
            StopCoroutine(SummonNecromancer());
        }
    }
}
