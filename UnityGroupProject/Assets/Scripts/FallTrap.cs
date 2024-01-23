using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrap : MonoBehaviour
{
    // will damage the player when fell into: Spikes, Lava, voids

    [SerializeField] int damageAmount;
    [SerializeField] bool tpPlayer;
    [SerializeField] bool damageOvertime;
    [SerializeField] float damageInterval;

    bool overtimedamage = false;
    private Coroutine damageCoroutine;
    

    void Start()
    {
        if (tpPlayer)
        {
            // turn off physical collider
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (damageOvertime)
        {
            overtimedamage = true;
            damageCoroutine = StartCoroutine(DamageOverTime(other));
        }
        else
            DamagePlayer(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if (overtimedamage)
        {
            StopCoroutine(damageCoroutine);
            overtimedamage = false;
        }
    }

    void DamagePlayer(Collider other)
    {
        if (other.isTrigger)
        {
            return; 
        }

        IDamageTeleport dmg = other.GetComponent<IDamageTeleport>();

        if (dmg != null)
            dmg.takeDamageTP(damageAmount, tpPlayer);
    }

    IEnumerator DamageOverTime(Collider other)
    {
        while (overtimedamage)
        {
            DamagePlayer(other);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
