using UnityEngine;
using System.Collections;

public class LiftTrigger : MonoBehaviour {

    public GameObject Lift;
    Vector3 startPosition;
    public Vector3 endPosition;
    float t;
    [Range(0,1)]
    public float speed;
    float distance;
    Vector3 direction;

    public GameObject gear;
    public float gearSpeed;

    void Start()
    {
        startPosition = Lift.transform.position;
        direction = (-startPosition + endPosition).normalized;
        distance = (Lift.transform.position - endPosition).magnitude;
    }

    void OnTriggerStay2D(Collider2D co)
    {

        if (t < (distance / speed)) {
            Debug.Log((Lift.transform.position - endPosition).magnitude);
            Lift.transform.position = startPosition + direction * t * speed;

            AnimateGear();
        }
        t += Time.deltaTime;
    }

    void AnimateGear()
    {
        if (gear != null)
            gear.transform.Rotate(0, 0, gearSpeed * Time.deltaTime);
    }
}
