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
    void Start()
    {
        canSave = true;
        controller = Player.GetComponent<CharacterController>();
    }
    
    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
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
}
