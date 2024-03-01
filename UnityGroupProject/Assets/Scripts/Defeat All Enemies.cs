using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public class DefeatAllEnemies : MonoBehaviour, IToggle 
{
    [SerializeField] GameObject EnemyBarrier; 

    void Update() 
    {
        if ( GameManager.instance.GetEnemyCount() <= 0 ) 
        {
            ToggleMe(); 
        }
    }

    public void ToggleMe() 
    {
        EnemyBarrier.SetActive( false ); 
    }
}

