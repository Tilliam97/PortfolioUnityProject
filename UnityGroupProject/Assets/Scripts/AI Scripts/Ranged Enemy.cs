using System.Collections; 
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters; 
using UnityEngine; 
using UnityEngine.AI; 
using UnityEngine.UI; 

/// <summary>
/// Task List Today
/// 
/// 1. Have the enemy roam around
/// 2. 
/// 3. 
/// 4. 
/// </summary>

public class RangedEnemy : MonoBehaviour, IDamage, IEnemy
{
    [Header("----- Components -----")]
    [SerializeField] Renderer headRenderer;
    [SerializeField] Renderer bodyRenderer;
    [SerializeField] Renderer armsRenderer;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] Animator rangedAni;
    [SerializeField] AudioSource damagedSound;
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource shootSound;

    [Header("----- Enemy Stats -----")]
    [Range(1, 15)][SerializeField] int HP;
    [SerializeField] int viewCone;
    [SerializeField] int fovShoot;
    [SerializeField] int targetFaceSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;
    
    [Header("----- Gun Attributes -----")]
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    bool isShooting;
    bool playerInRange;
    Vector3 playerDir;
    float angleToPlayer;
    bool destChosen;
    Vector3 startingPos;
    float stoppingDistOrig;
    float remainingDist;


    #region Enemy HP Bar 
    public Image enemyHPBar; 
    int HPOrig; 
    #endregion 


    // Start is called before the first frame update
    void Start()
    {

        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;

        HPOrig = HP; 
        updateEnemyUI();

        damagedSound = GameManager.instance.damagedSoundRanged;
        deathSound = GameManager.instance.deathSoundRanged;
        shootSound = GameManager.instance.shootSoundRanged;

        GameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        float animSpeed = agent.velocity.normalized.magnitude;

        rangedAni.SetFloat("Speed", Mathf.Lerp(rangedAni.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));

        if (playerInRange && !canSeePlayer())
        {
            StartCoroutine(roam());
        }

        else if (!playerInRange)
        {
            StartCoroutine(roam());
        }
    }

    #region Renderer Getters

    public Renderer GetHeadRenderer()
    {
       return headRenderer;
    }

    public Renderer GetBodyRenderer()
    {
        return bodyRenderer;
    }

    public Renderer GetArmsRenderer()
    {
        return armsRenderer;
    }

    #endregion

    IEnumerator roam()
    {

        if (agent.remainingDistance < 0.05f && !destChosen)
        {
            destChosen = true;
            //roaming dist needs to be zero to roam to exact pos
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamPauseTime);

            //move above yield to roam on start
            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startingPos;

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

        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if(Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {
                StopCoroutine(roam());

                agent.SetDestination(GameManager.instance.player.transform.position);
                
                if (agent.remainingDistance < agent.stoppingDistance && angleToPlayer <= viewCone)
                {
                    faceTarget();

                    if (angleToPlayer <= fovShoot && !isShooting)

                        StartCoroutine(shoot());
                }

                agent.stoppingDistance = stoppingDistOrig;

                return true;
            }
        }
        return false;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * targetFaceSpeed);
    }


    IEnumerator shoot()
    {
        isShooting = true;
        
        //stop enemy before shooting so doesn't chase
        agent.stoppingDistance = agent.stoppingDistance - 1;
        agent.isStopped = true;
        
        Instantiate(bullet, shootPos.position, transform.rotation);

        //play damage sound if not currently playing a sound
        if (shootSound != null)
            shootSound.Play();
        

        yield return new WaitForSeconds(shootRate);

        //resume movement
        agent.isStopped = false;
        agent.stoppingDistance = stoppingDistOrig;
        isShooting = false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        updateEnemyUI();

        if (damagedSound != null)
           damagedSound.Play();

        agent.SetDestination(GameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            GameManager.instance.updateGameGoal(-1);
            deathSound.Play();
            Destroy(gameObject);
        }
    }


    #region Enemy HP Bar 
    public void updateEnemyUI() 
    {

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

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }

}
