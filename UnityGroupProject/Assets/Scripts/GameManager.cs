using System.Collections; 
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour 
{
    public static GameManager instance;

    [SerializeField] GameObject menuActive; 
    [SerializeField] GameObject menuPause; 
    [SerializeField] GameObject menuWin; 
    [SerializeField] GameObject menuLose;
    public GameObject playerDamageFlash; 
    
    public Image playerHPBar;
    public Image playerAmmoBar;
    public TMP_Text HPTxt;
    public TMP_Text AmmoTxt;

    public GameObject reloadPrompt;
    public GameObject outOfAmmoPrompt;
    

    public GameObject player;
    public PlayerController playerScript;
    public GameObject playerSpawnPos; 

    public bool isPaused; // we'll make this a getter & setter later 
    int enemyCount; 



    // Start is called before the first frame update 
    void Awake() 
    {
        instance = this; // this creates our one instance so each class can talk to it 
        player = GameObject.FindGameObjectWithTag( "Player" ); 
        playerScript = player.GetComponent<PlayerController>();
        playerSpawnPos = GameObject.FindWithTag( "Player Spawn Pos" ); 
    }
    
    // Update is called once per frame 
    void Update() 
    {
        if ( Input.GetButtonDown( "Cancel" ) && menuActive == null )   // if the escape button is pressed 
        {
            statePaused();                      // it'll toggle the bool 
            menuActive = menuPause;             // move the pause menu into the temp menu 
            menuActive.SetActive( isPaused );   // and it'll pause or unpause the game - closing or opening the menu 
        }
    }

    public void statePaused() 
    {
        isPaused = !isPaused; // we extracted this toggle into it's on method 
        Time.timeScale = 0; 
        Cursor.visible = true; 
        Cursor.lockState = CursorLockMode.Confined; 
    }

    public void stateUnpaused() 
    {
        isPaused = !isPaused; 
        Time.timeScale = 1; 
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive( false ); // turns off the menu 
        menuActive = null;             // resets the temp menu 
    }

    public void updateGameGoal( int amount ) 
    {
        enemyCount += amount; 

        // bring up the game over menu if no enemies are remaining 
        if ( enemyCount <= 0 ) 
        {
            statePaused(); 
            menuActive = menuWin; 
            menuActive.SetActive( true ); 
        }
    }

    public void youLose() 
    {
        statePaused(); 
        menuActive = menuLose; 
        menuActive.SetActive( true );
    }
}

