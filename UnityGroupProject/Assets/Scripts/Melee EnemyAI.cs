using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyAI : MonoBehaviour, IDamage
{
    [Header ( "---- Components ----" )]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform meleePos;
    [SerializeField] Transform headPos;
    [SerializeField] Animator weaponAniController;

    [Header("----- Enemy Stats -----")]
    [Range (1,5)] [SerializeField] int HP;
    [SerializeField] int fov;
    [SerializeField] int fovAtk;
    [SerializeField] int targetFaceSpeed;

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
        weaponAniController.SetBool("swing", false);
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
        playerDir = playerDir = GameManager.instance.player.transform.position - headPos.position;

        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

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
        Quaternion rot = Quaternion.LookRotation(playerDir);
        //Smooth rotation over time while moving and inside stopping distance 
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * targetFaceSpeed);

    }

    IEnumerator swing()
    {
        isSwinging = true;

        yield return new WaitForSeconds(swingSpeed);

        isSwinging = false;
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
            weaponAniController.SetBool("swing", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            weaponAniController.SetBool("swing", false);
        }
    }
}
