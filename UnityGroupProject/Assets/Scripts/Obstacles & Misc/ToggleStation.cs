using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class ToggleStation : MonoBehaviour, IDamage
{
    // bool
    // checks if toggle is already on
    [SerializeField] bool isOn;
    [SerializeField] bool reqKey;    // requires user to have key to interact with
    [SerializeField] int codeKey;
    [SerializeField] Renderer model;
    [SerializeField] List<GameObject> areToggles;

    int interact;
    //List<Laser> numLaser = new List<Laser>(); // change to take in any object

    void Start()
    {
        //for (int i = 0; i < areToggles.Count; i++)
        //{
        //    numLaser.Add(areToggles[i].GetComponent<Laser>());
        //}
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
        Toggle();
        
    }
    public void takeDamage(int amount)
    {
        // checks if key is required  this is if there is a key
        if (reqKey)
        {
            return;
        }

        interact++;
        if (interact > 1) // this will cause problems if base player damage is greater than 1
            interact = 0;

        Toggle();
    }
    
    void Toggle()
    {
        // toggling desired objs.
        // change toggel of game object script is on to on
        for (int i = 0; i < areToggles.Count; i++)
        {
            //numLaser[i].isToggel = true;
            IToggle toggleMe = areToggles[i].GetComponentInChildren<IToggle>();
            toggleMe.ToggleMe();
        }

        // color change for button
        if (interact == 1)
        {
            // on
            model.material.color = Color.green;
        }
        else
        {
            // off
            model.material.color = Color.red;
        }
    }

}
