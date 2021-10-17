using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleLevelController : MonoBehaviour
{
    // �֘A�Q�[���I�u�W�F�N�g�̐ݒ�
    public GameObject canvas;
    public GameObject magicPower;

    // �Q�Ɛ�i�[�p�ϐ�
    private BattleController battleController;
    private MagicPowerController magicPowerController;
    private Animator animator;

    // ���x���A�b�v���̃R�X�g�ݒ�
    private Dictionary<int, int> level_up_cost = new Dictionary<int, int>();

    // ����p�t���O
    private bool status;
    private bool isMax;

    // �\���C����錾
    private Text currentLv;
    private Text cost;

    // Start is called before the first frame update
    void Start()
    {
        // �����̂��߂�gameObject�擾
        battleController = canvas.GetComponent<BattleController>();
        magicPowerController = magicPower.GetComponent<MagicPowerController>();
        currentLv = transform.Find("currentLv").gameObject.GetComponent<Text>();
        cost = transform.Find("cost").gameObject.GetComponent<Text>();
        animator = GetComponent<Animator>();

        // ���x���A�b�v�R�X�g�̐錾
        level_up_cost[0] = 0;
        level_up_cost[1] = 30;
        level_up_cost[2] = 60;
        level_up_cost[3] = 110;
        level_up_cost[4] = 150;
        level_up_cost[5] = 220;
        level_up_cost[6] = 300;

        // ���x���\�� �����ݒ�
        currentLv.text = "1";
        cost.text = level_up_cost[1].ToString();

        // �X�e�[�^�X�ɏ����l�ݒ�
        status = false;
        isMax = false;

    }

    // �X�e�[�^�X����
    void Update()
    {
        // ���x���}�b�N�X�̏ꍇ�̓X�e�[�^�X�X�V�����ɂ͓���Ȃ�
        if (!isMax) { 
            // ���x���A�b�v�\��Ԃ̏ꍇ�A�����ȂǂŃR�X�g���x�����Ȃ��Ȃ�����s��Ԃ֕ύX
            if (status) {
                if (magicPowerController.magicPower < level_up_cost[battleController.magic_recovery_level]) {
                    status = false;
                    this.GetComponent<EventTrigger>().enabled = false;
                    animator.SetBool("isValid", false);
                }
            } else
            // ���x���A�b�v�s��Ԃ̏ꍇ�A�����R�X�g����������N���b�N�\���
            {
                if (level_up_cost[battleController.magic_recovery_level] <= magicPowerController.magicPower)
                {
                    status = true;
                    this.GetComponent<EventTrigger>().enabled = true;
                    animator.SetBool("isValid", true);
                }
            }
        }
    }

    public void OnClick() {
        if (magicPowerController.UseMagicPower(level_up_cost[battleController.magic_recovery_level])) {
            Debug.Log("���x���A�b�v�ɐ������܂���");

            // ���x���}�b�N�X�ɂȂ�ꍇ��Max�t���O�𗧂Ă�
            if (battleController.magic_recovery_level == 6)
            {
                isMax = true;
            }

            // �{�^���������Ȃ�����
            status = false;
            this.GetComponent<EventTrigger>().enabled = false;
            animator.SetBool("isValid", false);

            // ���x���A�b�v�������s
            battleController.UpMagicLevel();

            // �\���̐؂�ւ����s��
            currentLv.text = battleController.magic_recovery_level.ToString();
            if (isMax) {
                cost.text = "-";
            } else {
                cost.text = level_up_cost[battleController.magic_recovery_level].ToString();
            }
        }
    }
}
