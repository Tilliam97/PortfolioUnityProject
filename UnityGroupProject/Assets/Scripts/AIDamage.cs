using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIDamage : MonoBehaviour, IDamage
{
    public enum collisionType { head, body, arms }
    public collisionType damageType;

     public SimMeleeEnemyAI enemy;

    //public SimMeleeEnemyAI aiController;
    public void takeDamage(int amount)
    {
        enemy.takeDamage(amount);
        StartCoroutine(flashRed());
        enemy.updateEnemyUI();
    }

    IEnumerator flashRed()
    {
        //Store the orig color 
        Color origColor = enemy.model.material.color;

        enemy.model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        enemy.model.material.color = origColor;
    }
}
