using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// scraping for CapsulePickup script.  does the same thing
// use this if creating a 1 time object that doesnt need a new scriptable abject

public enum CollectibleTypes 
{
    HEALTH = 1, 
    PISTOL, 
    SNIPER, 
    SHOTGUN, 
    LASER_GUN 
}

public class Collectible : MonoBehaviour
{
    [SerializeField] string collectType;  // key,  health pack

    [SerializeField] bool enemyCanDrop;   // can an enemy drop this?  // not implemented

    [SerializeField] int keyCode;         // code used                // not implemented
    [SerializeField] int grantHealth;     // amount to heal

    Collider objColid;

    void OnTriggerEnter(Collider other)   // collects object and destroys
    {
        if (other.isTrigger)              // ignoring other triggers
            return;
        objColid = other;
        Collect();
        //Destroy(gameObject);
    }

    void Collect()                        // what collecting does
    {
        // Check collection type
        NameCheck(collectType);
        // Key reference code here

        // Health 


    }
    void NameCheck(string name)
    {
        switch (name)
        {
            case "Key":
                KeyCollect();
                break;

            case "Health":
                HealthCollect();
                break;
                
            case null:    // add if needed
                break;
        }
    }

    void KeyCollect()
    {
        // gives player a key code to be used for example a door
        // make interface for keycode - IKey
    }

    void HealthCollect()
    {
        // grants health to the player
        // make interface to give player Health - IHeal
        IHeal heal = objColid.GetComponent<IHeal>();
        heal.HealMe(grantHealth);
    }
}