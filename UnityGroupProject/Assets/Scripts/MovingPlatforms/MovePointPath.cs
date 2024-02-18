using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePointPath : MonoBehaviour
{
    public Transform GetMovePoint(int movepointIndex)
    {
        return transform.GetChild(movepointIndex);
    }

    public int GetNextMovePoint(int currMovePointIndex)
    {
        int nextMoveIndex = currMovePointIndex + 1;
        
        if (nextMoveIndex == transform.childCount)
        {
            nextMoveIndex = 0;
        }

        return nextMoveIndex;
    }
}
