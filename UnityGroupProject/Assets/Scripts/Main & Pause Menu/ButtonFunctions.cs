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
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
			Application.Quit();
        #endif
    }


    public void LoadScene()
    {
        SceneManager.LoadScene(0);
        GameManager.instance.stateUnpaused();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void respawnPayer()
    {
        GameManager.instance.playerScript.respawn();
        GameManager.instance.stateUnpaused();
    }

    public void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int next = currentSceneIndex + 1;
        //Debug.Log(SceneManager.sceneCountInBuildSettings);
        if (next >= SceneManager.sceneCountInBuildSettings)
        {
            next = 0;
        }
        SceneManager.LoadSceneAsync(next);
        GameManager.instance.stateUnpaused();
    }
}
