using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{

    // ???????A
    private Dictionary<int, float> recovery_magic = new Dictionary<int, float>();
    private Dictionary<int, int> max_magic = new Dictionary<int, int>();
    public float magic_power;
    public int magic_level;
    public int magic_recovery_level;
    private float magic_recovery_adjust;

    public GameObject magicPower; // ?X?N???v?g??????????
    private MagicPowerController magicPowerController;
    public string resultType = "";
    public GameObject performancePanel;

    // Start is called before the first frame update
    void Start()
    {
        // ???u??(???[?U?[????)
        magic_level = 1;

        magic_recovery_level = 0;
        magic_recovery_adjust = 1 + magic_level * 0.2f;
        
        //????????
        recovery_magic[1] = 5.0f * magic_recovery_adjust;
        recovery_magic[2] = 7.0f * magic_recovery_adjust;
        recovery_magic[3] = 10.0f * magic_recovery_adjust;
        recovery_magic[4] = 15.0f * magic_recovery_adjust;
        recovery_magic[5] = 22.5f * magic_recovery_adjust;
        recovery_magic[6] = 30.0f * magic_recovery_adjust;
        recovery_magic[7] = 40.0f * magic_recovery_adjust;

        //???????
        max_magic[1] = 50 + (magic_level - 1) * 5;
        max_magic[2] = 100 + (magic_level - 1) * 10;
        max_magic[3] = 150 + (magic_level - 1) * 15;
        max_magic[4] = 200 + (magic_level - 1) * 20;
        max_magic[5] = 300 + (magic_level - 1) * 30;
        max_magic[6] = 400 + (magic_level - 1) * 40;
        max_magic[7] = 500 + (magic_level - 1) * 50;

        magicPowerController = magicPower.GetComponent<MagicPowerController>();

    }

    void Update()
    {
        if (magic_recovery_level == 0) {
            UpMagicLevel();
        }
    }

    //?????????
    public void UpMagicLevel()
    {
        Debug.Log("???x??????");
        magic_recovery_level = magic_recovery_level + 1;
        magicPowerController.maxMagicPower = max_magic[magic_recovery_level];
        magicPowerController.recoverMagicPower = recovery_magic[magic_recovery_level];
    }
    //?????
    public void viewResult(string type)
    {
        // type?? win or lose;
        // ?????????~????
        StartCoroutine(gameStop());
        // ???U???g?p?l?????\???????B???????Awin??lose?????U???g?p?l???????{??????
        resultType = type;
        performancePanel.SetActive(true);
    }
    //???????
    private IEnumerator gameStop() {
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 0;
    }
}
