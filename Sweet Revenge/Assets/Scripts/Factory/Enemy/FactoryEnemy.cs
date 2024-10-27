using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FactoryEnemy : MonoBehaviour
{
    [SerializeField] private Enemy[] enemies;
    private Dictionary<string, Enemy> idEnemy;
    private void Awake()
    {
        idEnemy = new Dictionary<string, Enemy>();
        foreach (Enemy enemy in enemies)
        {
        idEnemy.Add(enemy.Name, enemy);
        }
    }
    public Enemy CreateEnemy(string enemyType)
    {
        if (!idEnemy.TryGetValue(enemyType, out Enemy enemy))
        {
            throw new ArgumentException("This enemy doesn't exist");
        }
        return Instantiate(enemy);
    }
}
