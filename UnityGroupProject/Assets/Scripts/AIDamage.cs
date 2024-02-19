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
        enemy.updateEnemyUI();

        if (enemy.HP <= 0)
            Destroy(enemy.gameObject);
    }
}
