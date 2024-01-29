using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SimMeleeEnemyAI : MonoBehaviour, IDamage
{
    [Header ( "---- Components ----" )]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform meleePos;
    [SerializeField] Transform headPos;
    [SerializeField] Animator simAni;

    [Header("----- Enemy Stats -----")]
    [Range (1,5)] [SerializeField] int HP;
    [SerializeField] int fov;
    [SerializeField] int fovAtk;
    [SerializeField] int targetFaceSpeed;
     [SerializeField] int animSpeedTrans;

    [Header (" ---- Weapon Attributes ----")]
    [SerializeField] GameObject meleeWeapon;
    [Range(0.01f, 3.0f)][SerializeField] float swingSpeed;

    bool isSwinging;
    bool playerInRange;
    Vector3 playerDir;
    float angleToPlayer;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.updateGameGoal(1);
        simAni.SetBool("swing", false);
    }

    // Update is called once per frame
    void Update()
    {
        float animSpeed = agent.velocity.normalized.magnitude;


        simAni.SetFloat("Speed", Mathf.Lerp(simAni.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));

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

                if (angleToPlayer <= fovAtk && !isSwinging)
                    StartCoroutine(swing());

                //if inside stopping distance rotate enemy
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    faceTarget();
                }

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

        if (agent.remainingDistance <= agent.stoppingDistance)
            simAni.SetBool("swing", true);

        yield return new WaitForSeconds(swingSpeed);

        isSwinging = false;

        if (agent.remainingDistance > agent.stoppingDistance || angleToPlayer >= fovAtk)
            simAni.SetBool("swing", false);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        //if taking damage outside fov go to player's last known position 
        agent.SetDestination(GameManager.instance.player.transform.position);


        StartCoroutine(flashRed());

        if (HP <= 0)
        {
            GameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
    {
        //Store the orig color 
        Color origColor = model.material.color;

        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = origColor;
    }

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
        }
    }
}
