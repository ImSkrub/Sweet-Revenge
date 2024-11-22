using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Add a guntype for the BaseGun parent script
public enum GunType { Pistol, Shotgun, MachineGun }
[CreateAssetMenu(fileName = "New Values", menuName = "Parameters")]
public class Parameters : ScriptableObject
{
    [Header("Values Melee")]
    [Space(3)]
    public float damage = 10f;
    public float attackSpeed = 1.5f;
    public float attackCost = 30f;
    public float attackRange = 3f;
    public float knockbackForce = 5f;

    [Header("Values ranged")]
    public int maxBullets;
    public float bulletSpeed;
    public GunType gunType;
}
