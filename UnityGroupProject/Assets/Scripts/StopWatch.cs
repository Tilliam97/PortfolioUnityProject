using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StopWatch : MonoBehaviour
{
    bool stopWatchActive = false;
    float currTime;
    public TMP_Text currTimeText;

    // Start is called before the first frame update
    void Start()
    {
        currTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopWatchActive)
        {
            currTime = currTime + Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currTime);
        currTimeText.text = time.ToString(@"mm\:ss\:fff");
    }

    public string getTimeText()
    {
        return currTimeText.text;
    }

    public void StartTimer()
    {
        Debug.Log("Start");
        stopWatchActive = true;
    }

    public void StopTimer()
    {
        Debug.Log("Stop");
        stopWatchActive = false;
    }
}

