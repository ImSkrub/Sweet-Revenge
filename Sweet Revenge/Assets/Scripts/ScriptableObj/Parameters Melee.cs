using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Values", menuName = "Parameters")]
public class Parameters : ScriptableObject
{
    [Header("Values Melee")]
    [Space(3)]
    public float damage = 10f;
    public float attackSpeed = 1.5f;
    public float attackRange = 3f;
    public float knockbackForce = 5f;

    [Header("Values ranged")]
    public int maxBullets;
    public float bulletSpeed;
}
