using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFactory : MonoBehaviour
{
    [SerializeField] private string enemyName;
    [SerializeField] private FactoryEnemy2 factory;
    [SerializeField] private Transform spawnPoint, spawnPoint2, spawnPoint3, spawnPoint4, spawnPoint5;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            factory.Create(enemyName, spawnPoint);
            factory.Create(enemyName, spawnPoint2);
            factory.Create(enemyName, spawnPoint3);
            factory.Create(enemyName, spawnPoint4);
            factory.Create(enemyName, spawnPoint5);
            Destroy(gameObject);
        }
    }

}
