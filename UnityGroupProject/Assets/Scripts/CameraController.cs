using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public class CameraController : MonoBehaviour 
{
    [Header("----- Camera Settings -----")] 
    [SerializeField] float sensitivityX;
    [SerializeField] float sensitivityY;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

    float xRot;

    private static readonly string YPref = "YSensitivityPref";
    private static readonly string XPref = "XSensitivityPref";

    public bool tp;

    // Start is called before the first frame update 
    void Start() 
    {
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
        LoadSensitivity();
    }
    
    // Update is called once per frame 
    void Update() 
    {
        // get input 
        float mouseY = Input.GetAxis( "Mouse Y" ) * Time.deltaTime * sensitivityY; 
        float mouseX = Input.GetAxis( "Mouse X" ) * Time.deltaTime * sensitivityX;

        if ( invertY ) 
            xRot += mouseY; 
        else 
            xRot -= mouseY; 

        // clamp the rot on the X-axis 
        xRot = Mathf.Clamp( xRot, lockVertMin, lockVertMax );

        // rotate the cam on the X-axis 
        transform.localRotation = Quaternion.Euler( xRot, 0, 0 );

        // rotate the player on the Y-axis 
        transform.parent.Rotate( Vector3.up * mouseX );

       
        // rotate camera when of direction of last known pos - player must be teleporting
        if (tp)
        {
            // set x-axis to be leveled
            //transform.localRotation = Quaternion.Euler(Vector3.right);
            tp = false;
        }
    }

    private void LoadSensitivity()
    {
        sensitivityX = PlayerPrefs.GetFloat(XPref);
        sensitivityY = PlayerPrefs.GetFloat(YPref);

    }
}

