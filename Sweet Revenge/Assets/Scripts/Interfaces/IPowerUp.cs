using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerUp 
{
    string Name { get; }
    void ApplyPowerUp(GameObject player);
}
