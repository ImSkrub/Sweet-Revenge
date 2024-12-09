using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ImportantItem : MonoBehaviour
{
    //Variables de textos
    public GameObject dialogCanvas; // Referencia al objeto de Canvas que contiene el componente de texto
    public Text dialogText; // Referencia al componente de texto en el Canvas
    public string texto;
    public Transform tpSpawn;
    public bool item1=false;
    [SerializeField] private AudioClip clip;
      

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
            if (Input.GetKey(KeyCode.E))
            {
                OnInteract();
                collision.gameObject.transform.position = tpSpawn.position;
                SoundFXManager.instance.PlaySoundFXClip(clip, transform, 1f);
            }

        }      
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dialogCanvas.SetActive(false);
           
        }
    }

    private void OnInteract()
    {
        if (item1)
        {
             GameManager.Instance.gotItem1 = true;
        }
        else
        {
            GameManager.Instance.gotItem2 = true;
        }
    }

    private void ToggleUI()
    {
       dialogText.text = texto;
    }
}
