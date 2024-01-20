using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrap : MonoBehaviour
{
    // will damage the player when fell into: Spikes, Lava, voids

    [SerializeField] Rigidbody rb;
    [SerializeField] int damageAmount;
    [SerializeField] bool tpPlayer;
    [SerializeField] bool damageOvertime;
    [SerializeField] float damageInterval;

    bool overtimedamage = false;
    private Coroutine damageCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (damageOvertime)
        {
            overtimedamage = true;
            damageCoroutine = StartCoroutine(DamageOverTime(other));
        }
        else
            DamagePlayer(other);

        if (tpPlayer) // use for jumps that are impossible to make it back to level
        {
            // teleport payer if set true to teleport player
            // teleport player to safest location
        }
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

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null)
            dmg.takeDamage(damageAmount);
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
