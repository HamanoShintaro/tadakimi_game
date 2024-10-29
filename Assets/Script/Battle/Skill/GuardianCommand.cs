using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// 地面から金の鎖を召喚し、範囲内の味方ユニットに攻撃力の2倍のダメージを与え、ノックバックさせる。
    /// </summary>
    public class GuardianCommand : Skill
    {
        /// <summary>
        /// 攻撃力2倍+KB確定
        /// </summary>
        /// <param name="target"></param>
        protected override void SkillActionforBuddy(GameObject target)
        {
            var rate = 2;
            var attack = GetStatus().attack * rate;
            target.GetComponent<IDamage>().Damage(attack, Mathf.Infinity);
            Debug.Log("<color=red>attack: " + attack + "</color>");
        }
    }
}