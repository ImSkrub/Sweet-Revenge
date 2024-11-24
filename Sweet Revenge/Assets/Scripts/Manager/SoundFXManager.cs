using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip clip, Transform spawnTransform, float volume)
    {
        // spawnearlo en el GameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // asignar el AudioClip
        audioSource.clip = clip;

        // asignar volumen
        audioSource.volume = volume;

        // darle Play al sonido
        audioSource.Play();

        // obtener el tamaño del clip
        float clipLength = audioSource.clip.length;

        // destruir el clip
        Destroy(audioSource.gameObject, clipLength);
    }



    public void PlayRandomSoundFXClip(AudioClip[] clip, Transform spawnTransform, float volume)
    {
        int rand = Random.Range(0, clip.Length);

        // spawnearlo en el GameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // asignar el AudioClip
        audioSource.clip = clip[rand];

        // asignar volumen
        audioSource.volume = volume;

        // darle Play al sonido
        audioSource.Play();

        // obtener el tamaño del clip
        float clipLength = audioSource.clip.length;

        // destruir el clip
        Destroy(audioSource.gameObject, clipLength);
    }
}
