using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFactory : MonoBehaviour
{
    [SerializeField] private FactoryEnemy2 factory;
    [SerializeField] private Transform spawnPoint, spawnPoint2, spawnPoint3, spawnPoint4, spawnPoint5;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            factory.Create("Zombie", spawnPoint);
            factory.Create("Zombie", spawnPoint2);
            factory.Create("Zombie", spawnPoint3);
            factory.Create("Zombie", spawnPoint4);
            factory.Create("Zombie", spawnPoint5);
            Destroy(gameObject);
        }
    }

}
