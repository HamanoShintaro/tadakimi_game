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

        protected IEnumerator SkillActionToTargetsCoroutine()
        {
            yield return null;
            yield return null;
            foreach (GameObject target in enemyTargets)
            {
                SkillActionToEnemy(target);
            }
            foreach (GameObject target in buddyTargets)
            {
                SkillActionforBuddy(target);
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
            var level = saveController.characterSave.list.Find(characterSave => characterSave.id == characterId.ToString()).level;
            return level;
        }
    }
}
