using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        public Transform summonPosition;

        [SerializeField]
        private int minY = -30, maxY = 30;

        private void Awake()
        {
            summonPosition = GameObject.Find("Canvas_Static/[CharacterPanel]/Appear_Buddy").transform;
        }

        private void Start()
        {
            StartCoroutine(SummonNecromancer());
        }

        /// <summary>
        /// このインスタンスがある限り、味方ユニットが死んだ場合に死霊兵または強化死霊兵を召喚
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private IEnumerator SummonNecromancer()
        {
            while(true)
            {
                yield return null;
                foreach (GameObject buddy in buddyTargets)
                {
                    CharacterCore buddyCore = buddy.GetComponent<CharacterCore>();
                    if (buddyCore.Hp == 0)
                    {
                        if (buddy.name == "Player") yield break;

                        GameObject summonedNecromancer = Random.value < probability ? Instantiate(necromancer2, summonPosition.position, Quaternion.identity) : Instantiate(necromancer, summonPosition.position, Quaternion.identity);
                        summonedNecromancer.transform.parent = summonPosition.parent;

                        // 生成位置を決定
                        var pos = summonedNecromancer.transform.localPosition;
                        var random = Random.Range(minY, maxY);
                        pos.x = summonPosition.localPosition.x;
                        pos.y = summonPosition.localPosition.y + random;
                        pos.z = summonPosition.localPosition.z;
                        summonedNecromancer.transform.localPosition = pos;
                        summonedNecromancer.transform.SetAsFirstSibling();

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

                        buddyCore.Hp = 1;
                        Debug.Log("<color=red>召喚</color>");
                    }
                }
            }
        }

        protected override void OnDisable()
        {
            StopCoroutine(SummonNecromancer());
        }
    }
}
