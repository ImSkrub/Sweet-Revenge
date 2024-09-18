using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] GameObject spawnPoint;
    [SerializeField] float vel;

    Vector3 newPos;

    private void Update()
    {
        newPos = Vector3.Lerp(transform.position, spawnPoint.transform.position, vel * Time.deltaTime);
        newPos.z = -10;

        transform.position = newPos;
    }
}
