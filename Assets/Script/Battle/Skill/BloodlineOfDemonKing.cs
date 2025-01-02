using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BloodlineOfDemonKing : Skill
    {
        /// <summary>
        /// 回復(攻撃力の3倍)+攻撃力2倍（攻撃力は1度だけ増加）
        /// </summary>
        /// <param name="target"></param>
        protected override void SkillActionforBuddy(GameObject target)
        {
            var characterInfo = target.GetComponent<CharacterCore>();

            // 攻撃力の3倍で回復
            characterInfo.Recovery(GetStatus().attack * 3);

            // まだ攻撃力が2倍になっていなければ、攻撃力を2倍にする
            if (!characterInfo.hasDoubleAttackPower)
            {
                characterInfo.atkPower *= 2;
                characterInfo.hasDoubleAttackPower = true; // 2倍効果を適用したフラグを立てる
            }
        }
    }
}


