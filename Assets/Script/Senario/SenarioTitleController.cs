using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SenarioTitleController : MonoBehaviour
{
    private string stageId;
    private SenarioTalkScript senarioTalkScript;

    public GameObject stageNumber;
    private Text stageNumberText;
    public GameObject stageTitle;
    private Text stageTitleText;
    private string title;

    void Start()
    {
        stageId = PlayerPrefs.GetString(PlayerPrefabKeys.currentStageId);
        senarioTalkScript = Resources.Load<SenarioTalkScript>($"{ResourcePath.senarioTalkScriptPath}{stageId}");

        stageNumberText = stageNumber.GetComponent<Text>();
        stageNumberText.text = senarioTalkScript.stage;
        stageTitleText = stageTitle.GetComponent<Text>();
        stageTitleText.text = "";
        title = senarioTalkScript.title;

        StartCoroutine(stageTitleAppear());
    }

    private IEnumerator stageTitleAppear() {
        yield return new WaitForSeconds(1.0f);
        int i = 0;

        while (title.Length > i)
        {
            stageTitleText.text += title[i];
            i++;
            yield return new WaitForSeconds(0.15f);
        }

    }
}
