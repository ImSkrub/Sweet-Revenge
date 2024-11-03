using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatusManager : MonoBehaviour
{
    private Stack<PlayerMemento> savedStates = new Stack<PlayerMemento>();
    public PlayerLife player;

    private void Awake()
    {
       player.OnDeath += Checkpoint;
    }

    public void Checkpoint()
    {
        if (savedStates.Count > 0)
        {
            PlayerMemento lastSavedState = savedStates.Pop();
            player.RestoreState(lastSavedState);
            Debug.Log("Estado restaurado");
        }
    }

    //Trigger para agarrar el checkpoint
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            savedStates.Push(player.SaveState());
            Debug.Log("Saved State");
        }
    }
}
