using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerBossFight : MonoBehaviour
{
    public PlayableDirector Timeline;
    public Boss boss;

    private void Awake()
    {
        boss = FindObjectOfType<Boss>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Timeline.Play();
        boss.cinematicFinished = true;
        
    }

}
