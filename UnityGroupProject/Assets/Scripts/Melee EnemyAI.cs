using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyAI : MonoBehaviour, IDamage
{

    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform meleePos;

    [SerializeField] int HP;
    [SerializeField] GameObject weapon;
    [SerializeField] float swingSpeed;
    [SerializeField] GameObject weaponAni;

    bool isSwinging;
    bool playerInRange;

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
            agent.SetDestination(GameManager.instance.player.transform.position);

            if (!isSwinging) 
            {
                StartCoroutine(slice());            
            }
        }
    }

    IEnumerator slice()
    {
        isSwinging = true;

        weaponAni.GetComponent<Animator>().Play("basic weapon swing");
        yield return new WaitForSeconds(swingSpeed);

        isSwinging = false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        StartCoroutine(flashRed());

        if (HP <= 0)
        {
            GameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
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
