using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public class PlayerController : MonoBehaviour, IDamage 
{
    [SerializeField] CharacterController controller;

    [SerializeField] int HP; 
    [SerializeField] float playerSpeed; 
    
    public KeyCode sprintKey = KeyCode.LeftShift; 
    float playerSpeedOrig; 
    [SerializeField] float sprintSpeed; 
    bool isSprinting;

    public KeyCode jumpKey = KeyCode.Space; 
    [SerializeField] int jumpMax; 
    [SerializeField] float jumpHeight; 
    [SerializeField] float gravity;
    

    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;


    Vector3 move; 
    Vector3 playerVel; 
    bool groundedPlayer;
    int jumpCount;
    bool isShooting;
    int HPOrig; 

    // Start is called before the first frame update 
    void Start() 
    {
        HPOrig = HP; 
        respawn(); 
        // sprint 
        playerSpeedOrig = playerSpeed; 
        // sprint 
    }
    
    // Update is called once per frame 
    void Update() 
    {
        Debug.DrawRay( Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red ); 

        if ( Input.GetButton("Shoot") & !isShooting && !GameManager.instance.isPaused ) 
        {
            StartCoroutine( shoot() ); 
        }

        movement(); 
    }

    void movement() 
    {
        // sprint /**/
        isSprinting = Input.GetKey( sprintKey ); 
        if ( !isSprinting ) 
        {
            playerSpeed = playerSpeedOrig; 
        }
        else if ( isSprinting ) 
        {
            playerSpeed = sprintSpeed; 
        }
        // sprint 

        groundedPlayer = controller.isGrounded; 

        if ( groundedPlayer ) 
        {
            jumpCount = 0;
            playerVel.y = 0; 
        }

        move = Input.GetAxis( "Horizontal" ) * transform.right 
             + Input.GetAxis( "Vertical" ) * transform.forward; 

        controller.Move( move * playerSpeed * Time.deltaTime ); // this is telling the player object to move at a speed based on time 

        //if ( Input.GetButtonDown( "Jump" ) && jumpCount < jumpMax ) 
        if ( Input.GetKey( jumpKey ) && jumpCount < jumpMax ) 
        {
            playerVel.y = jumpHeight; 
            jumpCount++; 
        }

        playerVel.y += gravity * Time.deltaTime; 
        controller.Move( playerVel * Time.deltaTime ); 
    }

    IEnumerator shoot() 
    {
        isShooting = true; 

        RaycastHit hit; 
        if ( Physics.Raycast( Camera.main.ViewportPointToRay( new Vector2( 0.5f, 0.5f )), out hit, shootDist )) 
        {
            // we need to damage stuff 
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if ( hit.transform != transform && dmg != null ) // if we did not hit ourselves & if what we hit can take damage 
            {
                dmg.takeDamage( shootDamage ); 
            }
        }

        yield return new WaitForSeconds( shootRate ); 
        isShooting = false; 
    }

    public void takeDamage( int amount ) 
    {
        HP -= amount; 
        updatePlayerUI();
        StartCoroutine( flashDamage() ); 

        if ( HP <= 0 ) 
        {
            GameManager.instance.youLose(); 
        }
    }
    
    public void respawn() 
    {/*
        HP = HPOrig;
        updatePlayerUI(); 

        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPos.transform.position; 
        controller.enabled = true; */
    }

    public void updatePlayerUI() 
    {
        //GameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig; 
    }

    IEnumerator flashDamage() 
    {
        GameManager.instance.playerDamageFlash.SetActive( true ); 
        yield return new WaitForSeconds( 0.1f ); 
        GameManager.instance.playerDamageFlash.SetActive( false ); 
    }
}

