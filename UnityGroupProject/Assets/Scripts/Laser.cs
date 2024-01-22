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
    bool reqKey;
        // based on kills, collectable, interactable

    [SerializeField] public bool isToggel;

    void Update()
    {
        CheckReq();

        if (isToggel) // checks if can fire 
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

    void CheckReq() // will need updating when code is required
    {
        if (reqKey)
        {
            isToggel = true;
        }
    }

    public void Toggel()  // must be public so toggle station can properly read
    {
        isToggel = !isToggel;
    } 
}
