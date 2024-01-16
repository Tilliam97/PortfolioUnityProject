using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [SerializeField] int HP;
    [SerializeField] float playerSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravity;

/*    [SerializeField] int shootDamage;
    [SerializeField] float shootSpeed;
    [SerializeField] int shootDistance;*/



    private Vector3 move;
    private Vector3 playerVel;

    private bool groundedPlayer;
    int jumpCount;

    //bool isShooting;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
/*        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);

        if (Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(shoot());
        }*/
        Movement();
    }

    void Movement()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer)
        {
            jumpCount = 0;
        }

        move = Input.GetAxis("Horizontal") * transform.right
            + Input.GetAxis("Vertical") * transform.forward;

        controller.Move(move * playerSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVel.y = jumpHeight;
            jumpCount++;
        }

        playerVel.y += gravity * Time.deltaTime;

        controller.Move(playerVel * Time.deltaTime);
    }
/*
    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }
        yield return new WaitForSeconds(shootSpeed);

        isShooting = false;
    }*/
}
