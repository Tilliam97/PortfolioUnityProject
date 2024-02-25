using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyBindManager : MonoBehaviour
{
    private static readonly string forwardPref = "forwardPref";
    private static readonly string backwardPref = "backwardPref";
    private static readonly string leftStrafePref = "leftStrafePref";
    private static readonly string rightStrafePref = "rightStrafePref";
    private static readonly string jumpPref = "jumpPref";
    private static readonly string sprintPref = "sprintPref";
    private static readonly string dashPref = "dashPref";
    private static readonly string reloadPref = "reloadPref";
    private static readonly string pausePref = "pausePref";


    [Header("Objects")]
    [SerializeField] private TextMeshProUGUI forward;
    [SerializeField] private TextMeshProUGUI backward;
    [SerializeField] private TextMeshProUGUI leftStrafe;
    [SerializeField] private TextMeshProUGUI rightStrafe;
    [SerializeField] private TextMeshProUGUI jump;
    [SerializeField] private TextMeshProUGUI sprint;
    [SerializeField] private TextMeshProUGUI dash;
    [SerializeField] private TextMeshProUGUI reload;
    [SerializeField] private TextMeshProUGUI pause;

    string currKey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeForward()
    {
        currKey = forward.text;
        forward.text = "Not Assigned";
    }
}
