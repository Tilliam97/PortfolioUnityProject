using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))] 

public class LaserGunRaycast : MonoBehaviour
{
    [SerializeField] public Camera playerCamera; 
    [SerializeField] public Transform laserOrigin; 
    [SerializeField] public float gunRange = 50f; 
    [SerializeField] public float fireRate = 0.2f; 
    [SerializeField] public float laserDuration = 0f; //0.5f

    LineRenderer laserLine; 
    float fireTimer; 

    void Awake() 
    {
        laserLine = GetComponent<LineRenderer>(); 
    }

    private void Update() 
    {
        //fireTimer += Time.deltaTime; 
        /*
        if ( Input.GetButton( "Shoot" )) //Down // && fireTimer > fireRate 
        {
            //laserLine.enabled = true; 
            //fireTimer = 0; 
            laserLine.SetPosition( 0, laserOrigin.position ); 
            Vector3 rayOrigin = playerCamera.ViewportToWorldPoint( new Vector3( 0.5f, 0.5f, 0 ));
            RaycastHit hit; 
            if ( Physics.Raycast( rayOrigin, playerCamera.transform.forward, out hit, gunRange )) 
            {
                laserLine.SetPosition( 1, hit.point );
                //Destroy( hit.transform.gameObject ); 
            }
            else 
            {
                laserLine.SetPosition( 1, rayOrigin+( playerCamera.transform.forward * gunRange )); 
            }
            //laserLine.enabled = false; 
            //StartCoroutine( ShootLaser() ); 
        }*/


        /*
        IEnumerator ShootLaser() 
        {
            laserLine.enabled = true; 
            yield return new WaitForSeconds( laserDuration ); 
            laserLine.enabled = false; 
        }*/
        


    }
}

