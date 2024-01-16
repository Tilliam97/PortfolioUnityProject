using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public class CameraController : MonoBehaviour 
{
    [SerializeField] int sensitivity; 
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

    float xRot; 

    // Start is called before the first frame update 
    void Start() 
    {
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked; 
    }
    
    // Update is called once per frame 
    void Update() 
    {
        // get input 
        float mouseY = Input.GetAxis( "Mouse Y" ) * Time.deltaTime * sensitivity; 
        float mouseX = Input.GetAxis( "Mouse X" ) * Time.deltaTime * sensitivity;

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

    }
}

