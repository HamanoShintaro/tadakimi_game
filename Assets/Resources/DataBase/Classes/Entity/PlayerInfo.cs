using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "CratePlayerInfo")]
public class PlayerInfo : ScriptableObject
{
    [SerializeField]
    private int maxHP;

    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private float attackPower;

    [SerializeField]
    private float attackRange;

    [SerializeField]
    private Animator animator;

    public int MaxHp
    {
        get { return maxHP; }
    }
    public float MaxSpeed
    {
        get { return maxSpeed; }
    }
    public float AttackPower
    {
        get { return attackPower; }
    }
    public float AttackRange
    {
        get { return attackRange; }
    }
    public Animator Aimator
    {
        get { return animator; }
    }
}