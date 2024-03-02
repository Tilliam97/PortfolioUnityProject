using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{

    public GameObject TP;
    public PlayerController player;

    bool isReloading;

    // Update is called once per frame
    void Update()
    {
        if(!isReloading) 
        StartCoroutine(Reload());
    }
    IEnumerator Reload()
    {
        if (Input.GetButtonDown("Reload") && player.CurMag != player.MaxMag)
        {
            isReloading = true;
            TP.GetComponent<Animator>().Play("Reload");
            yield return new WaitForSeconds(1.1f);
            Vector3 pos = new Vector3(0.3950001f, -0.5120002f, 0.9910004f);
            TP.transform.localPosition = pos;
        }
        isReloading = false;
    }
}
