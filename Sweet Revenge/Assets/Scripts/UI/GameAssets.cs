using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;
    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }

    [Header("Sprite Assets")]
    public Sprite spikeBat;
    public Sprite warhammer;
    public Sprite pistol;
    public Sprite shotgun;
    public Sprite life;
    public Sprite dmgUp;
    public Sprite staminaUp;
    public Sprite shotSpeedUp;
}
