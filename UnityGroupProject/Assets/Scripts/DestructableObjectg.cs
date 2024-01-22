using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObjectg : MonoBehaviour, IDamage
{
    [SerializeField] int HP;
    [SerializeField] Renderer model;

    // add chance for dropping item
        // chance drops or garantee drops

    public void takeDamage(int amount)
    {
        HP -= amount;

        StartCoroutine(flashRed()); // for testing if took damage - change to breaking animation or image swapping if possible

        if (HP <= 0)
        {
            Destroy(gameObject); // play destruction animation if added
        }
    }
    
    IEnumerator flashRed()
    {
        Color color = model.material.color;
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = color;
    }
}
