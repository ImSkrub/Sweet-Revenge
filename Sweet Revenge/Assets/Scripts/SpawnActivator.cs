using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActivator : MonoBehaviour
{
    [SerializeField] private GameObject spawnGameObject;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            spawnGameObject.SetActive(true);
        }
    }
}
