using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretArea : MonoBehaviour
{
    public GameObject secretArea;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        secretArea.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
