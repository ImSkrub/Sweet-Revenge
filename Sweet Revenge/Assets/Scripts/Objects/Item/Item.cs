using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
   public enum ItemType
    {
        SpikeBat,
        Warhammer,
        Pistol,
        Shotgun,
        Life,
        DmgUp,
        Stamina,
        ShotSpeed
    }
    public static int GetCost(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.SpikeBat: return 0;
            case ItemType.Warhammer: return 1000;
            case ItemType.Pistol: return 750;
            case ItemType.Shotgun: return 1500;
            case ItemType.Life: return 200;
            case ItemType.DmgUp: return 300;
            case ItemType.Stamina: return 500;
            case ItemType.ShotSpeed: return 600;
        }
    }
    public static Sprite GetSprite(ItemType itemType)
    {
        switch (itemType)
        {
            default:
                case ItemType.SpikeBat: return GameAssets.i.spikeBat;
                case ItemType.Warhammer: return GameAssets.i.warhammer;
                case ItemType.Pistol:return GameAssets.i.pistol;
                case ItemType.Shotgun:return GameAssets.i.shotgun;
                case ItemType.Life: return GameAssets.i.life;
                case ItemType.DmgUp:return GameAssets.i.dmgUp;
                case ItemType.Stamina: return GameAssets.i.staminaUp;
                case ItemType.ShotSpeed: return GameAssets.i.shotSpeedUp;
        }
    }
}
