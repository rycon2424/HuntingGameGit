using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [SerializeField] FPSPlayer fpsPlayer;
    [SerializeField] Transform cameraTransform;
    RaycastHit hit;

    [SerializeField] float viewRangeY = 80; // THE RANGE THE PLAYER CAN LOOK UP AND DOWN IN THE Y AXIS
    [HideInInspector]
    float rotX;
    float rotY;

    void Start()
    {
        //Shader.Find("DeferredNightVision");
        //Shader.WarmupAllShaders();
    }

    void Update()
    {
        if (fpsPlayer.pv.IsMine)
        {
            if (fpsPlayer.pausescript.paused == false)
            {
                MoveView();
            }
        }
    }

    private void MoveView() // CAMERA ROTATION AND CLAMPING
    {
        rotX += Input.GetAxis("Mouse X") * fpsPlayer.mouseSensitivityX;
        rotY += Input.GetAxis("Mouse Y") * fpsPlayer.mouseSensitivityY;
        rotY = Mathf.Clamp(rotY, -viewRangeY, viewRangeY);
        cameraTransform.localRotation = Quaternion.Euler(-rotY, 0f, 0f);
    }

    public void Raycast() // RAYCASTING
    {
        //Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * FPSPlayer.rangeCameraRay);  //RayCast Debug

        Ray myRay = new Ray(gameObject.transform.position, gameObject.transform.forward);

        if (Input.GetKeyDown(fpsPlayer.pc.interact) && Physics.Raycast(myRay, out hit, fpsPlayer.rangeCameraRay))
        {
            //Debug.Log(hit.collider.gameObject.name + " has as tag "+ hit.collider.tag);
            if (hit.collider.CompareTag("Test"))
            {

            }
        }
    }

    public void Death()
    {
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        this.enabled = false;
    }

}