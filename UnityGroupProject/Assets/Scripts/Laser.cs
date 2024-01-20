using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] Renderer model;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform shootPos;
    [SerializeField] float shotSpeed;

    bool isShooting;

    //key toggle
    //interact toggle
    bool reqKey;
        // based on kills, collectable, interactable

    // bool
    //isToggleble
    //true - turret can be turned off -> goto isFiring
    //false - turret can not be turned off
    [SerializeField] bool isToggleble;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckReq();

        if (isToggleble) // checks if can fire 
        {
            if (!isShooting)
            {
                StartCoroutine(shoot());
            }
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(projectile, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shotSpeed);
        isShooting = false;
    }

    void CheckReq()
    {
        if (reqKey)
        {
            isToggleble = true;
        }
    }
}
