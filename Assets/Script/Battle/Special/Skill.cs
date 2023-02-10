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

        protected List<GameObject> enemyTargets = new List<GameObject>();
        protected List<GameObject> buddyTargets = new List<GameObject>();

        protected virtual void OnEnable()
        {
            StartCoroutine(Action());
        }

        protected virtual void OnDisable()
        {

        }

        protected virtual void OnTriggerStay(Collider target)
        {
            if (target.CompareTag("Enemy"))
            {
                enemyTargets.Add(target.gameObject);
            }
            else if (target.CompareTag("Buddy"))
            {
                buddyTargets.Add(target.gameObject);
            }
        }

        private IEnumerator Action()
        {
            yield return null;
            foreach (GameObject target in enemyTargets)
            {
                SkillActionToEnemy(target);
            }
            foreach (GameObject target in buddyTargets)
            {
                SkillActionforBuddy(target);
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
