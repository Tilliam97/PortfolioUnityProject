using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] string collectType;  // key,  health pack

    [SerializeField] bool enemyCanDrop;   // can an enemy drop this?

    [SerializeField] int keyCode;         // code used
    [SerializeField] int grantHealth;     // amount to heal

    void OnTriggerEnter(Collider other)   // collects object and destroys
    {
        Collect();
        Destroy(gameObject);
    }

    void Collect()                        // what collecting does
    {
        // Key

        // Health


    }
}
