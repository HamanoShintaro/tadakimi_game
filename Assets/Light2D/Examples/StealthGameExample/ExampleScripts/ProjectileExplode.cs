using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Light2D))]
public class ProjectileExplode : MonoBehaviour {
    Light2D light2D;
    public float explosionRange;
    public float time = 0;
    public float ExplosionTime = 10;

    public AnimationCurve ExplosionCurve;
    // Use this for initialization
    void Awake () {
        light2D = this.GetComponent<Light2D>();
	}
	

    void OnCollisionEnter2D(Collision2D coll)
    {
        this.GetComponent<Rigidbody2D>().isKinematic=true;
        this.GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine("Explode");
    }

    public void OnLight2DEnter(GameObject player)
    {
        if (player.tag == "Player")
        {
            player.GetComponent<GameManager>().KillPlayer();
        }
    }

    IEnumerator Explode()
    {
        while(time < ExplosionTime)
        {
            light2D.Range = explosionRange*ExplosionCurve.Evaluate(time/ ExplosionTime);
            time += Time.deltaTime;
            yield return null;
        }
        light2D.Range = 0;
        
        yield return null;

    }


}
