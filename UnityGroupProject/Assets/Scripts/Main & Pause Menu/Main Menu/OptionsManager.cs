using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [Header("--------Audio--------")]
    private static readonly string firstStart = "firstStart";
    private static readonly string backgroundPref = "backgroundPref";
    private static readonly string SFXPref = "SFXPref";
    private int firstPlayInt;
    public Slider backgroundSlider;
    public Slider SFXSlider;
    private float backgroundFloat;
    private float SFXFloat;
    public AudioSource backgroundAudio;
    public AudioSource[] SFXAudio;

    [Header("--------Sensitivity--------")]
    private static readonly string YPref = "YSensitivityPref";
    private static readonly string XPref = "XSensitivityPref";
    public Slider XSlider, YSlider;
    private float XFloat, YFloat;


    // Start is called before the first frame update
    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(firstStart);

        if (firstPlayInt == 0 )
        {
            backgroundFloat = .25f;
            SFXFloat = .5f;
            backgroundSlider.value = backgroundFloat;
            SFXSlider.value = SFXFloat;
            PlayerPrefs.SetFloat(backgroundPref, backgroundFloat);
            PlayerPrefs.SetFloat(SFXPref, SFXFloat);


            XFloat = .3f;
            YFloat = .3f;
            XSlider.value = XFloat;
            YSlider.value = YFloat;
            PlayerPrefs.SetFloat(XPref, XFloat * 1000);
            PlayerPrefs.SetFloat(YPref, YFloat * 1000);

            PlayerPrefs.SetInt(firstStart, -1);
        }
        else
        {
            backgroundFloat = PlayerPrefs.GetFloat(backgroundPref);
            backgroundSlider.value = backgroundFloat; 
            SFXFloat = PlayerPrefs.GetFloat(SFXPref);
            SFXSlider.value = SFXFloat;


            XFloat = PlayerPrefs.GetFloat(XPref);
            XSlider.value = XFloat / 1000;
            YFloat = PlayerPrefs.GetFloat("YSensitivityPref");
            YSlider.value = YFloat / 1000;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(backgroundPref, backgroundSlider.value);
        PlayerPrefs.SetFloat(SFXPref, SFXSlider.value);
    }

    public void SaveSensitivity()
    {
        PlayerPrefs.SetFloat(XPref, XSlider.value * 1000);
        YFloat = YSlider.value;
        PlayerPrefs.SetFloat("YSensitivityPref", YFloat * 1000);
    }

    public void SaveSettings()
    {
        SaveSensitivity();
        SaveSoundSettings();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveSettings();
        }
    }

    public void UpdateSound()
    {
        backgroundAudio.volume = backgroundSlider.value;

        for (int i = 0; i < SFXAudio.Length; i++)
        {
            SFXAudio[i].volume = SFXSlider.value;
        }
    }
}
