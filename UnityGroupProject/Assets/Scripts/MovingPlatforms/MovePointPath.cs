using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePointPath : MonoBehaviour
{
    public int nextMoveIndex;

    public Transform GetMovePoint(int movepointIndex)
    {
        return transform.GetChild(movepointIndex);  // checks child object and returns its index in  hiearchy
    }

    public int GetNextMovePoint(int currMovePointIndex)
    {
        nextMoveIndex = currMovePointIndex + 1;  // moves to next index
        
        if (nextMoveIndex == transform.childCount)
        {
            nextMoveIndex = 0;                      // if index is totals the count of indexes then start from first index
        }

        return nextMoveIndex;
    }
}
