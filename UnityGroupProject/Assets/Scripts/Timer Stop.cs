using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStop : MonoBehaviour
{

    [SerializeField] StopWatch Watch;

    public bool _stop;
    private void OnTriggerEnter(Collider other)
    {
        if (_stop)
        {
            Watch.StopTimer();
        }
        else
        {
            Watch.StartTimer();
        }
    }
}
