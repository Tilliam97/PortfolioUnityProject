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
    [SerializeField] float _returnTime;

    float _cTime;

    bool isDisabled;
    Color orig;
    Renderer _platColor;
    void Start()
    {
        SetPlatform(_tangeble);
        _platColor = _platformOn.GetComponent<Renderer>();
        orig = _platColor.material.color;
    }

    void Update()
    {
        if (_timedBased && !isDisabled)
        {
            StartCoroutine(DisappearingAct());
        }
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
        ColorTimerSet();
        yield return new WaitForSeconds(_cTime);
        _platColor.material.color = Color.yellow;
        yield return new WaitForSeconds(_cTime);
        
        SetPlatform(false);
        isDisabled = true;
        _platColor.material.color = orig;
        yield return new WaitForSeconds(_returnTime);

        SetPlatform(true);
        isDisabled = false;
    }

    void ColorTimerSet()
    {
        _cTime = _timeBeforeGhost / 2;
    }
}
