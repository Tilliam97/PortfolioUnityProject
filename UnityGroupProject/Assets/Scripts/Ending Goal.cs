using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance._locationGoalReached = true;
        GameManager.instance.updateGameGoal(0);
    }
}
