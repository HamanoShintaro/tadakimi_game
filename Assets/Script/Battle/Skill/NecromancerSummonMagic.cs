using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Battle
{
    public class NecromancerSummonMagic : MonoBehaviour
    {
        [SerializeField]
        private GameObject necromancer;

        [SerializeField]
        private GameObject necromancer2;

        [SerializeField]
        public Transform summonPosition;

        [SerializeField]
        private int minY = -30, maxY = 30;

        [SerializeField, Range(0, 100)]
        private float probabilityNecromancer = 90f;

        public List<CharacterCore> deadCharacters = new List<CharacterCore>();

        private void Awake()
        {
            if (summonPosition == null)
            {
                summonPosition = GameObject.Find("Canvas_Static/[CharacterPanel]/Appear_Buddy")?.transform;
            }
        }

        private void Start()
        {
            var core = GetComponent<CharacterCore>();
            if (core != null && core.GetCharacterId().ToString() == "Sara_01")
            {
                StartCoroutine(WatchCharacterHP());
                Debug.Log("<color=green>Sara_01: 死亡キャラ監視開始</color>");
            }
        }

        public void SummonNecromancer(int level)
        {
            StartCoroutine(SummonNecromancerCoroutine(level));
        }

        private IEnumerator SummonNecromancerCoroutine(int level)
        {
            GameObject baseNecromancer = (Random.Range(0f, 100f) < probabilityNecromancer) ? necromancer : necromancer2;
            GameObject summoned = Instantiate(baseNecromancer, summonPosition.position, Quaternion.identity);
            yield return null;

            // 親を設定（Canvas内）
            Transform panel = GameObject.Find("Canvas_Static/[CharacterPanel]")?.transform;
            if (panel != null)
            {
                summoned.transform.SetParent(panel, false);
            }

            // レベル設定（CharacterCoreが存在する場合）
            CharacterCore core = summoned.GetComponent<CharacterCore>();
            if (core != null)
            {
                core.level = level;
                core.SetCharacterInfo(level);
                Debug.Log($"<color=red>レベル {level} の死霊兵 ({summoned.name}) を召喚しました！</color>");
            }

            // ランダムな生成位置調整
            var pos = summoned.transform.localPosition;
            pos.x = summonPosition.localPosition.x;
            pos.y = summonPosition.localPosition.y + Random.Range(minY, maxY);
            pos.z = summonPosition.localPosition.z;
            summoned.transform.localPosition = pos;

            // 表示順制御（最前面）
            summoned.transform.SetAsFirstSibling();
        }

        private IEnumerator WatchCharacterHP()
        {
            while (true)
            {
                yield return null;

                var newDeadCharacters = FindObjectsOfType<CharacterCore>()
                    .Where(c => c.Hp == 0 &&
                                c.GetCharacterId().ToString() != "Player" &&
                                c.GetCharacterId().ToString() != "Summon_01" &&
                                c.GetCharacterId().ToString() != "Summon_02" &&
                                !deadCharacters.Contains(c))
                    .ToList();

                foreach (var dead in newDeadCharacters)
                {
                    deadCharacters.Add(dead);
                    SaraSkillAction(dead);
                    RemoveOldestDeadCharacter();
                }
            }
        }

        private void SaraSkillAction(CharacterCore deadCharacter)
        {
            SummonNecromancer(deadCharacter.level);
        }

        private void RemoveOldestDeadCharacter()
        {
            if (deadCharacters.Count > 10)
            {
                deadCharacters.RemoveAt(0);
            }
        }
    }
}
