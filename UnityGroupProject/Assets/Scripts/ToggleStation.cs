using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleStation : MonoBehaviour, IDamage
{
    // bool
    // checks if toggle is already on
    [SerializeField] bool isOn;
    [SerializeField] bool reqKey;    // requires user to have key to interact with
    [SerializeField] int codeKey;
    [SerializeField] Renderer model;
    [SerializeField] GameObject toggel;
    int interact;

    Laser laser;

    // Start is called before the first frame update

    void Start()
    {
        if (isOn)
        {
            interact = 1;
            model.material.color = Color.green;
        }
        else
        {
            interact = 0;
            model.material.color = Color.red;
        }

        laser = toggel.GetComponent<Laser>();
    }
    public void takeDamage(int amount)
    {
        // checks if key is required  this is if there is a key
        if (reqKey)
        {
            return;
        }

        interact += amount;
        if (interact > 1)
            interact = 0;

        if (interact == 1)
        {
            // on
            model.material.color = Color.green;
            // change toggel of game object script is on to on
            laser.isToggel = true;
        }
        else
        {
            // off
            model.material.color = Color.red;
            // change toggel of game object script is on to off
            laser.isToggel = false;
        }

    }
    //public void TakeKey(int amount)  // use if key is added
    //{
    //    if(codeKey == amount)
    //        reqKey = false;
    //}
}
