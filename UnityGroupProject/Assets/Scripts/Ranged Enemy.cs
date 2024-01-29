using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Task List Today
/// 
/// 1. Have the enemy follow the player up ramps and jump
/// 2. 
/// 3. 
/// 4. 
/// </summary>

public class RangedEnemy : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPosLeft;
    [SerializeField] Transform shootPosRight;
    [SerializeField] Transform headPos;

    [Header("----- Enemy Stats -----")]
    [Range(1, 100)][SerializeField] int HP;
    [SerializeField] int viewCone;
    [SerializeField] int fovshoot;
    [SerializeField] int targetFaceSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int attackRange;
    
    [Header("----- Gun Attributes -----")]
    [SerializeField] GameObject bullet;
    [SerializeField] float shootrate;

    bool isShooting;
    bool playerInRange;
    Vector3 playerDir;
    float angleToPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            if (canSeePlayer())
            {

            }
        }
    }

    bool canSeePlayer()
    {
        playerDir = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);
        Debug.DrawRay(headPos.position, playerDir);


        RaycastHit hit;
        if(Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {
                agent.SetDestination(GameManager.instance.player.transform.position);
                
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    faceTarget();

                    if (angleToPlayer <= fovshoot && !isShooting)
                        StartCoroutine(shoot());
                }

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
        Instantiate(bullet, shootPosLeft.position, transform.rotation);
        yield return new WaitForSeconds(shootrate);
        Instantiate(bullet, shootPosRight.position, transform.rotation);
        yield return new WaitForSeconds(shootrate);
        isShooting = false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        agent.SetDestination(GameManager.instance.player.transform.position);
        StartCoroutine(flashRed());

        Debug.Log("Take Damage");

        if (HP <= 0)
        {
            GameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
    {
        Color color = model.material.color;
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = color;
    }

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
        }
    }

}
