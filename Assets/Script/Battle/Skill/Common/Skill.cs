using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Battle
{
    /// <summary>
    /// スキルの継承元のクラス(インスタンスが生成されたら一度だけ実行される)
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Skill : MonoBehaviour
    {
        [SerializeField]
        protected CharacterId characterId;

        [SerializeField]
        protected AttackType attackType = AttackType.Single;

        public List<GameObject> enemyTargets = new List<GameObject>();
        public List<GameObject> buddyTargets = new List<GameObject>();

        protected virtual void OnDisable()
        {
            enemyTargets.Clear();
            buddyTargets.Clear();
        }

        public void SkillActionToTargets()
        {
            StartCoroutine(SkillActionToTargetsCoroutine());
        }

        protected virtual void OnTriggerStay2D(Collider2D target)
        {
            if (IsLongRangeTrigger(target)) return;
            if (target.CompareTag("Enemy"))
            {
                if (enemyTargets.Contains(target.gameObject)) return;
                enemyTargets.Add(target.gameObject);
            }
            else if (target.CompareTag("Buddy"))
            {
                if (buddyTargets.Contains(target.gameObject)) return;
                buddyTargets.Add(target.gameObject);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D target)
        {
            if (IsLongRangeTrigger(target)) return;
            if (target.CompareTag("Enemy"))
            {
                if (!enemyTargets.Contains(target.gameObject)) return;
                enemyTargets.Remove(target.gameObject);
            }
            else if (target.CompareTag("Buddy"))
            {
                if (!buddyTargets.Contains(target.gameObject)) return;
                buddyTargets.Remove(target.gameObject);
            }
        }

        private bool IsLongRangeTrigger(Collider2D t)
        {
            BoxCollider2D[] colliders = t.gameObject.GetComponents<BoxCollider2D>();

            // 2つ目のBoxCollider2D(攻撃範囲のCollider2D)が存在するか確認
            if (colliders.Length > 1 && t == colliders[1])
            {
                return true;
            }
            return false;
        }

        protected IEnumerator SkillActionToTargetsCoroutine()
        {
            yield return null;
            yield return null;
            foreach (GameObject target in enemyTargets)
            {
                SkillActionToEnemy(target);
                if (attackType == AttackType.Single) break;
            }
            foreach (GameObject target in buddyTargets)
            {
                SkillActionforBuddy(target);
                if (attackType == AttackType.Single) break;
            }
            Debug.Log("スキルアクション発動");
        }

        protected virtual void SkillActionToEnemy(GameObject target)
        {
        }

        protected virtual void SkillActionforBuddy(GameObject target)
        {

        }

        //ステータスを取得するメソッド
        protected CharacterInfo.Status GetStatus()
        {
            var status = Resources.Load<CharacterInfo>($"DataBase/Data/CharacterInfo/{characterId}").status[GetLevel()];
            return status;
        }

        //レベルを取得するメソッド
        protected int GetLevel()
        {
            //セーブデータを生成
            SaveController saveController = new SaveController();
            //セーブデータを取得
            saveController.characterSave.Load();
            //characterIdのセーブデータのレベルを取得
            var characterSaveData = saveController.characterSave.list.Find(characterSave => characterSave.id == characterId.ToString());
            if (characterSaveData == null)
            {
                Debug.LogError($"Character ID {characterId} のセーブデータが見つかりませんでした。");
                return 1;
            }
            var level = characterSaveData.level;
            return level;
        }
    }
}
