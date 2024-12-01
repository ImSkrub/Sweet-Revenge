using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImportantItem : MonoBehaviour
{
    //References
    [SerializeField] private Image imageItem;



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ToggleUI();
        }      
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ToggleUI();
        }
    }

    private void ToggleUI()
    {
        imageItem.gameObject.SetActive(!imageItem.gameObject.activeSelf);
    }
}
