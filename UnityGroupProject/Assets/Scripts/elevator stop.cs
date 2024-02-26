using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevatorstop : MonoBehaviour
{

    [SerializeField] MovingPlatform elevator;

    bool _activated;
    private void OnTriggerEnter(Collider other)
    {
        if (!_activated)
        {
            StartCoroutine(TimedStopped());
            _activated = true;
        }
    }

    IEnumerator TimedStopped()
    {

        yield return new WaitForSeconds(.5f);
        elevator.ToggleMe();
    }
}
