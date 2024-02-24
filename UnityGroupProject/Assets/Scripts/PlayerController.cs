using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage, IDamageTeleport, IHeal, IAmmoRefill
{
    #region Player Settings 
    [Header("----- Player Settings -----")]
    //[SerializeField] CharacterController controller;
    [Range(1, 100)][SerializeField] int HP;

    //Rigid Body Settings
    Rigidbody rb;
    float _horMove;
    float _verMove;
    [SerializeField] float rbDrag = 6f;
    [SerializeField] float airDrag = 2f;
    float movementMult = 10f;
    float airMult = 0.4f;
    // needs gravity force to apply only when not grounded
    

    public KeyCode reloadKey = KeyCode.R;
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
    //[Range(-25.0f, 0)] public float gravity; // gravity is set by the physics tab in the project settings for rigid body
    #endregion

    #region Dash Variables 
    [Header("----- Player Dash Settings -----")]
    [SerializeField] float dashForce;
    [SerializeField] float dashUpwardForce;
    [SerializeField] float dashTime;
    [SerializeField] int dashMax;
    public KeyCode dashKey = KeyCode.E;
    //private bool isdashing; //commenting out for now cause annoying warning - use this for bullet time or immunity frames 
    private int dashCount;
    #endregion

    #region Drop Gun Variables 
    [Header("----- Weapon Drop Settings -----")]
    public KeyCode dropKey = KeyCode.X; 
    [SerializeField] GameObject PistolDrop; 
    [SerializeField] GameObject ShotgunDrop; 
    [SerializeField] GameObject SniperDrop; 

    bool isDropping; 

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

    #region Laser Gun Variables 
    [SerializeField] public Camera playerCamera; 
    [SerializeField] public Transform laserOrigin; 
    [SerializeField] public float gunRange = 50f; 
    [SerializeField] public float fireRate = 0.2f; 
    [SerializeField] public float laserDuration = 0f; 

    LineRenderer laserLine; 

    #endregion 

    #region SafeTelport Variables 
    [Header("----- Safe Teleport Settings ----- ")]
    [SerializeField] SafeTP safeTP;
    [SerializeField] bool teleportEnabled;
    public Vector3 posSafe;
    float rotYSafe;
    bool canTP;
    #endregion


    Vector3 move;
    //Vector3 playerVel; // no longer used since rigidbody change
    public bool groundedPlayer;
    int jumpCount;
    bool isShooting;
    int HPOrig;
    int selectedGun; 

    bool isFlashing;
    bool magIsEmpty;

    bool isReloading;
    //float gravityCurr;
    float startJumpHeight;

    bool isDisabled;
    

    // Start is called before the first frame update 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // 
        laserLine = GetComponent<LineRenderer>(); 
        // 

        HPOrig = HP;
        playerSpeedOrig = playerSpeed;
        //gravityCurr = gravity;
        startJumpHeight = jumpHeight; 

        respawn(); 
        canTP = safeTP.canTP;
    }

    void Update()
    {
        if (!isDisabled)
        {
            //Debug.Log("Player is enabled");
            if (!GameManager.instance.isPaused)
            {
                PlayerInput();  // players input
                ControlDrag();  // rigidbody Drag
                GroundedCheck();// is the player grounded
                JumpCheck();
                DashCheck();

                TPCheck();
                if (gunList.Count != 0)
                {
                    OutOfAmmo();
                }
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.yellow);

                if (gunList.Count > 0)
                {
                    selectGun();
                    //Reload( ref CurrPistolMag, ref MaxPistolMag, ref CurrPistolAmmo, ref MaxPistolAmmo );
                    Reload();

                    if (Input.GetButton("Shoot") & !isShooting)
                    {
                        StartCoroutine(shoot());
                    }

                    if (magIsEmpty && !isFlashing && CurAmmo > 0)
                    {
                        StartCoroutine(promptReload());
                    }

                    if (Input.GetKey(dropKey) && !isDropping)
                    {
                        StartCoroutine(DropGun());
                    }
                }
            }
        }
    }

    // Update is called once per frame 
    void FixedUpdate()
    {
        if (!isDisabled)
        {
            //Debug.Log("Player is enabled");
            if (!GameManager.instance.isPaused)
            {
                //Debug.Log("is disabled " + isDisabled);
                movement();
            }
        }
    }


    #region Player Moveset 
    void movement()
    {
        //move = Input.GetAxis("Horizontal") * transform.right
        //  + Input.GetAxis("Vertical") * transform.forward;

        RBMovement();
        //controller.Move(move * playerSpeed * Time.deltaTime); // this is telling the player object to move at a speed based on time 

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

        //groundedPlayer = controller.isGrounded;
        

        // Dash 
        
        // Dash

        //if ( Input.GetButtonDown( "Jump" ) && jumpCount < jumpMax ) 
        //Jump has been changed into a method called JumpCheck
        

        //playerVel.y += gravity * Time.deltaTime;
        //controller.Move(playerVel * Time.deltaTime);
    }

    void PlayerInput()
    {
        _horMove = Input.GetAxisRaw("Horizontal"); // inputs
        _verMove = Input.GetAxisRaw("Vertical");
        move = transform.forward * _verMove + transform.right * _horMove;
    }

    void ControlDrag()
    {
        if (groundedPlayer)
            rb.drag = rbDrag;
        else
            rb.drag = airDrag;
    }

    void RBMovement()
    {
        if (groundedPlayer)
        {
            rb.AddForce(move.normalized * playerSpeed * movementMult, ForceMode.Acceleration);
        }
        else if (!groundedPlayer)
        {
            rb.AddForce(move.normalized * playerSpeed * movementMult * airMult, ForceMode.Acceleration);
        }
    }

    void JumpCheck()
    {
        if (Input.GetKeyDown(jumpKey) && jumpCount < jumpMax)
        {
            //playerVel.y = jumpHeight;
            Jump();
            jumpCount++;
        }
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
    }

    void DashCheck()
    {
        if (Input.GetKeyDown(dashKey) && dashCount < dashMax)
        {
            StartCoroutine(Dash());
            dashCount++;
        }
    }

    private IEnumerator Dash()
    {
        //isdashing = true;
        //playerVel = new Vector3(move.x * dashForce, dashUpwardForce, move.z * dashForce); // RigidBody Change
        rb.AddForce(move.x * dashForce, dashUpwardForce, move.z * dashForce, ForceMode.Impulse);
        yield return new WaitForSeconds(dashTime);
        //playerVel = Vector3.zero;
        //isdashing = false;
    }

    private IEnumerator DropGun() 
    {
        isDropping = true; 

        AmmoTypes gunTypeToDrop = gunList[selectedGun].ammoType;    // figure out what type of gun they are trying to drop 
        GameObject gunToDrop = PistolDrop;                          // temporary assertion 
        bool isDroppable = true;                                    // is it a gun we want them to be allowed to drop 
        
        switch ( gunTypeToDrop )                                    // figure out what type of gun drop to use 
        {
            case AmmoTypes.PISTOL: 
                gunToDrop = PistolDrop; 
                break; 
            case AmmoTypes.SNIPER: 
                gunToDrop = SniperDrop; 
                break; 
            case AmmoTypes.SHOTGUN: 
                gunToDrop = ShotgunDrop; 
                break; 
            default: 
                isDroppable = false; 
                break; 
        }

        if ( isDroppable )                                          // if they are trying to drop a gun we want them to be able to drop, it'll drop 
        {
            string gunName = GetSelectedGunName(); 

            switch ( gunName ) 
            {
                case "Infinity Gun": 
                    break; 
                case "AR": 
                case "Shotgun": 
                case "Sniper": 
                case "Laser Gun": 
                    GunStats statsOfGunToDrop = new GunStats();     // create variable for stats of current gun (the gun to be dropped) 
                    statsOfGunToDrop = gunList[selectedGun];        // grab stats of current gun (this records ammo as well) 
                
                    gunList.Remove( gunList[selectedGun] );       // removes GunStats from gunList, making it unusable 
                    if ( gunList.Count > 0 ) 
                    {
                        selectedGun--; 
                        changeGun(); 
                    }

                    Vector3 dropPosition = (GameManager.instance.player.transform.position) + (GameManager.instance.player.transform.forward * 3); 
                    Quaternion dropRotation = GameManager.instance.player.transform.localRotation; 

                    gunToDrop.GetComponentInChildren<GunPickup>().gunStats = statsOfGunToDrop;
                    Instantiate( gunToDrop, dropPosition, dropRotation ); 

                    yield return new WaitForSeconds( 1 ); 

                    break; 
            }

            isDropping = false; 
        }
    }

    public bool getIsDropping()
    {
        return isDropping;
    }

    IEnumerator shoot()
    {
        if (gunList[selectedGun].CurGunMag > 0)
        {
            isShooting = true;
            gunList[selectedGun].CurGunMag--;
            CurMag--;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            {
                // we need to damage stuff 
                AIDamage dmg = hit.collider.GetComponent<AIDamage>();
                IDamage damage = hit.collider.GetComponent<IDamage>();

                if (hit.transform != transform && dmg != null) // if we did not hit ourselves & if what we hit can take damage 
                {
                    switch (dmg.damageType)
                    {
                        case AIDamage.collisionType.head: dmg.takeDamage(shootDamage * 2);                         
                        break;
                        case AIDamage.collisionType.body : dmg.takeDamage(shootDamage);
                        break;
                    }

                }
                else if ((hit.transform != transform && damage != null))                
                {
                    damage.takeDamage(shootDamage);
                }

                //Instantiate(gunList[selectedGun].hitEffect, hit.point, gunList[selectedGun].hitEffect.transform.rotation ); // gunshot effect, applicable for every gun 
            }
            if (CurMag == 0)
            {
                magIsEmpty = true;
            }

            updatePlayerUI();
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }

    /*
    IEnumerator shootLaser() 
    {
        
    }*/

    #endregion


    #region Heal/Reload/Gun Stuff 
    #region Getters 
    public int GetPlayerHP() 
    {
        return HP; 
    }
    public int GetPlayerHPOrig()
    {
        return HPOrig; 
    }
    
    public string GetSelectedGunName() 
    {
        return gunList[selectedGun].model.tag; 
    }

    #endregion

    public void HealMe( int amount ) 
    {
        HP += amount;       // Fill health 
        if ( HP > HPOrig )  // Make sure health value isn't greater than max capacity 
            HP = HPOrig; 
        updatePlayerUI();   // Update health bar 

        // UI make flash green? 
    }

    void Reload()
    {
        if (Input.GetKeyDown(reloadKey) && CurMag != MaxMag) // if you push the R key & you are not at full mag capacity 
        {
            if ( gunList[selectedGun].hasInfinteAmmo ) // infinite gun 
            {
                gunList[selectedGun].CurGunMag = gunList[selectedGun].CurGunCapacity; 
                CurMag = MaxMag; 
            }
            else
            {
                int magFill = MaxMag - CurMag; // this is how much ammo is needed to fill the mag 

                if (CurAmmo > 0 && CurAmmo >= magFill) // if you have enough ammo to fully fill your mag 
                {
                    CurMag += magFill;
                    CurAmmo -= magFill;
                    gunList[selectedGun].CurGunMag = CurMag;
                    gunList[selectedGun].CurGunCapacity = CurAmmo;
                }
                else if (CurAmmo > 0 && CurAmmo < magFill) // if you don't have enough ammo to fully fill your mag, use CurrAmmo (less than magFill, greater than 0) 
                {
                    CurMag += CurAmmo;
                    CurAmmo = 0;
                    gunList[selectedGun].CurGunMag = CurMag;
                    gunList[selectedGun].CurGunCapacity = CurAmmo;
                }
            }

            updatePlayerUI();

            if (CurMag > 0)
            {
                magIsEmpty = false;
            }
        }
    }

    public void FillAmmo( int fillAmount ) 
    {
        if ( CurAmmo + fillAmount > MaxAmmo ) 
        {
            CurAmmo = MaxAmmo; 
        }
        else 
        {
            CurAmmo += fillAmount; 
        }
        //CurMag = MaxMag; // ammo capsule should set mag & ammo capacity to full, but this line causes errors 

        updatePlayerUI(); 
    }

    public void RefillAmmo( AmmoTypes ammoType, int ammoAmount ) 
    {
        switch ( ammoType ) 
        {
            case AmmoTypes.PISTOL: 
                FillAmmo( ammoAmount ); 
                break; 
            case AmmoTypes.SNIPER: 
                FillAmmo( ammoAmount ); 
                break; 
            case AmmoTypes.SHOTGUN: 
                FillAmmo( ammoAmount ); 
                break; 
            case AmmoTypes.LASER: 
                FillAmmo( ammoAmount ); 
                break; 
            default: 
                break; 
        }
    }

    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            selectedGun++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
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
        updatePlayerUI();
        OutOfAmmo();
        if (CurMag == 0)
        {
            magIsEmpty = true;
        }
        else
        {
            magIsEmpty = false;
        }
    }

    public void getGunStats(GunStats gun)
    {
        for ( int i = 0; i < gunList.Count; i++ ) 
        {
            if ( gunList[i] == gun ) 
            {
                selectedGun = i;
                changeGun();
                RefillAmmo( gunList[selectedGun].ammoType, gunList[selectedGun].MaxGunCapacity );
                return; 
            }
        }

        gunList.Add(gun);
        selectedGun = gunList.Count - 1;

        shootDamage = gun.shootDamage;
        shootDist = gun.shootDist;
        shootRate = gun.shootRate;

        CurMag = gunList[selectedGun].CurGunMag;
        MaxMag = gunList[selectedGun].MaxGunMag;
        CurAmmo = gunList[selectedGun].CurGunCapacity;
        MaxAmmo = gunList[selectedGun].MaxGunCapacity;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.model.GetComponent<MeshFilter>().sharedMesh; // this gives us the gun model 
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.model.GetComponent<MeshRenderer>().sharedMaterial;

        updatePlayerUI();
        if (CurMag == 0)
        {
            magIsEmpty = true;
        }
        else
        {
            magIsEmpty = false;
        }
    }

    #endregion 


    #region Take Damage/Respawn 
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

    void TPCheck()
    {
        if (canTP && teleportEnabled)
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
            if (canTP)
            {
                StartCoroutine(Teleport());
            }

            takeDamage(amount);
        }
    }

    IEnumerator Teleport()
    {
        if (teleportEnabled) 
        { 
        isDisabled = true;
        //Debug.Log("Tried to teleport.  player disabled " + isDisabled);
        yield return new WaitForSeconds(.05f);
        transform.position = new Vector3(posSafe.x, posSafe.y, posSafe.z); // working on adjusting cam pos.
        //gameObject.GetComponentInChildren<CameraController>().tp = true; //set players x to be leveled.
        //transform.rotation = Quaternion.Euler(0, rotYSafe, 0); // is currently not setting the correct direction
        yield return new WaitForSeconds(.05f);
        isDisabled = false;
        //Debug.Log("player disabled " + isDisabled);
        }
    }


    public void respawn()
    {
        HP = HPOrig;
        updatePlayerUI();

        //controller.enabled = false; // commented out for Rigid body changes
        isDisabled = true;
        transform.position = GameManager.instance.playerSpawnPos.transform.position;
        //controller.enabled = true;
        isDisabled = false;

    }

    #endregion 


    #region UI/Interface 
    public void updatePlayerUI()
    {
        GameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
        updateHealthText();
        updateAmmoText();
        updateGunImage();
    }

    public void updateGunImage()
    {
        if (gunList.Count != 0)
        {
            switch (gunList[selectedGun].model.tag)
            {
                case "Pistol":
                    {
                        GameManager.instance.gunPistol.SetActive(true);
                        GameManager.instance.gunAR.SetActive(false);
                        GameManager.instance.gunShotgun.SetActive(false);
                        GameManager.instance.gunSniper.SetActive(false);
                        break;
                    }
                case "AR":
                    {
                        GameManager.instance.gunAR.SetActive(true);
                        GameManager.instance.gunPistol.SetActive(false);
                        GameManager.instance.gunShotgun.SetActive(false);
                        GameManager.instance.gunSniper.SetActive(false);
                        break;
                    }
                case "Shotgun":
                    {
                        GameManager.instance.gunShotgun.SetActive(true);
                        GameManager.instance.gunAR.SetActive(false);
                        GameManager.instance.gunPistol.SetActive(false);
                        GameManager.instance.gunSniper.SetActive(false);
                        break;
                    }
                case "Sniper":
                    {
                        GameManager.instance.gunSniper.SetActive(true);
                        GameManager.instance.gunAR.SetActive(false);
                        GameManager.instance.gunPistol.SetActive(false);
                        GameManager.instance.gunShotgun.SetActive(false);
                        break;
                    }
                case "Laser Gun":
                default:
                    {
                        break;
                    }
            }
        }
    }

    public void updateHealthText()
    {
        GameManager.instance.HPTxt.text = HP + "/" + HPOrig;
    }
    public void updateAmmoText()
    {
        if (gunList.Count == 0)
        {
            GameManager.instance.AmmoTxt.text = "00/00";
        }
        else
        {
            GameManager.instance.AmmoTxt.text = CurMag + "/" + CurAmmo;
        }
    }


    IEnumerator flashDamage()
    {
        GameManager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.playerDamageFlash.SetActive(false);
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
        if (CurAmmo > 0 || CurMag > 0)
        {
            GameManager.instance.outOfAmmoPrompt.SetActive(false);
        }
    }

    #endregion


    private void OnTriggerEnter( Collider other ) 
    {
        
    }

    public void WallRunStart()
    {
        //gravity = -2.5f;
        jumpHeight = startJumpHeight / 4;
        jumpCount = 0;
        //playerVel.y = 0; // needs adjusting for Rigid body movement
        dashCount = 0;
    }

    public void WallRunEnd()
    {
        //gravity = gravityCurr;
        jumpHeight = startJumpHeight;
    }

    void GroundedCheck()
    {
        // checking for object just below player using raycast cause I don't like controller.isgrounded feature

        // change to a shpear cast so it isnt a single point or a plane cast
        RaycastHit floorhit;
        Vector3 down = transform.TransformDirection(-Vector3.up);
        if (Physics.Raycast(transform.position, down, out floorhit, 1.1f))
        {
            Debug.DrawLine(transform.position, floorhit.point, Color.red);
            groundedPlayer = true;
        }
        else
            groundedPlayer = false;

        if (groundedPlayer)
        {
            jumpCount = 0;
            //playerVel.y = 0;
            dashCount = 0;
        }
    }
}

