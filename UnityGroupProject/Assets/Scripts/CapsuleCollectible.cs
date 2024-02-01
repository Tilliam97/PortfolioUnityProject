using System.Collections; 
using System.Collections.Generic; 
using UnityEngine;

public enum CapsuleType 
{
    HEALTH = 1, 
    A_PISTOL, 
    A_SHOTGUN, 
    A_SNIPER, 
    KEY 
}


[CreateAssetMenu] 

public class CapsuleCollectible : ScriptableObject 
{
    public int refillAmount; 
    public CapsuleType capsuleType; 

    public GameObject model; 
    public AudioClip collectSound; 
    [Range(0, 1)] public float collectSoundVol; 
}

