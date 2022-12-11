using UnityEngine;

public class PlayerDetection : MonoBehaviour
{

    // Movement Variables
    public MoveType moveType;
    public Vector3 start;
    [Range(0, 1)]
    public float offsetStart;
    public Vector3 offset;
    public float speed = 1;

    // Object References
    Light2D light2D;
    public GameObject Player;

    // State Containers

    public Color[] stateColors = { Color.red, Color.white };
    public PatrolLightState currentState = PatrolLightState.Normal;

    // Firing Variables
    public GameObject projectile;
    public float projectileForce;
    public float shootTime;
    public float shootTimer;

    
    // Light Triggers
    public void OnLight2DEnter(GameObject player)
    {
        if (player.tag == "Player")
        {
            PlayerInSight();
            Player = player;
        }
    }
    public void OnLight2DExit(GameObject player)
    {
        if (player.tag == "Player" && currentState == PatrolLightState.PlayerInSight)
        {
            PlayerLost();
        }
    }

    
    void Start()
    {
        // Getting access to Light2D Script. 
        light2D = this.GetComponent<Light2D>();
        
        // Setting start based on moveType
        switch (moveType)
        {
            case MoveType.Rotate:

                start = (this.transform.rotation.eulerAngles);
                break;

            case MoveType.Translate:
                start = this.transform.position;
                break;
        }

    }
    

    void Update()
    {
        // If player in sight, then shoot
        if (currentState == PatrolLightState.PlayerInSight)
        {
            Shoot();
        }

        Move();
    }

    void PlayerInSight()
    {
        // Behaviour called when player enters the light
        shootTimer = shootTime;
        currentState = PatrolLightState.PlayerInSight;
        light2D.setColorWithoutAlpha(stateColors[(int)currentState]);
    }
    void PlayerLost()
    {
        // Behaviour called when player leaves light
        currentState = PatrolLightState.Normal;

        light2D.setColorWithoutAlpha(stateColors[(int)currentState]);
    }
   

    void Move()
    {
        switch (moveType)
        {
            case MoveType.Rotate:
                this.transform.rotation = Quaternion.Euler(start +
                    Mathf.PingPong(Time.time * speed + offsetStart, 1) * offset);
                break;

            case MoveType.Translate:
                this.transform.position = (start + Mathf.PingPong(Time.time * speed + offsetStart, 1) * offset);
                break;
        }

    }
    
    void Shoot()
    {

        shootTimer -= Time.deltaTime;
        if (shootTimer < 0)
        {
            GameObject go = Instantiate(projectile, this.transform.position, this.transform.rotation) as GameObject;
            go.GetComponent<Rigidbody2D>().AddForce( (-this.transform.position + Player.transform.position).normalized*projectileForce , ForceMode2D.Impulse);
            shootTimer = shootTime;
        }
    }

    

    public enum PatrolLightState
    {
        PlayerInSight = 0,
        Normal = 1
    };
    public enum MoveType
    {
        Rotate = 0,
        Translate = 1
    };
}