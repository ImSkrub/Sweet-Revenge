using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    [SerializeField] AudioSource clip;
    [SerializeField] private float volumeFadeOff, volumeFadeOn, maxVolume;

    private void Update()
    {
        clip.volume -= volumeFadeOff * Time.deltaTime;
        clip.volume += volumeFadeOn * Time.deltaTime;
        if (clip.volume > maxVolume) clip.volume = maxVolume;
    }
}
