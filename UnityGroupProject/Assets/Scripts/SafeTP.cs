using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTP : MonoBehaviour
{
    [SerializeField] float timeSave;
    [SerializeField] public bool canTP;
    [SerializeField] GameObject Player;

    CharacterController controller;
    public Vector3 playerPos;
    public float yRot;
    bool isGrounded;
    bool canSave;
    


    // Start is called before the first frame update
    void Awake()
    {
        canSave = true;
        //controller = Player.GetComponent<CharacterController>();
    }
    
    // Update is called once per frame
    void Update()
    {
        GroundedCheck();
        if (canTP)
        {
            if (isGrounded && canSave) // add restriction of canSave
            {
                //save player pos and rot
                StartCoroutine(SavePlayerPos());
            }
        }
    }

    void GetPlayerPos()
    {
        
        playerPos = Player.transform.localPosition;
        yRot = Player.transform.localRotation.y;
    }
    IEnumerator SavePlayerPos()
    {
        canSave = false;
        GetPlayerPos();
        // instantiate safe player spawn
        yield return new WaitForSeconds(timeSave);
        canSave = true;
    }

    void GroundedCheck()
    {
        // checking for object just below player using raycast cause I don't like controller.isgrounded feature

        // change to a shpear cast so it isnt a single point or a plane cast
        RaycastHit floorhit;
        Vector3 down = transform.TransformDirection(-Vector3.up);
        if (Physics.Raycast(transform.position, down, out floorhit, 1.1f))
        {
            //Debug.DrawLine(transform.position, floorhit.point, Color.red);
            isGrounded = true;
        }
        else
            isGrounded = false;
    }
}
