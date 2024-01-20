using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Laser laser;
    List<Laser> numLaser = new List<Laser>();

    void Start()
    {
        for (int i = 0; i < areToggles.Count; i++)
        {
            numLaser.Add(areToggles[i].GetComponent<Laser>());
        }
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

        interact += amount;
        if (interact > 1)
            interact = 0;

        Toggle();
    }

    void Toggle()
    {
        if (interact == 1)
        {
            // on
            model.material.color = Color.green;
            // change toggel of game object script is on to on
            for (int i = 0; i < numLaser.Count; i++)
            {
                numLaser[i].isToggel = true;
            }
        }
        else
        {
            // off
            model.material.color = Color.red;
            // change toggel of game object script is on to off
            for (int i = 0; i < numLaser.Count; i++)
            {
                numLaser[i].isToggel = false;
            }
        }
    }
    //public void TakeKey(int amount)  // use if key is added
    //{
    //    if(codeKey == amount)
    //        reqKey = false;
    //}

}
