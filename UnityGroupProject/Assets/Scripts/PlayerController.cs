using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage, IDamageTeleport, IHeal, IAmmoRefill
{
    #region Player Settings 
    [Header("----- Player Settings -----")]
    [SerializeField] CharacterController controller;
    [Range(1, 10)][SerializeField] int HP;

    public KeyCode reloadKey = KeyCode.R;
    [Range(1, 100)][SerializeField] int PistolAmmoCapacity;
    [Range(1, 12)][SerializeField] int PistolMagCapacity;

    int CurrPistolAmmo;
    int MaxPistolAmmo;
    int CurrPistolMag;
    int MaxPistolMag;

    //[Range (1, 25)] [SerializeField] int SniperAmmoCapacity; 
    //[Range (1, 8)] [SerializeField] int SniperMagCapacity; 
    //
    //[Range (1, 50)] [SerializeField] int ShotgunAmmoCapacity; 
    //[Range (1, 5)] [SerializeField] int ShotgunMagCapacity; 
    #endregion

    #region Speed & Sprint Variables 
    [Header("----- Player Speed & Sprint Settings -----")]
    public KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] float playerSpeed;
    [SerializeField] float sprintSpeed;
    float playerSpeedOrig;
    bool isSprinting;
    #endregion

    #region Jump Variables 
    [Header("----- Player Jump Settings -----")]
    public KeyCode jumpKey = KeyCode.Space;
    [SerializeField] int jumpMax;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravity;
    #endregion

    #region Dash 
    [Header("----- Player Dash Settings -----")]
    [SerializeField] float dashForce;
    [SerializeField] float dashUpwardForce;
    [SerializeField] float dashTime;
    [SerializeField] int dashMax;
    public KeyCode dashKey = KeyCode.E;
    //private bool isdashing; //commenting out for now cause annoying warning - use this for bullet time or immunity frames 
    private int dashCount;
    #endregion

    #region Gun Variables 
    [Header("----- Player Gun Settings -----")]
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    #endregion

    #region SafeTelport 
    [Header("----- Safe Teleport Settings ----- ")]
    [SerializeField] SafeTP safeTP;
    Vector3 posSafe;
    float rotYSafe;
    bool canTP;
    #endregion


    Vector3 move;
    Vector3 playerVel;
    public bool groundedPlayer;
    int jumpCount;
    bool isShooting;
    int HPOrig;





    // Start is called before the first frame update 
    void Start()
    {
        HPOrig = HP;
        playerSpeedOrig = playerSpeed;

        // wip 
        MaxPistolAmmo = PistolAmmoCapacity;
        MaxPistolMag = PistolMagCapacity;
        CurrPistolAmmo = MaxPistolAmmo;
        CurrPistolMag = MaxPistolMag;
        // wip

        respawn();  // moved down to bottom of start.  if UI doesn't exist in scene player original speed is not set
        canTP = safeTP.canTP;
    }

    // Update is called once per frame 
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.yellow);

        if (Input.GetButton("Shoot") & !isShooting && !GameManager.instance.isPaused)
        {
            StartCoroutine(shoot());
            /*Debug.Log(shoot()); */
        }

        TPCheck();
        movement();
        Reload(ref CurrPistolMag, ref MaxPistolMag, ref CurrPistolAmmo, ref MaxPistolAmmo);
    }

    void movement()
    {
        move = Input.GetAxis("Horizontal") * transform.right
            + Input.GetAxis("Vertical") * transform.forward;

        controller.Move(move * playerSpeed * Time.deltaTime); // this is telling the player object to move at a speed based on time 


        // sprint /**/
        isSprinting = Input.GetKey(sprintKey);
        if (!isSprinting)
        {
            playerSpeed = playerSpeedOrig;
        }
        else if (isSprinting)
        {
            playerSpeed = sprintSpeed;
        }
        // sprint 

        groundedPlayer = controller.isGrounded;

        if (groundedPlayer)
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
        if (Input.GetKeyDown(jumpKey) && jumpCount < jumpMax)
        {
            playerVel.y = jumpHeight;
            jumpCount++;
        }

        playerVel.y += gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);
    }

    private IEnumerator Dash()
    {
        //isdashing = true;
        playerVel = new Vector3(move.x * dashForce, dashUpwardForce, move.z * dashForce);
        yield return new WaitForSeconds(dashTime);
        playerVel = Vector3.zero;
        //isdashing = false;
    }


    #region Reload WIP 
    void Reload(ref int CurrMag, ref int MaxMag, ref int CurrAmmo, ref int MaxAmmo) // may have to pass these by reference 
    {
        if (Input.GetKeyDown(reloadKey) && CurrMag != MaxMag)
        {
            int magFill = MaxMag - CurrMag;

            if (CurrAmmo > 0 && CurrAmmo >= magFill)
            {
                CurrMag += magFill;
                CurrAmmo -= magFill;
                updatePlayerUI();
            }
            else if (CurrAmmo > 0 && CurrAmmo < magFill) // use CurrAmmo (less than magFill, greater than 0) 
            {
                CurrMag += CurrAmmo;
                CurrAmmo = 0;
            }
        }
    }

    void FillAmmo(int CurrAmmo, int MaxAmmo, int fillAmount)
    {
        if (CurrAmmo + fillAmount > MaxAmmo)
        {
            CurrAmmo = MaxAmmo;
        }
        else
        {
            CurrAmmo += fillAmount;
        }
    }

    public void RefillAmmo(AmmoTypes ammoType, int ammoAmount)
    {
        switch (ammoType)
        {
            case AmmoTypes.PISTOL:
                FillAmmo(CurrPistolAmmo, MaxPistolAmmo, ammoAmount);
                break;
            case AmmoTypes.SNIPER:
                break;
            case AmmoTypes.SHOTGUN:
                break;
            default:
                break;
        }
    }
    #endregion 

    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            // we need to damage stuff 
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null && CurrPistolMag > 0) // if we did not hit ourselves & if what we hit can take damage 
            {
                dmg.takeDamage(shootDamage);
            }
        }
        if ( CurrPistolMag > 0 ) 
        {
            CurrPistolMag--; 
        }

        Debug.Log(CurrPistolMag);
        //Debug.Log( CurrPistolAmmo ); 

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    void TPCheck()
    {
        if (canTP)
        {
            posSafe = safeTP.playerPos;
            rotYSafe = safeTP.yRot;
        }
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

    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();
        StartCoroutine(flashDamage());

        if (HP <= 0)
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
        GameManager.instance.playerAmmoBar.fillAmount = (float)CurrPistolMag / PistolMagCapacity;

    }

    IEnumerator flashDamage()
    {
        GameManager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.playerDamageFlash.SetActive(false);
    }

    public void HealMe(int amount)
    {
        HP += amount;
        if (HP > HPOrig)
            HP = HPOrig;
        updatePlayerUI();
        // UI make flash green
    }


}

