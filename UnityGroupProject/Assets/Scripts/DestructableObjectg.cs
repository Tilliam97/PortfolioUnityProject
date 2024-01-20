using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObjectg : MonoBehaviour, IDamage
{
    [SerializeField] int HP;
    [SerializeField] Renderer model;

    public void takeDamage(int amount)
    {
        HP -= amount;

        StartCoroutine(flashRed());

        if (HP <= 0)
        {
            Destroy(gameObject);
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
