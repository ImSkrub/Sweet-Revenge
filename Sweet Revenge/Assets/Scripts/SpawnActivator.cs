using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnActivator : MonoBehaviour
{
    public float fadeDuration = 6f; // Duración del desvanecimiento en segundos
    [SerializeField] private Image filterImage;
    private float currentAlpha = 0f;
    private float targetAlpha = 0f; // Alpha objetivo
    private bool isFading = false;

    void Start()
    {
        if (filterImage != null)
        {
            // Asegurar que el alfa inicial sea 0
            Color color = filterImage.color;
            color.a = 0f;
            filterImage.color = color;
        }
    }

    void Update()
    {
        if (isFading)
        {
            // Cambiar gradualmente el alpha hacia el objetivo
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, Time.deltaTime / fadeDuration);

            // Actualizar el color del filtro
            if (filterImage != null)
            {
                Color color = filterImage.color;
                color.a = currentAlpha;
                filterImage.color = color;
            }

            // Detener el fade cuando se alcanza el alpha objetivo
            if (Mathf.Approximately(currentAlpha, targetAlpha))
            {
                isFading = false;
            }
        }
    }

    // Método para iniciar el fade hacia un alpha específico
    public void StartFadeTo(float alpha)
    {
        targetAlpha = Mathf.Clamp01(alpha); // Asegurar que el alpha esté entre 0 y 1
        isFading = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartFadeTo(0.05f); // Cambiar al 50% de opacidad
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartFadeTo(0f); // Regresar al 0% de opacidad
        }
    }
}
