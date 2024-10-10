using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

        [SerializeField]
        private int minY = -30, maxY = 30;

        /// <summary>
        /// 傀儡戦車を召喚+自身がノックバック
        /// </summary>
        /// <param name="target"></param>
        protected override void SkillActionToEnemy(GameObject target)
        {
            GameObject summonedPuppet;
            if (GetLevel().Equals(4))
            {
                summonedPuppet = Instantiate(_puppetTank, transform.position, Quaternion.identity);
            }
            else
            {
                summonedPuppet = Instantiate(_puppet, transform.position, Quaternion.identity);
            }
            summonedPuppet.transform.parent = GameObject.Find("Canvas_Static/[CharacterPanel]").transform;
            summonedPuppet.GetComponent<CharacterCore>().level = GetLevel();

            // 生成位置を決定
            var pos = summonedPuppet.transform.localPosition;
            var random = Random.Range(minY, maxY);
            pos.x = transform.localPosition.x;
            pos.y = transform.localPosition.y + random;
            pos.z = transform.localPosition.z;
            summonedPuppet.transform.localPosition = pos;
            summonedPuppet.transform.SetAsFirstSibling();

            // 辞書式を宣言
            var buddyDic = new Dictionary<GameObject, float>();
            // 辞書式にキャラクターを代入(AppearとPlayerを除くため-3を入れている)
            for (int i = 0; i < transform.childCount - 3; i++)
            {
                var childBuddy = transform.GetChild(i).gameObject;
                var rectTransform = childBuddy.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    buddyDic.Add(childBuddy, rectTransform.anchoredPosition.y);
                }
            }
            // 辞書式をvalueの大きい順に並べ替え
            var sortedKeys = buddyDic.OrderBy(x => x.Value).Select(x => x.Key).ToList();
            // 階層を並べ替え
            for (int i = 0; i < sortedKeys.Count; i++)
            {
                sortedKeys[i].transform.SetAsFirstSibling();
            }

            var attack = 0;
            var atkKB = Mathf.Infinity;
            orend.GetComponent<IDamage>().Damage(attack, atkKB);
        }
    }
}
