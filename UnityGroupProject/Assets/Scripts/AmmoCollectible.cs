using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

[CreateAssetMenu] 

public class AmmoCollectible : ScriptableObject 
{
    public int refillAmount; 

    public GameObject model; 
    public AudioClip collectSound; 
    [Range(0, 1)] public float collectSoundVol; 
}

