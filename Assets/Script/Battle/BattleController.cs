using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{

    // 魔力関連
    private Dictionary<int, float> recovery_magic = new Dictionary<int, float>();
    private Dictionary<int, int> max_magic = new Dictionary<int, int>();
    public float magic_power;
    public int magic_level;
    public int magic_recovery_level;
    private float magic_recovery_adjust;

    public GameObject magicPower; // スクリプトを持つ対象
    private MagicPowerController magicPowerController;

    // Start is called before the first frame update
    void Start()
    {
        // 仮置き(ユーザー設定)
        magic_level = 1;

        magic_recovery_level = 0;
        magic_recovery_adjust = 1 + magic_level * 0.2f;
        
        // 魔力増加ペースの定義(DB移植)
        recovery_magic[1] = 5.0f * magic_recovery_adjust;
        recovery_magic[2] = 7.0f * magic_recovery_adjust;
        recovery_magic[3] = 10.0f * magic_recovery_adjust;
        recovery_magic[4] = 15.0f * magic_recovery_adjust;
        recovery_magic[5] = 22.5f * magic_recovery_adjust;
        recovery_magic[6] = 30.0f * magic_recovery_adjust;
        recovery_magic[7] = 40.0f * magic_recovery_adjust;

        // 魔力最大値の定義
        max_magic[1] = 50 + (magic_level - 1) * 5;
        max_magic[2] = 100 + (magic_level - 1) * 10;
        max_magic[3] = 150 + (magic_level - 1) * 15;
        max_magic[4] = 200 + (magic_level - 1) * 20;
        max_magic[5] = 300 + (magic_level - 1) * 30;
        max_magic[6] = 400 + (magic_level - 1) * 40;
        max_magic[7] = 500 + (magic_level - 1) * 50;

        magicPowerController = magicPower.GetComponent<MagicPowerController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (magic_recovery_level == 0) {
            UpMagicLevel();
        }
        
    }

    // 魔法レベルアップの処理
    public void UpMagicLevel()
    {
        Debug.Log("レベル処理");
        magic_recovery_level = magic_recovery_level + 1;
        magicPowerController.maxMagicPower = max_magic[magic_recovery_level];
        magicPowerController.recoverMagicPower = recovery_magic[magic_recovery_level];

    }
}
