using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePlatform : MonoBehaviour, IToggle
{
    [SerializeField] GameObject _platformOn;  // set platform that will represent on
    [SerializeField] GameObject _platformOff; //  set platform that will represent off

    [SerializeField] bool _tangeble;
    [SerializeField] bool _contactBased;
    [SerializeField] bool _timedBased;

    [SerializeField] float _timeBeforeGhost;

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

    private void OnTriggerEnter(Collider other)
    {
        if (_contactBased)
        {
            StartCoroutine(DisappearingAct());
        }
    }

    IEnumerator DisappearingAct()
    {
        Color orig = _platformOn.GetComponent<MeshRenderer>().material.color;
        _platformOn.GetComponent<MeshRenderer>().material.color = Color.yellow;
        yield return new WaitForSeconds(_timeBeforeGhost);
        SetPlatform(false);
        yield return new WaitForSeconds(5f);
        SetPlatform(true);
        _platformOn.GetComponent<MeshRenderer>().material.color = orig;
    }
}
