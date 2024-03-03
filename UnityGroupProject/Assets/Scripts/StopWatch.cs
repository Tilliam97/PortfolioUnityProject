using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StopWatch : MonoBehaviour
{
    float currTime;
    public TMP_Text currTimeText;

    private static readonly string ScorePref = "ScorePref";


    // Start is called before the first frame update
    void Start()
    {
        currTime = PlayerPrefs.GetFloat(ScorePref);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetFloat("isActive") == 1)
        {
            currTime = currTime + Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currTime);
        currTimeText.text = time.ToString(@"mm\:ss\:fff");
    }


    public void StartTimer()
    {
        //Debug.Log("Start");
        PlayerPrefs.SetFloat("isActive", 1);
    }

    public void StopTimer()
    {
        //Debug.Log("Stop");
        PlayerPrefs.SetFloat("isActive", 0);
    }
}

