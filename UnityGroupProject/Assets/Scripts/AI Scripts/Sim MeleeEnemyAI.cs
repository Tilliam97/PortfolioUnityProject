using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SimMeleeEnemyAI : MonoBehaviour, IDamage
{
    #region Enemy Components
    [Header("---- Components ----")]
    [SerializeField] public Renderer model;
    /* [SerializeField] public Renderer headRenderer;
     [SerializeField] public Renderer bodyRenderer;*/
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform meleePos;
    [SerializeField] public Transform headPos;
    [SerializeField] Animator simAni;
    [SerializeField] AudioSource damagedSound;
    [SerializeField] AudioSource deathSound;
    #endregion

    #region Enemy Stats
    [Header("----- Enemy Stats -----")]
    [Range(1, 10)][SerializeField] public int HP;
    [SerializeField] int fov;
    [SerializeField] int fovAtk;
    [SerializeField] int targetFaceSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;
    #endregion

    #region Enemy Weapon 
    [Header(" ---- Weapon Attributes ----")]
    [SerializeField] GameObject meleeWeapon;
    [SerializeField] Collider weaponCollider;
    [Range(0.01f, 3.0f)][SerializeField] float swingSpeed;
    [Range(0.05f, 1.0f)][SerializeField] float damageTime;
    #endregion

    bool isSwinging;
    bool playerInRange;
    Vector3 playerDir;
    float angleToPlayer;
    //roam fields
    bool destChosen;
    Vector3 startPos;
    float stopDistOrig;
    bool hurt;

    #region Enemy HP Bar 
    public Image enemyHPBar;
    int HPOrig;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        updateEnemyUI();
        GameManager.instance.updateGameGoal(1);
        stopDistOrig = agent.stoppingDistance;
        hurt = false;
        simAni.SetBool("swing", false);
    }

    // Update is called once per frame
    void Update()
    {
        float animSpeed = agent.velocity.normalized.magnitude;

        simAni.SetFloat("Speed", Mathf.Lerp(simAni.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));

        if (playerInRange && !canSeePlayer())
        {
            StartCoroutine(roam());
        }
        else if (!playerInRange)
        {
            StartCoroutine(roam());
        }
    }

    #region AI Controls

    IEnumerator roam()
    {
        startPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

        if (agent.remainingDistance < 0.05f && !destChosen)
        {
            destChosen = true;
            //roaming dist needs to be zero to roam to exact pos
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamPauseTime);

            //move above yield to roam on start
            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            destChosen = false;

        }
    }

    bool canSeePlayer()
    {
        playerDir = GameManager.instance.player.transform.position - headPos.position;

        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x,
                                                0, playerDir.z),
                                                transform.forward);

        Debug.Log(angleToPlayer);
        Debug.DrawRay(transform.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= fov)
            {
                agent.SetDestination(GameManager.instance.player.transform.position);

                if (angleToPlayer <= fovAtk && !isSwinging && !hurt)
                    StartCoroutine(swing());

                //if inside stopping distance rotate enemy
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    faceTarget();
                }

                agent.stoppingDistance = stopDistOrig;

                return true;
            }
        }

        return false;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x,
                                                             transform.position.y,
                                                             playerDir.z));

        //Smooth rotation over time while moving and inside stopping distance 
        transform.rotation = Quaternion.Lerp(transform.rotation, rot,
                                    Time.deltaTime * targetFaceSpeed);

    }

    IEnumerator swing()
    {
        isSwinging = true;

        if (agent.remainingDistance <= agent.stoppingDistance && !hurt)
            simAni.SetBool("swing", true);

        yield return new WaitForSeconds(swingSpeed);

        isSwinging = false;

        if (agent.remainingDistance > agent.stoppingDistance || angleToPlayer >= fovAtk)
            simAni.SetBool("swing", false);
    }
    #endregion

    #region Take Damage
    IEnumerator damaged()
    {
        simAni.SetBool("swing", false);

        hurt = true;


        simAni.SetBool("Hit", true);
        //Play damage sound 
        if (damagedSound != null)
        {
            damagedSound.Play();
        }


        yield return new WaitForSeconds(damageTime);

        hurt = false;

        simAni.SetBool("Hit", false);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        if (HP <= HPOrig && HP > 0)
        {
            StartCoroutine(damaged());
        }
        //if taking damage outside fov go to player's last known position 
        agent.SetDestination(GameManager.instance.player.transform.position);

        if (HP <= 0 && deathSound != null)
        {
            deathSound.Play();
            GameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }

    }
    #endregion

    #region Enemy HP Bar 
    public void updateEnemyUI()
    {
        //GameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig; 
        this.enemyHPBar.fillAmount = (float)HP / HPOrig;
    }

    #endregion 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }

    public void weaponColliderOn()
    {
        weaponCollider.enabled = true;
    }

    public void weaponColliderOff()
    {
        weaponCollider.enabled = false;
    }

}