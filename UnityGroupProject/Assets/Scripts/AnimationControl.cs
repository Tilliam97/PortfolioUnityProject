using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{

    public GameObject TP;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Reload"))
        {
            //TP.GetComponent<Animator>().Play("Reload");
            TP.GetComponent<Animator>().Play("Reload");
        }
    }
}
