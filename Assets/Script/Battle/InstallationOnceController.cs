using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallationOnceController : MonoBehaviour
{
    // �_���[�W�ʂɊւ���ݒ�
    public int attack = 10;

    // �p������(effect�ƍ��킹��)
    public float attackTime;
    public float destroyTime;

    private GameObject target;
    private List<GameObject> targets = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Attack");
    }

    // �U���Ώۂ̑I��Fplayer�̏����Ȃ�G�ɁA�G�Ȃ�v���C���[�ɁB
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            if ((this.gameObject.tag == "player" && other.gameObject.tag == "enemy")
                || this.gameObject.tag == "enemy" && other.gameObject.tag == "player")
            {
                targets.Add(other.gameObject);
            }
        }
    }
    // �g���K�[���������ꂽ��^�[�Q�b�g����O��
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            if ((this.gameObject.tag == "player" && other.gameObject.tag == "enemy")
                   || this.gameObject.tag == "enemy" && other.gameObject.tag == "player")
            {
                targets.Remove(other.gameObject);
            }
        }
    }
    private IEnumerator Attack()
    {
        Debug.Log("�G�t�F�N�g��Attack�������Ă΂�܂���");
        yield return new WaitForSeconds(attackTime);

        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                Debug.Log("targetinstance��" + target.GetInstanceID());
                target.GetComponent<movingEnemyPtn001>().hp =
                       (target.GetComponent<movingEnemyPtn001>().hp - this.attack);
            }
        }
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }

}
