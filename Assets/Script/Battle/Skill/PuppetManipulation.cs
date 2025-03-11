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

        //ノックバックの秒数(全体フレーム)
        private const float knockBackDuration = 0.5f;
        //ノックバックの距離
        private const float knockBackForce = 800f;
        //ノックバックする時の高さ
        private const float jumpHeight = 50f;

        private CharacterCore characterCore; // CharacterCoreをフィールドとして持つ

        void Start()
        {
            characterCore = GetComponent<CharacterCore>(); // GetComponentで取得
        }
        // CharacterCore の characterType を取得する
        public CharacterType GetCharacterType()
        {
            return characterCore.characterType;
        }

        // CharacterCore の originalY を取得する
        public float GetOriginalY()
        {
            return characterCore.originalY;
        }

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
                _puppetTank.GetComponent<CharacterCore>().level = GetLevel();
            }
            else
            {
                summonedPuppet = Instantiate(_puppet, transform.position, Quaternion.identity);
                _puppet.GetComponent<CharacterCore>().level = GetLevel();
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
            //orend.GetComponent<IDamage>().Damage(attack, atkKB);

            // **オレンドのノックバックを開始**
            StartCoroutine(OrendKnockBack());

        }

        /// <summary>
        /// 傀儡戦車を召喚+自身がノックバック
        /// </summary>
        /// <param name="target"></param>
        protected override void SkillActionforBuddy(GameObject target)
        {
            GameObject summonedPuppet;
            if (GetLevel().Equals(4))
            {
                summonedPuppet = Instantiate(_puppetTank, transform.position, Quaternion.identity);
                _puppetTank.GetComponent<CharacterCore>().level = GetLevel();
            }
            else
            {
                summonedPuppet = Instantiate(_puppet, transform.position, Quaternion.identity);
                _puppet.GetComponent<CharacterCore>().level = GetLevel();
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
            //orend.GetComponent<IDamage>().Damage(attack, atkKB);

            // **オレンドのノックバックを開始**
            StartCoroutine(OrendKnockBack());

        }

        private IEnumerator OrendKnockBack()
        {
            if (orend == null)
            {
                Debug.LogError("Orendがnullのため、ノックバック処理を実行できません");
                yield break; // ここで処理を中断
            }

            CharacterCore orendCore = orend.GetComponent<CharacterCore>();
            if (orendCore == null)
            {
                Debug.LogError("OrendにCharacterCoreがアタッチされていません");
                yield break; // ここで処理を中断
            }

            // キャラクターの種類に応じて、ノックバックの方向を決定する
            float direction = orendCore.characterType == CharacterType.Buddy ? -1 : 1;

            // ノックバック開始時の位置を取得
            float startY = orendCore.originalY; // CharacterCore から originalY を取得
            float elapsedTime = 0f;

            while (elapsedTime < knockBackDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / knockBackDuration;

                // X座標の移動
                float newX = orend.transform.position.x + direction * knockBackForce * Time.deltaTime;

                // Y座標の移動 (正弦波でジャンプの高さを表現)
                float newY = startY + jumpHeight * Mathf.Sin(t * Mathf.PI);

                // xが範囲外なら元の位置に戻す
                if (orendCore.IsOutOfBounds(newX))
                {
                    newX = orend.transform.position.x;
                }

                orend.transform.position = new Vector2(newX, newY);

                yield return null;
            }

            // ノックバックが終了したら、Y座標を元の位置に戻す
            orend.transform.position = new Vector2(orend.transform.position.x, startY);
        }
    }
}
