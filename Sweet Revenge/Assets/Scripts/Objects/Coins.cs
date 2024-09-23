using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField] private float pointsQuantity;
    [SerializeField] private UI_Coins points;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            points.AddCoins(pointsQuantity);
            Destroy(gameObject);
        }
    }
}
