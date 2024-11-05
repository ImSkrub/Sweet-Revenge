using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GenerateItem : MonoBehaviour
{
    [SerializeField] private GameObject item; // Prefab de essence

    public void SpawnItem()
    {
        // Generar la esencia en la posición del enemigo que ha muerto
        Instantiate(item, transform.position, Quaternion.identity);
    }
}
