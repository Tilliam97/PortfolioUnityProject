using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePlatform : MonoBehaviour, IToggle
{
    
    [SerializeField] bool _tangeble;
    [SerializeField] bool _contactBased;
    [SerializeField] bool _timedBased;

    [SerializeField] GameObject _platformOn;  // set platform that will represent on
    [SerializeField] GameObject _platformOff; //  set platform that will represent off

    void Start()
    {
        SetPlatform(_tangeble);
    }
    public void ToggleMe()
    {
        _tangeble = !_tangeble;
        SetPlatform(_tangeble);
    }

    void SetPlatform(bool tangeble)
    {
        if (tangeble)
        {
            _platformOn.SetActive(true);
            _platformOff.SetActive(false);
        }
        else
        {
            _platformOn.SetActive(false);
            _platformOff.SetActive(true);
        }
    }
}
