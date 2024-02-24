using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    //void takeDamage(int amount);
    void updateEnemyUI();
    Renderer GetHeadRenderer();
    Renderer GetBodyRenderer();
    Renderer GetArmsRenderer();


}
