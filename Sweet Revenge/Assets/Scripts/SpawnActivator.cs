using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActivator : MonoBehaviour
{
    [SerializeField] private GameObject spawnerEnemy; // Referencia al SpawnerEnemy
  

    private void Awake()
    {
        //spawnerEnemy = FindFirstObjectByType<SpawnerEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spawnerEnemy.SetActive(true);

            //// Verifica si el área actual es la misma que el área a activar
            //if (spawnerEnemy.CurrentArea == areaToActivate)
            //{
            //    // Cambia al área anterior
            //    spawnerEnemy.ActivateSpawnArea(previousArea);
            //}
            //else
            //{
            //    // Cambia al área a activar
            //    spawnerEnemy.ActivateSpawnArea(areaToActivate);
            //}
        }
    }
}
