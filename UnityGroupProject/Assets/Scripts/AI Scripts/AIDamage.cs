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

    /*IEnumerator flashRed()
    {
        Color origColor;

        //Store the orig color 
        switch (damageType)
        {
            case collisionType.head:
                                    origColor = enemy.headRenderer.material.color;
                                    //enemy.headRenderer.material.renderQueue = 6000;
                                    enemy.headRenderer.material.color = Color.red;
                                    yield return new WaitForSeconds(0.1f);
                                    enemy.headRenderer.material.color = origColor;
                                    //enemy.headRenderer.material.renderQueue = 2000;
                break;
            case collisionType.body:
                                    origColor = enemy.bodyRenderer.material.color;
                                    //enemy.bodyRenderer.material.renderQueue = 6000;
                                    enemy.bodyRenderer.material.color = Color.red;
                                    yield return new WaitForSeconds(0.1f);
                                    enemy.bodyRenderer.material.color = origColor;
                                    //enemy.bodyRenderer.material.renderQueue = 2000;
                break;


        }*/

    IEnumerator flashRed()
    {
        Color origColor = enemy.model.material.color;

        enemy.model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        enemy.model.material.color = origColor;
    }

}
