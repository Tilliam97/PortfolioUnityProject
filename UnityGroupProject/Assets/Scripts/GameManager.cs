using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;  // there are two instances set to static cause there are 2 UI managers other manager is UIManager

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    public GameObject playerDamageFlash;

    public Image playerHPBar;
    public TMP_Text HPTxt;

    public GameObject gunPistol;
    public GameObject gunAR;
    public GameObject gunShotgun;
    public GameObject gunSniper;
    public TMP_Text AmmoTxt;

    public GameObject reloadPrompt;
    public GameObject outOfAmmoPrompt;

    [Header("Audio")]
    public AudioSource pistolShot;
    public AudioSource ARShot;
    public AudioSource shotgunShot;
    public AudioSource sniperShot;
    public AudioSource reloadSound;
    public AudioSource changeWeaponSound;
    public AudioSource damagedSound;
    public AudioSource deathSound;
    public AudioSource swingSound;
    public AudioSource damagedSoundRanged;
    public AudioSource deathSoundRanged;
    public AudioSource shootSoundRanged;


    public GameObject player;
    public PlayerController playerScript;
    public GameObject playerSpawnPos;

    public bool isPaused; // we'll make this a getter & setter later 
    int enemyCount;
    StopWatch stopWatch;

    public List<string> leaderboards;

    float currTime;
    string score;

    public bool _locationGoalReached;


    // Start is called before the first frame update 
    void Awake()
    {
        instance = this; // this creates our one instance so each class can talk to it 
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
        _locationGoalReached = false;
        currTime = 0;
    }

    // Update is called once per frame 
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && menuActive == null)   // if the escape button is pressed 
        {
            statePaused();                      // it'll toggle the bool 
            menuActive = menuPause;             // move the pause menu into the temp menu 
            menuActive.SetActive(isPaused);   // and it'll pause or unpause the game - closing or opening the menu 
        }

        setScore();
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
        menuActive.SetActive(false); // turns off the menu 
        menuActive = null;             // resets the temp menu 
    }

    public void updateGameGoal(int amount)
    {
        enemyCount += amount;

        // bring up the game over menu if no enemies are remaining  - updated to \|/
        // Player has reached end location
        if (_locationGoalReached) //enemyCount <= 0 changed to reaching location
        {
            statePaused();
            setScore();
            checkHighScore();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
    }

    public void checkHighScore()
    {
        LoadLeaderboard();

        if (score != null && score != "00:00:000")
        {
            leaderboards.Add(score);
        }
        leaderboards.Sort((x, y) =>
        {
            string[] xParts = x.Split(':');
            string[] yParts = y.Split(':');

            int xTotalMilliseconds = int.Parse(xParts[2]) + int.Parse(xParts[1]) * 1000 + int.Parse(xParts[0]) * 60 * 1000;
            int yTotalMilliseconds = int.Parse(yParts[2]) + int.Parse(yParts[1]) * 1000 + int.Parse(yParts[0]) * 60 * 1000;

            if (xTotalMilliseconds <= 0)
            {
                xTotalMilliseconds = 999999999;
            }
            else if (yTotalMilliseconds <= 0)
            {
                yTotalMilliseconds = 999999999;
            }

            return xTotalMilliseconds.CompareTo(yTotalMilliseconds);
        });

        if (leaderboards.Count == 7)
        {
            leaderboards.RemoveAt(6);
        }

        saveLeaderboard();

    }
    public void saveLeaderboard()
    {
       
        PlayerPrefs.SetInt("LeaderBoard_count", 6);

        for (int i = 0; i < 6; i++)
        {
            PlayerPrefs.SetString("myList_" + i, leaderboards[i]);

        }
    }

    public void LoadLeaderboard()
    {
        for (int i = 0; i < 6; i++)
        {
            string tempScore;

            if (leaderboards.Count == 0)
            {
                leaderboards = new List<string>();
                for (int j = 0; j < 6; j++)
                {
                    leaderboards.Add("00:00:000");
                }
            }

            tempScore = PlayerPrefs.GetString("myList_" + i, leaderboards[i]);
            leaderboards[i] = tempScore;
        }
    }

    public void setScore()
    {
        currTime = currTime + Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(currTime);
        score = time.ToString(@"mm\:ss\:fff");
    }

    public List<string> GetLeaderBoards()
    {
        return leaderboards;
    }

    public void youLose()
    {
        statePaused();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }
}

