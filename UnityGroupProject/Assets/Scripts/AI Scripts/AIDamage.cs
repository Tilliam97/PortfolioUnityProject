using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIDamage : MonoBehaviour, IDamage
{
    public enum collisionType { head, body, arms }
    public collisionType damageType;

    public SimMeleeEnemyAI melee;
    public RangedEnemy ranged;


    public void takeDamage(int amount)
    {
        if (melee != null && ranged == null)
        {
            melee.takeDamage(amount);
            StartCoroutine(blinkRed());
        }
        else
        {
            ranged.takeDamage(amount);
            StartCoroutine(flashRed());
        }
    }

     IEnumerator blinkRed()
     {
         Color origColor = melee.GetBodyRenderer().material.color;

         melee.GetBodyRenderer().material.color = Color.red;
         yield return new WaitForSeconds(0.1f);
         melee.GetBodyRenderer().material.color = origColor;
     }

    IEnumerator flashRed()
    {
        Color origColor;

        //Store the orig color 
        switch (damageType)
        {
            case AIDamage.collisionType.head:
                origColor = ranged.GetHeadRenderer().material.color;
                ranged.GetHeadRenderer().material.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                ranged.GetHeadRenderer().material.color = origColor;
                break;
            case AIDamage.collisionType.body:
                origColor = ranged.GetBodyRenderer().material.color;
                ranged.GetBodyRenderer().material.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                ranged.GetBodyRenderer().material.color = origColor;
                break;
            case AIDamage.collisionType.arms:
                origColor = ranged.GetArmsRenderer().material.color;
                ranged.GetArmsRenderer().material.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                ranged.GetArmsRenderer().material.color = origColor;
                break;
        }
    }

}
