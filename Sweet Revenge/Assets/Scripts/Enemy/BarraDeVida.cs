using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    private Slider slider;
    private void Start()
    {
        slider = GetComponent<Slider>();
    }
    public void CambiarVidaActual(float cantidadVida)
    {
        slider.value = cantidadVida;    
    }
    public void InicializarBarraVida(float cantidadVida) 
    {
        CambiarVidaActual(cantidadVida);
    }
}
