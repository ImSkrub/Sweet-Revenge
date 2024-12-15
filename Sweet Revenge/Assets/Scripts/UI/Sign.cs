using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    //Variables de textos
    public GameObject dialogCanvas; // Referencia al objeto de Canvas que contiene el componente de texto
    public Text dialogText; // Referencia al componente de texto en el Canvas
    public string texto;
    
    private void Start()
    {
        dialogCanvas.SetActive(false);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dialogCanvas.SetActive(true);
            ToggleUI();

        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dialogCanvas.SetActive(false);

        }
    }

    private void ToggleUI()
    {
        dialogText.text = texto;
    }
}
