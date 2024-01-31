using System.Collections; 
using System.Collections.Generic; 
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
            switch ( capsule.capsuleType ) 
            {
                case CapsuleType.HEALTH: 
                    GameManager.instance.playerScript.HealMe( capsule.refillAmount ); 
                    break;
                case CapsuleType.A_PISTOL: 
                case CapsuleType.A_SHOTGUN: 
                case CapsuleType.A_SNIPER: 
                    GameManager.instance.playerScript.FillAmmo( capsule.refillAmount ); 
                    break; 
                default:
                    break; 
            }
            Destroy( gameObject ); 
        }
    }
}

