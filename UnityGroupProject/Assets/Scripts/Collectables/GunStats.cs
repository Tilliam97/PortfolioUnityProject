using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

[CreateAssetMenu]

public class GunStats : ScriptableObject 
{
    public AmmoTypes ammoType; 

    public int shootDamage; 
    public float shootRate; 
    public int shootDist; 

    public int CurGunMag; 
    public int MaxGunMag; 
    public int CurGunCapacity; 
    public int MaxGunCapacity; 

    public GameObject model; 
    public ParticleSystem hitEffect; 
    public AudioClip shootSound; 
    [Range(0, 1)] public float shootSoundVol; 

    public bool hasInfinteAmmo; 
}

