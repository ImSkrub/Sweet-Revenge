using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActivator : MonoBehaviour
{
    [SerializeField] private SpawnerEnemy spawnerEnemy; // Referencia al SpawnerEnemy

    private void Awake()
    {
        spawnerEnemy = FindFirstObjectByType<SpawnerEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
          
            spawnerEnemy.ActivateSpawnArea(); // Notifica al SpawnerEnemy
        }
    }
}
