using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public SpawnPoint.SpawnArea areaToActivate; // El área que se activará al entrar en el trigger
    private SpawnerEnemy enemySpawner; // Referencia al EnemySpawner

    private void Start()
    {
        // Encuentra el EnemySpawner en la escena
        enemySpawner = FindObjectOfType<SpawnerEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Cambia el área actual en el EnemySpawner
            enemySpawner.ChangeArea(areaToActivate);
        }
    }
}
