using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public class GunPickup : MonoBehaviour 
{
    public GunStats gunStats; //[SerializeField] 

    // Start is called before the first frame update 
    void Start() 
    {
        if (!GameManager.instance.playerScript.getIsDropping())
        {
            gunStats.CurGunMag = gunStats.MaxGunMag;
            gunStats.CurGunCapacity = gunStats.MaxGunCapacity;
        }
    }

    private void OnTriggerEnter( Collider other ) 
    {
        if ( other.CompareTag( "Player" )) 
        {
            GameManager.instance.playerScript.getGunStats( gunStats ); 
            Destroy( gameObject ); 
        }
    }
}

