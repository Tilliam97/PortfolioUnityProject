using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeWeapon : MonoBehaviour
{
    [SerializeField] GameObject weapon;
    [SerializeField] int dmgAmt;

    // Start is called before the first frame update
    /*void Start()
    {
       
    }*/

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        if(other.CompareTag("Player"))
        {
            IDamage dmg =  other.GetComponent<IDamage>();
            
            if (dmg != null)
            {
                dmg.takeDamage(dmgAmt);
            }
        }

        
    }
}
