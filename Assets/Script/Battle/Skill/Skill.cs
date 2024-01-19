using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// スキルの継承元のクラス(インスタンスが生成されたら一度だけ実行される)
    /// </summary>
    public class Skill : MonoBehaviour
    {
        [SerializeField]
        protected CharacterCore.CharacterId characterId;

        public List<GameObject> enemyTargets = new List<GameObject>();
        public List<GameObject> buddyTargets = new List<GameObject>();

        protected virtual void OnEnable()
        {
            StartCoroutine(Action());
        }

        protected virtual void OnDisable()
        {
            enemyTargets.Clear();
            buddyTargets.Clear();
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

        public IEnumerator Action()
        {
            yield return new WaitForSeconds(0.1f);
            foreach (GameObject target in enemyTargets)
            {
                SkillActionToEnemy(target);
                //Debug.Log("スキルアクション(敵へ)");
            }
            foreach (GameObject target in buddyTargets)
            {
                SkillActionforBuddy(target);
                //Debug.Log("スキルアクション(味方へ)");
            }
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
