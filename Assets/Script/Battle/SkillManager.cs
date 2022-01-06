using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    // efectの召喚場所
    public GameObject efectPanel;
    public string prefabPath;
    private GameObject effectPrefab;

    private void Start() {
        // 召喚用のオブジェクト類のロード
        effectPrefab = Resources.Load<GameObject>(prefabPath);
    }

    public void ActivateSkill(string characterName,RectTransform rectTransform) {
        if (characterName == "volcus") {
            StartCoroutine(Volcus(rectTransform));
        }
    }

    private IEnumerator Volcus(RectTransform rectTransform)
    {
        yield return new WaitForSeconds(3.0f);

        GameObject effectClone = Instantiate(effectPrefab, efectPanel.transform);
        RectTransform effectRect = effectClone.GetComponent<RectTransform>();
        effectRect.position = new Vector3(rectTransform.position.x - 100.0f, effectRect.position.y);

        yield return new WaitForSeconds(0.2f);

        GameObject effectClone2 = Instantiate(effectPrefab, efectPanel.transform);
        RectTransform effectRect2 = effectClone2.GetComponent<RectTransform>();
        effectRect2.position = new Vector3(rectTransform.position.x - 200.0f, effectRect.position.y);

        yield return new WaitForSeconds(0.2f);

        GameObject effectClone3 = Instantiate(effectPrefab, efectPanel.transform);
        RectTransform effectRect3 = effectClone3.GetComponent<RectTransform>();
        effectRect3.position = new Vector3(rectTransform.position.x - 300.0f, effectRect.position.y);
    }
}
