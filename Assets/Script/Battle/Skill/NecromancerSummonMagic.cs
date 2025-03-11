using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            summonedNecromancer.GetComponent<CharacterCore>().level = GetComponent<CharacterCore>().level;

            // **召喚された necromancer の CharacterCore に死亡キャラのレベルをセット**
            CharacterCore necromancerCore = summonedNecromancer.GetComponent<CharacterCore>();
            if (necromancerCore != null)
            {
                necromancerCore.level = level; // **生成後にレベルを設定**
                Debug.Log($"<color=red>レベル {level} の死霊兵 ({summonedNecromancer.name}) を召喚しました！</color>");
            }
            else
            {
                Debug.LogError($"{summonedNecromancer.name} に CharacterCore がアタッチされていません！");
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
    }
}
