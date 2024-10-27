using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryPowerUp : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUpPrefabs;
    private Dictionary<string, GameObject> idPowerUp;

    private void Awake()
    {
        idPowerUp = new Dictionary<string, GameObject>();

        foreach (var prefab in powerUpPrefabs)
        {
            IPowerUp powerUp = prefab.GetComponent<IPowerUp>();
            if (powerUp != null)
            {
                idPowerUp.Add(powerUp.Name, prefab);
            }
        }
    }
   public IPowerUp CreatePowerUp(string powerUpType)
    {
        if (idPowerUp.ContainsKey(powerUpType))
        {
            GameObject powerUpInstance = Instantiate(idPowerUp[powerUpType]);
            return powerUpInstance.GetComponent<IPowerUp>();  
        }
        throw new ArgumentException("This powerUp doesn't exist");
    }
}
