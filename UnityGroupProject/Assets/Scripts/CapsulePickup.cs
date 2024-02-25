using System.Collections; 
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine; 

public class CapsulePickup : MonoBehaviour 
{
    [SerializeField] CapsuleCollectible capsule; 

    // Start is called before the first frame update 
    void Start() 
    {
        
    }

    private void OnTriggerEnter( Collider other ) 
    {
        if ( other.CompareTag( "Player" )) 
        {
            //int curMag = GameManager.instance.playerScript.GetCurMag(); 
            //int maxMag = GameManager.instance.playerScript.GetMaxMag(); 
            //int curAmmo = GameManager.instance.playerScript.GetCurAmmo(); 
            //int maxAmmo = GameManager.instance.playerScript.GetMaxAmmo();

            string gunType = GameManager.instance.playerScript.GetSelectedGunName(); 


            switch ( capsule.capsuleType ) 
            {
                case CapsuleType.HEALTH:

                    int curHP = GameManager.instance.playerScript.GetPlayerHP(); 
                    int maxHP = GameManager.instance.playerScript.GetPlayerHPOrig(); 

                    if ( curHP < maxHP ) 
                    {
                        GameManager.instance.playerScript.HealMe( capsule.refillAmount ); 
                        Destroy( gameObject ); 
                    }
                    break; 
                case CapsuleType.A_PISTOL: 
                    if ( gunType == "AR" ) 
                    {
                        GameManager.instance.playerScript.RefillAmmo( AmmoTypes.PISTOL, capsule.refillAmount ); 
                        Destroy( gameObject ); 
                    }
                    break; 
                case CapsuleType.A_SHOTGUN:
                    if ( gunType == "Shotgun" ) 
                    {
                        GameManager.instance.playerScript.RefillAmmo( AmmoTypes.SHOTGUN, capsule.refillAmount ); 
                        Destroy( gameObject ); 
                    }
                    break; 
                case CapsuleType.A_SNIPER: 
                    if ( gunType == "Sniper" ) 
                    {
                        GameManager.instance.playerScript.RefillAmmo( AmmoTypes.SNIPER, capsule.refillAmount ); 
                        Destroy( gameObject ); 
                    }
                    break; 
                case CapsuleType.A_LASER_GUN: 
                    if ( gunType == "Laser Gun" ) 
                    {
                        GameManager.instance.playerScript.RefillAmmo( AmmoTypes.LASER, capsule.refillAmount ); 
                        Destroy( gameObject ); 
                    }
                    break; 
                default:
                    break; 
            }
        }
    }
}

