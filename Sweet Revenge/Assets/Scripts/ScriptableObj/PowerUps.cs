using UnityEngine;

[CreateAssetMenu(fileName = "New Value", menuName = "Power Up Value")]

public class PowerUps : ScriptableObject
{
    [Header("Power Up Value")]
    [Space(2)]
    public float effect;
    public float cooldown;
    public LayerMask playerLayer;
    //[Header("References")]
    //public string Name;
}
