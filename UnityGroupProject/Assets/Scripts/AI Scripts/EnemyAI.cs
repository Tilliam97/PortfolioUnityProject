using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.AI;
using UnityEngine.UI; 

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer Model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPos;

    [SerializeField] int HP;
    [SerializeField] GameObject bullet;
    [SerializeField] int shootSpeed;

    bool isShooting; 
    bool playerInRange;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            agent.SetDestination(GameManager.instance.player.transform.position);

            if (!isShooting)
                StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootSpeed);
        isShooting = false;
    }

    public void takeDamage(int damage)
    {
        HP -= damage;
        updateEnemyUI(); 

        StartCoroutine(flashRed());

        if (HP <= 0)
        {
            GameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
    {
        Model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        Model.material.color = Color.white;
    }

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
        }
    }
}
