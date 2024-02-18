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
    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        if (operation.progress >= 0.9f)
        {
            operation.allowSceneActivation = true;
        }
        yield return null;
    }

    public void LoadFirstScene(string scene)
    {
        if (scene != "")
        {
            StartCoroutine(LoadAsynchronously(scene));
        }
    }
    public void respawnPayer()
    {
        GameManager.instance.playerScript.respawn();
        GameManager.instance.stateUnpaused();
    }
}
