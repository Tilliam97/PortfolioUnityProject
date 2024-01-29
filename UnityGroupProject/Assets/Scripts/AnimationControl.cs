using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{

    public GameObject TP;

    bool isReloading;

    // Update is called once per frame
    void Update()
    {
        if(!isReloading) 
        StartCoroutine(Reload());
    }
    IEnumerator Reload()
    {
        isReloading = true;
        if (Input.GetButtonDown("Reload"))
        {
            TP.GetComponent<Animator>().Play("Reload");
            yield return new WaitForSeconds(1.5f);
        }
        isReloading = false;
    }
}
