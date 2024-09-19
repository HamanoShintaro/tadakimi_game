using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class PuppetManipulation : Skill
    {
        [SerializeField]
        private GameObject orend; //このスキルを発動したオレンドを格納

        [SerializeField]
        private GameObject _puppet;//召喚するパペットを格納

        [SerializeField]
        private GameObject _puppetTank;//召喚するパペットタンクを格納

        /// <summary>
        /// 傀儡戦車を召喚+自身がノックバック
        /// </summary>
        /// <param name="target"></param>
        protected override void SkillActionToEnemy(GameObject target)
        {
            if (GetLevel().Equals(10))
            {
                var puppetTank = Instantiate(_puppetTank, transform.position, Quaternion.identity);
                puppetTank.transform.parent = GameObject.Find("Canvas_Static/[CharacterPanel]").transform;
                puppetTank.GetComponent<CharacterCore>().level = GetLevel();
            }
            else
            {
                var puppet = Instantiate(_puppet, transform.position, Quaternion.identity);
                puppet.transform.parent = GameObject.Find("Canvas_Static/[CharacterPanel]").transform;
                puppet.GetComponent<CharacterCore>().level = GetLevel();
            }
            var attack = 0;
            var atkKB = Mathf.Infinity;
            orend.GetComponent<IDamage>().Damage(attack, atkKB);
        }
    }
}
