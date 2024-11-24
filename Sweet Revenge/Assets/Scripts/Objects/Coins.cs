using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    private enum coinType
    {
        door,
        shop,
    }
    [SerializeField] private coinType _coinType;
    [SerializeField] private int pointsQuantity;
    [SerializeField] private AudioClip coinSound;
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (_coinType)
            {
                case coinType.door:
                    PointManager.Instance.AddDoorCoin(pointsQuantity);
                    break;
                case coinType.shop:
                    PointManager.Instance.AddShopCoin(pointsQuantity);
                    break;
                    
            }
            Destroy(gameObject);
        }
    }
}
