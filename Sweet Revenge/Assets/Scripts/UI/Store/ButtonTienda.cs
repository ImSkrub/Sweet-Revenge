using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonTienda : MonoBehaviour
{
    public Transform tpLocation;
    [SerializeField] private TMP_Text text;
    private bool doOnce;
   
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            text.text = "Toca E para volver al inicio";
            if (Input.GetKeyUp(KeyCode.E)&&doOnce)
            {
                collision.gameObject.transform.position = tpLocation.position;
                doOnce = false;
            }
        }  
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        text.gameObject.SetActive(false);
    }

}
