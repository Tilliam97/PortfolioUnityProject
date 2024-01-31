using System.Collections; 
using System.Collections.Generic; 
using Unity.VisualScripting; 
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
    [SerializeField] List<GunStats> gunList = new List<GunStats>(); 
    [SerializeField] GameObject gunModel; 
    [SerializeField] int shootDamage; 
    [SerializeField] float shootRate; 
    [SerializeField] int shootDist; 
    [SerializeField] int CurMag; 
    [SerializeField] int MaxMag; 
    [SerializeField] int CurAmmo; 
    [SerializeField] int MaxAmmo; 
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
    int selectedGun; 

    bool isFlashing;
    bool magIsEmpty;




    // Start is called before the first frame update 
    void Start()
    {
        HPOrig = HP; 
        playerSpeedOrig = playerSpeed;

        // wip 
        //MaxPistolAmmo = PistolAmmoCapacity;
        //MaxPistolMag = PistolMagCapacity;
        //CurrPistolAmmo = MaxPistolAmmo;
        //CurrPistolMag = MaxPistolMag;
        //updateAmmoText();
        // wip

        respawn();  // moved down to bottom of start.  if UI doesn't exist in scene player original speed is not set
        canTP = safeTP.canTP;
    }

    // Update is called once per frame 
    void Update()
    {
        if ( !GameManager.instance.isPaused ) 
        {
            movement(); 
            TPCheck(); 
            // Debug.DrawRay( Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.yellow ); 

            if ( gunList.Count > 0 ) 
            {
                selectGun(); 
                //Reload( ref CurrPistolMag, ref MaxPistolMag, ref CurrPistolAmmo, ref MaxPistolAmmo );
                Reload(); 

                if ( Input.GetButton( "Shoot" ) & !isShooting ) 
                {
                    StartCoroutine( shoot() ); 
                }

                if (magIsEmpty && !isFlashing && CurrPistolAmmo > 0)
                {
                    StartCoroutine(promptReload());
                }
                else if (magIsEmpty && CurrPistolAmmo == 0)
                {
                    OutOfAmmo();
                }
            }
        }
    }

    void movement()
    {
        move = Input.GetAxis("Horizontal") * transform.right
            + Input.GetAxis("Vertical") * transform.forward;

        controller.Move(move * playerSpeed * Time.deltaTime); // this is telling the player object to move at a speed based on time 


        // sprint 
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


    #region Reload 
    void Reload() 
    {
        if ( Input.GetKeyDown( reloadKey ) && CurMag != MaxMag ) // if you push the R key & you are not at full mag capacity 
        {
            int magFill = MaxMag - CurMag; // this is how much ammo is needed to fill the mag 

            if ( CurAmmo > 0 && CurAmmo >= magFill ) // if you have enough ammo to fully fill your mag 
            {
                CurMag += magFill; 
                CurAmmo -= magFill; 
            }
            else if ( CurAmmo > 0 && CurAmmo < magFill) // if you don't have enough ammo to fully fill your mag, use CurrAmmo (less than magFill, greater than 0) 
            {
                CurMag += CurAmmo; 
                CurAmmo = 0; 
            }
            updatePlayerUI(); 


            if ( CurMag > 0 ) 
            {
                magIsEmpty = false; 
            }
        }
    }

    void FillAmmo( int fillAmount ) 
    {
        if ( CurAmmo + fillAmount > MaxAmmo ) 
        {
            CurAmmo = MaxAmmo; 
        }
        else 
        {
            CurAmmo += fillAmount; 
        }
    }

    public void RefillAmmo( AmmoTypes ammoType, int ammoAmount ) 
    {
        switch ( ammoType ) 
        {
            case AmmoTypes.PISTOL: 
                FillAmmo( ammoAmount ); 
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
        if ( gunList[selectedGun].CurGunMag > 0 ) 
        {
            isShooting = true; 
            gunList[selectedGun].CurGunMag--; 

            RaycastHit hit; 
            if ( Physics.Raycast(Camera.main.ViewportPointToRay( new Vector2( 0.5f, 0.5f )), out hit, shootDist )) 
            {
                // we need to damage stuff 
                IDamage dmg = hit.collider.GetComponent<IDamage>(); 

                if ( hit.transform != transform && dmg != null ) // if we did not hit ourselves & if what we hit can take damage 
                {
                    dmg.takeDamage( shootDamage ); 
                }

                //Instantiate(gunList[selectedGun].hitEffect, hit.point, gunList[selectedGun].hitEffect.transform.rotation ); // gunshot effect, applicable for every gun 
            }
            
            yield return new WaitForSeconds( shootRate ); 
            isShooting = false; 
        }
    }

    void selectGun() 
    {
        if ( Input.GetAxis( "Mouse ScrollWheel" ) > 0 && selectedGun < gunList.Count-1 ) 
        {
            selectedGun++; 
            changeGun(); 
        }
        else if ( Input.GetAxis( "Mouse ScrollWheel" ) < 0 && selectedGun > 0 ) 
        {
            selectedGun--; 
            changeGun(); 
        }
    }

    void changeGun() 
    {
        shootDamage = gunList[selectedGun].shootDamage; 
        shootDist = gunList[selectedGun].shootDist; 
        shootRate = gunList[selectedGun].shootRate; 

        CurMag = gunList[selectedGun].CurGunMag; 
        MaxMag = gunList[selectedGun].MaxGunMag; 
        CurAmmo = gunList[selectedGun].CurGunCapacity; 
        MaxAmmo = gunList[selectedGun].MaxGunCapacity; 

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh; // this gives us the gun model 
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial; 
    }

    public void getGunStats( GunStats gun ) 
    {
        gunList.Add( gun ); 

        shootDamage = gun.shootDamage; 
        shootDist = gun.shootDist; 
        shootRate = gun.shootRate; 



        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.model.GetComponent<MeshFilter>().sharedMesh; // this gives us the gun model 
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.model.GetComponent<MeshRenderer>().sharedMaterial; 

        selectedGun = gunList.Count - 1; 
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
        updateHealthText();
        updateAmmoText();

    }

    public void updateHealthText()
    {
        GameManager.instance.HPTxt.text = HP + "/" + HPOrig;
    }
    public void updateAmmoText()
    {
        GameManager.instance.AmmoTxt.text = CurrPistolMag + "/" + CurrPistolAmmo;
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

    IEnumerator promptReload()
    {
        isFlashing = true;
        GameManager.instance.reloadPrompt.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.reloadPrompt.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        isFlashing = false;
    }

    public void OutOfAmmo()
    {
        GameManager.instance.outOfAmmoPrompt.SetActive(true);
        if (CurrPistolAmmo > 0 || CurrPistolMag > 0)
        {
            GameManager.instance.outOfAmmoPrompt.SetActive(false);
        }
    }


}

