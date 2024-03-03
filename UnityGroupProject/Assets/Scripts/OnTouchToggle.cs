using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTouchToggle : MonoBehaviour, IToggle
{
    // this script was only created for moving platforms CURRENTLY 
    // can be adjusted to work for event systems or just simple open door

    [SerializeField] GameObject _obj;
    bool _active;

    void Awake()
    {
        _active = _obj.activeSelf;  // if using this script make sure to set this object in the inspector or the program will crash
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
            ToggleMe();
    }
    public void ToggleMe()
    {
        if (_obj.GetComponent<ToggleStation>()) // checks if it contains ToggleStation Script
        {
            _obj.GetComponent<ToggleStation>().takeDamage(1);
        }
        if (_obj.GetComponent<ToggleObject>())
        {
            //Debug.Log("Toggle Activated");
            _obj.GetComponent<ToggleObject>().ToggleMe();
        }
    }
}
