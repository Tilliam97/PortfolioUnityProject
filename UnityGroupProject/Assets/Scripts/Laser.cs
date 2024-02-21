using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Laser : MonoBehaviour, IToggle
{
    [SerializeField] Renderer model;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform shootPos;
    [SerializeField] float shotSpeed;

    // Line Renderer
    [SerializeField] LineRenderer laserLine;
    // Damage amount
    [SerializeField] int damageAmt;

    // bool isShooting;

    //key toggle
    bool reqKey;
        // based on kills, collectable, interactable

    [SerializeField] public bool isToggel;

    void Update()
    {
        CheckReq();


        
        if (isToggel) // checks if can fire 
        {
            // may need adjusting when changing how shooting works
            //if (!isShooting)
            //{
            //    //StartCoroutine(shoot());
            //}
            StartCoroutine(LaserOn());
        }
        else
        {
            StopCoroutine(LaserOn());
            laserLine.enabled = false;
        }
    }


    // change to laser On instead of shoot
    IEnumerator shoot()
    {
        //isShooting = true;
        Instantiate(projectile, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shotSpeed);
        //isShooting = false;
    }

    // raycasts infront of itself.
    // on hit deal damage through IDamage and pass in the amount of damage
    // make new method to do above ^^^
    IEnumerator LaserOn()
    {
        laserLine.enabled = true;  // turns on line renderer
        RaycastHit hit;
        Vector3 forward = shootPos.transform.TransformDirection(Vector3.forward);
        
        yield return new WaitForSeconds(0.1f); // .1 is the min befor it breaks player restart cause it goes negative very quickly
        
        if (Physics.Raycast(shootPos.position, forward, out hit, 100.0f))
        {
            //Debug.DrawLine(shootPos.position, hit.point, Color.red);
            laserLine.SetPosition(1, hit.point - shootPos.position + Vector3.forward);
            // deal player damage when player enters path
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (hit.transform != transform && dmg != null)
            {
                dmg.takeDamage(damageAmt);  // this does damage on each individual raycast hit causing rapid hits  even if wait for seconds is every .1 seconds
            }
            
        }
        
    }

   

    void CheckReq() // will need updating when code is required
    {
        if (reqKey)
        {
            isToggel = true;
        }
    }

    public void ToggleMe()  // must be public so toggle station can properly read
    {
        isToggel = !isToggel;
    } 
}
