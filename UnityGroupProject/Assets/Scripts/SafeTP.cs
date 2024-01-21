using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTP : MonoBehaviour
{
    [SerializeField] float timeSave;
    [SerializeField] bool canTP;
    [SerializeField] GameObject Player;

    [SerializeField] FallTrap fallTrapScript;
    CharacterController controller;
    Vector3 playerPos;
    float yRot;
    bool isGrounded;
    bool canSave;
    


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("script found " + fallTrapScript);
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

    public void SafeSpawn(GameObject obj)
    {
        Debug.Log("Player tried to Safe Spawn");
    }
}
