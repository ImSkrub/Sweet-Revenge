using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public enum SpawnArea
    {
        Start,
        Cueva,
        Cueva_Profunda,
        Volcan,
        Tierra
    }
    public SpawnArea Area;
    public bool IsActive { get; private set; } = true;

    public void EnableSpawn()
    {
        IsActive = true;
    }

    public void DisableSpawn()
    {
        IsActive = false;
    }

    public Vector3 GetSpawnPosition()
    {
        return transform.position;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
