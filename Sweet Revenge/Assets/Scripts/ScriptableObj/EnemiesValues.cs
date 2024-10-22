using UnityEngine;

[CreateAssetMenu(fileName = "New Value", menuName = "Enemy Value")]

public class EnemiesValues : ScriptableObject
{
    [Header("Enemy Values")]
    [Space(2)]
    [Header("General")]
    public int damage;
    public float attackCooldown;
    public int velocity = 10;
    public float rangeToAttack;
    public float closestDist;
    public float lerpSpeedRotation;
    public LayerMask playerLayer;
    [Header("References")]
    public string Name;
}
