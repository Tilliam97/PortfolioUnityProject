using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public class GunPickup : MonoBehaviour 
{
    [SerializeField] GunStats gun; 

    // Start is called before the first frame update 
    void Start() 
    {
        gun.CurGunMag = gun.MaxGunMag; 
        gun.CurGunCapacity = gun.MaxGunCapacity; 
    }

    private void OnTriggerEnter( Collider other ) 
    {
        if ( other.CompareTag( "Player" )) 
        {
            GameManager.instance.playerScript.getGunStats( gun ); 
            Destroy( gameObject ); 
        }
    }
}

