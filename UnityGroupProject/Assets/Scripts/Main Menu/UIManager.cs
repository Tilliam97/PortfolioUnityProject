using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance; // there are two instances set to static cause there are 2 UI managers other manager is GameManager
    private Animator CameraObject;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject firstMenu;
    public GameObject playMenu;
    public GameObject exitMenu;
    public GameObject leaderboardsMenu;
    public GameObject creditsMenu;


    [Header("PANELS")]
    public GameObject mainCanvas;
    public GameObject PanelControls;
    public GameObject PanelVideo;
    public GameObject PanelGame;
    public GameObject PanelKeyBindings;
    public GameObject PanelMovement;
    public GameObject PanelCombat;
    public GameObject PanelGeneral;

    [Header("SETTINGS SCREEN")]
    public GameObject lineGame;
    public GameObject lineVideo;
    public GameObject lineControls;
    public GameObject lineKeyBindings;
    public GameObject lineMovement;
    public GameObject lineCombat;
    public GameObject lineGeneral;

    [Header("LOADING SCREEN")]
    public bool waitForInput = false;
    public GameObject loadingMenu;
    public Slider loadingBar;
    public TMP_Text loadPromptText;
    public KeyCode userPromptKey;

    [Header("SFX")]
    public AudioSource hoverSound;
    public AudioSource sliderSound;
    public AudioSource swooshSound;

    [Header("Leaderboards")]
    [SerializeField] List<TMP_Text> leaderBoard;
    List<string> leaderboardStrings;

    // Start is called before the first frame update
    void Start()
    {
        CameraObject = transform.GetComponent<Animator>();

        playMenu.SetActive(false);
        exitMenu.SetActive(false);
        leaderboardsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        firstMenu.SetActive(true);
        mainMenu.SetActive(true);

        

        LoadLeaderboard();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play()
    {
        exitMenu.SetActive(false);
        leaderboardsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        playMenu.SetActive(true);
    }

    public void ReturnMenu()
    {
        playMenu.SetActive(false);
        leaderboardsMenu.SetActive(false);
        exitMenu.SetActive(false);
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void LoadFirstScene(string scene)
    {
        if (scene != "")
        {
            StartCoroutine(LoadAsynchronously(scene));
        }
    }

    public void DisablePlayMenu()
    {
        playMenu.SetActive(false);
    }

    public void MoveToSettings()
    {
        DisablePlayMenu();
        CameraObject.SetFloat("Animate", 1);
    }
    public void MoveToMain()
    {
        CameraObject.SetFloat("Animate", 0);
    }
    void DisablePanels()
    {
        #region Disable on different settings menus
        PanelControls.SetActive(false);
        PanelVideo.SetActive(false);
        PanelGame.SetActive(false);
        PanelKeyBindings.SetActive(false);

        lineGame.SetActive(false);
        lineControls.SetActive(false);
        lineVideo.SetActive(false);
        lineKeyBindings.SetActive(false);

        PanelMovement.SetActive(false);
        lineMovement.SetActive(false);
        PanelCombat.SetActive(false);
        lineCombat.SetActive(false);
        PanelGeneral.SetActive(false);
        lineGeneral.SetActive(false);
        #endregion
    }


    public void GamePanel()
    {
        DisablePanels();
        PanelGame.SetActive(true);
        lineGame.SetActive(true);
    }

    public void VideoPanel()
    {
        DisablePanels();
        PanelVideo.SetActive(true);
        lineVideo.SetActive(true);
    }

    public void ControlsPanel()
    {
        DisablePanels();
        PanelControls.SetActive(true);
        lineControls.SetActive(true);
    }

    public void KeyBindingsPanel()
    {
        DisablePanels();
        MovementPanel();
        PanelKeyBindings.SetActive(true);
        lineKeyBindings.SetActive(true);
    }
    public void MovementPanel()
    {
        DisablePanels();
        PanelKeyBindings.SetActive(true);
        PanelMovement.SetActive(true);
        lineMovement.SetActive(true);
    }

    public void CombatPanel()
    {
        DisablePanels();
        PanelKeyBindings.SetActive(true);
        PanelCombat.SetActive(true);
        lineCombat.SetActive(true);
    }

    public void GeneralPanel()
    {
        DisablePanels();
        PanelKeyBindings.SetActive(true);
        PanelGeneral.SetActive(true);
        lineGeneral.SetActive(true);
    }

    public void PlayHover()
    {
        hoverSound.Play();
    }

    public void PlaySFXHover()
    {
        sliderSound.Play();
    }

    public void PlaySwoosh()
    {
        swooshSound.Play();
    }

    public void AreYouSure()
    {
        exitMenu.SetActive(true);
        leaderboardsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        DisablePlayMenu();
    }

    public void LeaderboardMenu()
    {
        playMenu.SetActive(false);
        leaderboardsMenu.SetActive(true);
        creditsMenu.SetActive(false);
        exitMenu.SetActive(false);
    }

    public void CreditsMenu()
    {
        playMenu.SetActive(false);
        leaderboardsMenu.SetActive(false);
        creditsMenu.SetActive(true);
        exitMenu.SetActive(false);
    }

    public void saveLeaderboard()
    {
        PlayerPrefs.SetInt("LeaderBoard_count", leaderBoard.Count);

        for (int i = 0; i < leaderboardStrings.Count; i++)
        {

            PlayerPrefs.SetString("myList_" + i, leaderboardStrings[i]);

        }
    }

    public void LoadLeaderboard()
    {
        for (int i = 0; i < leaderBoard.Count; i++)
        {
            string tempScore; 

            if (leaderboardStrings == null)
            {
                leaderboardStrings = new List<string>();
                for (int j = 0; j < leaderBoard.Count; j++)
                {
                    leaderboardStrings.Add(leaderBoard[j].text);
                }
            }
            tempScore = PlayerPrefs.GetString("myList_" + i, leaderboardStrings[i]);
            leaderboardStrings[i] = tempScore;
        }
    }

    public void UpdateLeaderBoard()
    {
        LoadLeaderboard();

        if (leaderboardStrings == null)
        {
            leaderboardStrings = new List<string>();
            for (int i = 0; i < leaderBoard.Count; i++)
            {
                leaderboardStrings.Add(leaderBoard[i].text);
            }
        }

         saveLeaderboard();


        for (int i = 0; i < leaderBoard.Count; i++)
        {

            leaderBoard[i].text = leaderboardStrings[i];
        }
    }


    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
    }


    IEnumerator LoadAsynchronously(string sceneName)
    { // scene name is just the name of the current scene being loaded
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        mainCanvas.SetActive(false);
        loadingMenu.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .95f);
            loadingBar.value = progress;

            if (operation.progress >= 0.9f && waitForInput)
            {
                loadPromptText.text = "Press " + userPromptKey.ToString().ToUpper() + " to continue";
                loadingBar.value = 1;

                if (Input.GetKeyDown(userPromptKey))
                {
                    operation.allowSceneActivation = true;
                }
            }
            else if (operation.progress >= 0.9f && !waitForInput)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

}
