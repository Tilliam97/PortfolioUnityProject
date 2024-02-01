using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHeal 
{
    void HealMe(int amount); // use for other sources of healing other than collectables.
}
