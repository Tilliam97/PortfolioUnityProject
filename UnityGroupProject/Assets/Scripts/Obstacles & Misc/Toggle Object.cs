using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObject : MonoBehaviour, IToggle
{
    [SerializeField] GameObject _obj;
    [SerializeField] bool _enabled;

    void Start()
    {
        //_obj = GetComponent<GameObject>().gameObject;
    }
    public void ToggleMe()
    {
        _enabled = !_enabled;
        _obj.SetActive(_enabled);
    }
}
