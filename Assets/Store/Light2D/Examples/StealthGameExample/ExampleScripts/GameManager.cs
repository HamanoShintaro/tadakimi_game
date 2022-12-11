using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Vector3[] spawnPoints;
    public int currentSpawnPoint;
    public Text startText;

    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void KillPlayer()
    {
        this.transform.position = spawnPoints[currentSpawnPoint];

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "SpawnPoint")
        {
            currentSpawnPoint++;
            other.gameObject.SetActive(false);
        }
        if (other.name == "EndPoint")
        {
            currentSpawnPoint=0;
            this.transform.position = spawnPoints[currentSpawnPoint];
            startText.text = "Well done! Try Again?";
        }

    }
}
