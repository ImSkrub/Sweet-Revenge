using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryEnemy2 : MonoBehaviour
{
    [SerializeField] private Enemy[] enemies;
    private Dictionary<string, Enemy> enemiesDictionary = new Dictionary<string, Enemy>();

    private void Awake()
    {
        foreach (Enemy enemy in enemies)
        {
            enemiesDictionary.Add(enemy.Name, enemy);
        }
    }

    public Enemy Create(string id, Transform position)
    {
        if (enemiesDictionary.TryGetValue(id, out Enemy enemy))
        {
            return Instantiate(enemy, position.position, Quaternion.identity);
        }
        return null;
    }

    

}
