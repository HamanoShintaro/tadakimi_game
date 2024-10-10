using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// イービルウィンド : 正面に攻撃力の3倍のダメージを与える。
/// </summary>
namespace Battle
{
    public class EvilWind : Skill
    {
        protected override void SkillActionforBuddy(GameObject target)
        {
            target.GetComponent<IDamage>().Damage(GetComponent<CharacterCore>().atkPower * 3);
            Debug.Log("<color=red>イービルウィンドが発動されました</color>");
        }
    }
}