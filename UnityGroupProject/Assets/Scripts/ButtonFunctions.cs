using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void resume()
    {
        GameManager.instance.stateUnpaused();
    }

    public void restart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.stateUnpaused();
    }

    public void quit() 
    { 
        Application.Quit();
    }

    public void respawnPayer()
    {
        GameManager.instance.playerScript.respawn();
        GameManager.instance.stateUnpaused();
    }
}
