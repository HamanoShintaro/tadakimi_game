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
        private float probabilityNecromancer = 90f; // インスペクターで変更可能 (デフォルト90%)

        public List<CharacterCore> deadCharacters = new List<CharacterCore>(); // HP 0 のキャラを記録

        private void Awake()
        {
            summonPosition = GameObject.Find("Canvas_Static/[CharacterPanel]/Appear_Buddy").transform;
        }
        
        /// <summary>
        /// 死亡キャラのレベルに応じて、対応する死霊兵を1体召喚する
        /// </summary>
        public void SummonNecromancer(int level)
        {
            // 確率に応じて召喚する死霊兵を決定
            GameObject baseNecromancer = (Random.Range(0f, 100f) < probabilityNecromancer) ? necromancer : necromancer2;

            // **プレハブをインスタンス化**
            GameObject summonedNecromancer = Instantiate(baseNecromancer, summonPosition.position, Quaternion.identity);
            summonedNecromancer.transform.parent = summonPosition.parent;

            // **召喚された necromancer の CharacterCore に死亡キャラのレベルをセット**
            CharacterCore necromancerCore = summonedNecromancer.GetComponent<CharacterCore>();
            if (necromancerCore != null)
            {
                necromancerCore.level = level;
                necromancerCore.SetCharacterInfo(level);
                Debug.Log($"<color=red>レベル {level} の死霊兵 ({summonedNecromancer.name}) を召喚しました！</color>");
            }
            // 召喚位置を決定
            var pos = summonedNecromancer.transform.localPosition;
            var random = Random.Range(minY, maxY);
            pos.x = summonPosition.localPosition.x;
            pos.y = summonPosition.localPosition.y + random;
            pos.z = summonPosition.localPosition.z;
            summonedNecromancer.transform.localPosition = pos;
            summonedNecromancer.transform.SetAsFirstSibling();
        }
        
        /// <summary>
        /// シーン内のプレイヤー以外のキャラクターのHPを常に監視し、HPが0になったらSaraSkillActionを発動
        /// </summary>
        private IEnumerator WatchCharacterHP()
        {
            while (true)
            {
                yield return null;

                // HP 0 になったキャラを探す (プレイヤー & 召喚キャラ Summon_01, Summon_02 を除外)
                var newDeadCharacters = FindObjectsOfType<CharacterCore>()
                    .Where(c => c.Hp == 0 &&
                                c.GetCharacterId().ToString() != "Player" &&
                                c.GetCharacterId().ToString() != "Summon_01" &&
                                c.GetCharacterId().ToString() != "Summon_02" &&
                                !deadCharacters.Contains(c)) //ここで既にリストにあるキャラは除外
                    .ToList();

                foreach (var deadCharacter in newDeadCharacters)
                {
                    deadCharacters.Add(deadCharacter); // リストに追加
                    
                    /*
                    if (skillCost <= magicPowerController.magicPower)
                    {
                        SaraSkillAction(deadCharacter); // 死亡キャラのレベルを渡してスキル発動
                    }
                    */
                    SaraSkillAction(deadCharacter); // 死亡キャラのレベルを渡してスキル発動
                    // 古いリストを削除
                    RemoveOldestDeadCharacter();
                }
            }
        }

        private void SaraSkillAction(CharacterCore deadCharacter)
        {
            //magicPowerController.magicPower -= skillCost;

            SummonNecromancer(deadCharacter.level); // 死亡キャラのレベルを渡して召喚
        }
        //リストの削除
        private void RemoveOldestDeadCharacter()
        {
            if (deadCharacters.Count > 10)
            {
                CharacterCore oldestCharacter = deadCharacters[0]; // 一番古いキャラ (リストの先頭)
                deadCharacters.RemoveAt(0); // リストから削除
            }
        }
    }
}
