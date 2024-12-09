using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerBossFight : MonoBehaviour
{
    public PlayableDirector Timeline;
    public Boss boss;
    public Player player;

    private void Awake()
    {
        boss = FindObjectOfType<Boss>();
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Timeline.Play();
            player.cinematicPlaying = true;
            Timeline.stopped += OnTimelineStopped;
        }
        
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
       player.cinematicPlaying = false;
        boss.cinematicFinished = true; // Marca la cinemática como terminada

        // Desuscribirse del evento para evitar llamadas múltiples
        director.stopped -= OnTimelineStopped;
    }
}
