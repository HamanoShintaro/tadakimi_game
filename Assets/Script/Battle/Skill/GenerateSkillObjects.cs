using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSkillObjects : MonoBehaviour
{
    [SerializeField]
    private GameObject skillPrefab;

    [SerializeField]
    private float generateInterval = 5.0f;

    [SerializeField]
    private Vector2 generateOffset;

    private void OnEnable()
    {
        StartCoroutine(GenerateSkillObjectRoutine());
    }

    private IEnumerator GenerateSkillObjectRoutine()
    {
        while (true)
        {
            Vector3 generatePosition = new Vector3(transform.position.x + generateOffset.x, transform.position.y + generateOffset.y, transform.position.z);
            Instantiate(skillPrefab, generatePosition, transform.rotation);
            yield return new WaitForSeconds(generateInterval);
        }
    }
}
