using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarController : MonoBehaviour
{

    public GameObject clearStage;
    private Text clearStageText;
    public GameObject money;
    private Text moneyText;
    private SenarioTalkScript senarioTalkScript;
    private int moneyInt;

    // Start is called before the first frame update
    void Start()
    {
        clearStageText = clearStage.GetComponent<Text>();
        moneyText = money.GetComponent<Text>();
        senarioTalkScript = Resources.Load<SenarioTalkScript>(ResourcePath.senarioTalkScriptPath + PlayerPrefs.GetString(PlayerPrefabKeys.clearStageId));

        clearStageText.text = senarioTalkScript.stage;
        moneyInt = PlayerPrefs.GetInt(PlayerPrefabKeys.playerMoney);
        moneyText.text = moneyInt.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(moneyInt != PlayerPrefs.GetInt(PlayerPrefabKeys.playerMoney))
        {
            int diff = moneyInt - PlayerPrefs.GetInt(PlayerPrefabKeys.playerMoney);
            if (diff > 100) {
                moneyInt -= 100;
            } else if (diff > 0) {
                moneyInt--;
            } else if (diff < -100)
            {
                moneyInt += 100;
            } else if (diff < 0)
            {
                moneyInt++;
            }
            
            moneyText.text = moneyInt.ToString();
        }
    }
}
