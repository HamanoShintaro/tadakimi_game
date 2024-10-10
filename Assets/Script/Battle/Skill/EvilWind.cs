using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class EvilWind : Skill
    {
        protected override void SkillActionforBuddy(GameObject target)
        {
            target.GetComponent<IDamage>().Damage(GetStatus().attack * 3);
            Debug.Log("EvilWind");
        }
    }
}