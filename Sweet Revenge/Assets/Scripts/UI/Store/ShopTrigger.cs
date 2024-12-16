using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    [SerializeField] private Transform shopSpawnpoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ShopManager.instance.ToggleShop();
            ShopManager.instance.SetSpawnPoint(shopSpawnpoint);
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ShopManager.instance.ToggleShop();
        }
    }
}
