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

    // Start is called before the first frame update
    void Start()
    {
        clearStageText = clearStage.GetComponent<Text>();
        moneyText = money.GetComponent<Text>();
        senarioTalkScript = Resources.Load<SenarioTalkScript>(ResourcePath.senarioTalkScriptPath + PlayerPrefs.GetString(PlayerPrefabKeys.clearStageId));

        clearStageText.text = senarioTalkScript.stage;
        moneyText.text = PlayerPrefs.GetInt(PlayerPrefabKeys.playerMoney).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
