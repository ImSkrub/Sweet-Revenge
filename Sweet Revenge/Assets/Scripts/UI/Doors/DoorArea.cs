using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorArea : MonoBehaviour
{
    [SerializeField] private Image message;
    [SerializeField] private TMP_Text text;
    [SerializeField] private int valueDoor;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            message.gameObject.SetActive(true);
            text.SetText($"To open this door you need /{valueDoor}");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        message.gameObject.SetActive(false);
    }
}
