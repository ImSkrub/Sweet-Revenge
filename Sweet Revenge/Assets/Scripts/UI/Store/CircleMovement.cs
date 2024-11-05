using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    public Transform target; // The target position to move to
    public float speed = 5f; // Speed of the movement

    private void Update()
    {
        if (target != null)
        {
            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // Check if the circle has reached the target
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                Destroy(gameObject); // Destroy the circle after reaching the target
            }
        }
    }
}
