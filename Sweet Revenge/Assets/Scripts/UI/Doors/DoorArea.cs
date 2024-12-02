using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorArea : MonoBehaviour
{
    [SerializeField] private Image message;
    [SerializeField] private GameObject door;
    [SerializeField] private TMP_Text text;
    [SerializeField] private int valueDoor;
    [SerializeField] private AudioClip doorSoundClip;
    public bool bossDoor = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&& !bossDoor)
        {
            message.gameObject.SetActive(true);
            // Get the current amount of coins the player has
            int currentCoins = PointManager.Instance._doorCoin;

            // Determine the color based on the current coins compared to the door value
            string color;
            if (currentCoins < valueDoor)
            {
                color = "red"; // Less than required
            }
            else
            {
                color = "green"; // Greater than or equal to required
            }
            if (Input.GetKey(KeyCode.F)&& currentCoins >= valueDoor)
            {
                SoundFXManager.instance.PlaySoundFXClip(doorSoundClip, transform, 1f);
                door.SetActive(false);
                message.gameObject.SetActive(false);
                PointManager.Instance._doorCoin -= valueDoor;
            }


            // Set the text with the determined color
            text.SetText($"<color=white>To open this door you need:</color> " +
                          $"<color={color}>{currentCoins}</color>/</color=white>{valueDoor}</color>" +
                          "Press F to buy");
        }
        if(collision.CompareTag("Player") && bossDoor)
        {
            if (GameManager.Instance.activeBossBattle)
            {
                SoundFXManager.instance.PlaySoundFXClip(doorSoundClip, transform, 1f);
                door.SetActive(false);
            }
            else
            {
              text.SetText($"<color=white>To open this door you need to grab the parts scatered on the map</color>");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject == null)
        {
            message.gameObject.SetActive(false);
        }
    }
}
