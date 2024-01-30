using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public enum AmmoTypes 
{
    PISTOL = 1, 
    SNIPER, 
    SHOTGUN
}

public interface IAmmoRefill
{
    void RefillAmmo( AmmoTypes ammoType, int ammoAmount ); 
}

