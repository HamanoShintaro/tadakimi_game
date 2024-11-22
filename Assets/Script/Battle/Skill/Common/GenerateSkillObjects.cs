using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class GenerateSkillObjects : MonoBehaviour
{
    [SerializeField]
    private GameObject skillPrefab;

    [SerializeField]
    private float generateInterval = 5.0f;

    [SerializeField]
    private Vector2 generateOffset;

    public void GenerateSkillObject()
    {
        StartCoroutine(GenerateSkillObjectCoroutine());
    }

    private IEnumerator GenerateSkillObjectCoroutine()
    {
        yield return new WaitForSeconds(generateInterval);
        Vector3 generatePosition = new Vector3(transform.position.x + generateOffset.x, transform.position.y + generateOffset.y, transform.position.z);
        var skillObject = Instantiate(skillPrefab, generatePosition, transform.rotation);
        skillObject.transform.parent = GameObject.Find("Canvas_Static/[BackPanel]").transform;
        skillObject.GetComponent<HealingMagicCircle>().atkPower = (int)GetComponent<CharacterCore>().atkPower;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
