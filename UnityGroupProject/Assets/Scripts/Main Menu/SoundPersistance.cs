using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPersistance : MonoBehaviour
{
    private static readonly string backgroundPref = "backgroundPref";
    private static readonly string SFXPref = "SFXPref";
    private float backgroundFloat;
    private float SFXFloat;
    public AudioSource backgroundAudio;
    public AudioSource[] SFXAudio;

    // Start is called before the first frame update
    void Awake()
    {
        ContinueSettings();
    }

    private void ContinueSettings()
    {
        backgroundFloat = PlayerPrefs.GetFloat(backgroundPref);
        SFXFloat = PlayerPrefs.GetFloat(SFXPref);

        backgroundAudio.volume = backgroundFloat;

        for (int i = 0; i < SFXAudio.Length; i++)
        {
            SFXAudio[i].volume = SFXFloat;
        }
    }
}
