using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{

    [SerializeField] Rigidbody rb;

    [SerializeField] int damageAmount;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) //We want triggers to ignore other triggers
        {
            return;
        }

        IDamage dmg = other.GetComponent<IDamage>(); //If it has IDamage on it

        if (dmg != null)
        {
            dmg.takeDamage(damageAmount);
        }

        Destroy(gameObject);

    }
}
