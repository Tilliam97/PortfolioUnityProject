using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public class PlayerController : MonoBehaviour, IDamage, IDamageTeleport
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

    #region Dash
    [SerializeField] float dashForce;
    [SerializeField] float dashUpwardForce;
    [SerializeField] float dashTime;
    [SerializeField] int dashMax;
    public KeyCode dashKey = KeyCode.E;
    //private bool dashing; 
    private int dashCount;
    #endregion

    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;


    Vector3 move; 
    Vector3 playerVel; 
    public bool groundedPlayer;
    int jumpCount;
    bool isShooting;
    int HPOrig;

    #region SafeTelport
    [SerializeField] SafeTP safeTP;
    Vector3 posSafe;
    float rotYSafe;
    bool canTP;
    #endregion

    // Start is called before the first frame update 
    void Start() 
    {
        HPOrig = HP; 
        // sprint 
        playerSpeedOrig = playerSpeed;
        // sprint 

        respawn();  // moved down to bottom of start.  if UI doesn't exist in scene player original speed is not set
        canTP = safeTP.canTP; 
    }
    
    // Update is called once per frame 
    void Update() 
    {
        Debug.DrawRay( Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.yellow ); 

        if ( Input.GetButton("Shoot") & !isShooting && !GameManager.instance.isPaused ) 
        {
            StartCoroutine( shoot() );
            /*Debug.Log(shoot());*/
        }

        
        if (canTP)
        {
            posSafe = safeTP.playerPos;
            rotYSafe = safeTP.yRot;
        }

        movement(); 
    }

    void movement() 
    {
        move = Input.GetAxis("Horizontal") * transform.right
            + Input.GetAxis("Vertical") * transform.forward;

        controller.Move(move * playerSpeed * Time.deltaTime); // this is telling the player object to move at a speed based on time 


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
            dashCount = 0; 
        }
        
        // Dash 
        if (Input.GetKeyDown(dashKey) && dashCount < dashMax)
        {
            StartCoroutine(Dash());
            dashCount++;
        }
        // Dash

       

        //if ( Input.GetButtonDown( "Jump" ) && jumpCount < jumpMax ) 
        if ( Input.GetKeyDown( jumpKey ) && jumpCount < jumpMax ) 
        {
            playerVel.y = jumpHeight; 
            jumpCount++; 
        }

        playerVel.y += gravity * Time.deltaTime; 
        controller.Move( playerVel * Time.deltaTime ); 
    }

    private IEnumerator Dash()
    {
        //dashing = true;

        playerVel = new Vector3(move.x * dashForce, dashUpwardForce, move.z * dashForce);
        yield return new WaitForSeconds(dashTime);
        playerVel = Vector3.zero;
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

    public void takeDamageTP(int amount, bool TP)
    {
        if (!TP)  // teleport is false
            takeDamage(amount);
        else      // teleport is true and will teleport player to last safe pos
        {
            takeDamage(amount);
            transform.position = posSafe; // working on adjusting cam pos.
            gameObject.GetComponentInChildren<CameraController>().tp = true; //set players x to be leveled.
            //transform.rotation = Quaternion.Euler(0, rotYSafe, 0); // is currently not setting the correct direction
        }
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
    {
        HP = HPOrig;
        updatePlayerUI(); 

        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true; 
    }

    public void updatePlayerUI() 
    {
        GameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig; 
    }

    IEnumerator flashDamage() 
    {
        GameManager.instance.playerDamageFlash.SetActive( true ); 
        yield return new WaitForSeconds( 0.1f ); 
        GameManager.instance.playerDamageFlash.SetActive( false ); 
    }
}

