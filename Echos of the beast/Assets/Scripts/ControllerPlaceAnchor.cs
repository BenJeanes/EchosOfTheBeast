using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlaceAnchor : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObj;
    public GameObject anchor;

    public GameObject VR_Camera;
    public GameObject VR_Rig;

    public float forceMultiplier;
    public float movementScalar;
    public float someScale = 10;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    bool moving = false;

    Vector3 controllerPosAtTrigDown;
    Vector3 camPosAtTrigDown;
    Vector3 rigPosAtTrigDown;

    Vector3 change;

    public float moveSpeed = 2;
    float magnitude;

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        
    }    
    private void Update()
    {
        magnitude = Controller.velocity.magnitude;
        if(Controller.GetHairTriggerDown())
        {
            moving = true;
            controllerPosAtTrigDown = this.transform.position;
            camPosAtTrigDown = VR_Camera.transform.position;
            rigPosAtTrigDown = VR_Rig.transform.position;
        }
        if(Controller.GetHairTriggerUp())
        {
            moving = false;            
        }
        if(moving)
        {            
            change = controllerPosAtTrigDown - this.transform.position; //Difference between controller posiiton at trigger press and current position of controller.
            change = ClampYToZero(change);
            change *= 100;

            Vector3 pos = camPosAtTrigDown;
            Vector3 dir = ClampYToZero(controllerPosAtTrigDown - camPosAtTrigDown).normalized;
            Debug.DrawLine(pos, pos + (dir * 10), Color.red);
            if (Controller.velocity.magnitude > 0.01f)
            {
                //VR_Rig.GetComponent<Rigidbody>().AddForce(change * ((1 + (this.GetComponent<Rigidbody>().velocity.magnitude * 2))));                
            }
            //VR_Rig.GetComponent<Rigidbody>().AddForce(ClampYToZero(dir) * someScale);
           
            if(magnitude < 0.1)
            {
                moveSpeed = 0;
                Debug.Log("1");
                VR_Rig.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            else if(magnitude < 2)
            {
                Debug.Log("2");
                moveSpeed = 0.1f;
                VR_Rig.GetComponent<Rigidbody>().velocity = VR_Camera.transform.forward * 200;
            }
            else
            {
                moveSpeed = 0.3f;
                Debug.Log("3");
                VR_Rig.GetComponent<Rigidbody>().velocity = VR_Camera.transform.forward * 500;
            }            
            Vector3 moveVector;
            moveVector = VR_Camera.transform.forward * moveSpeed;
            //Debug.Log(moveVector);
            //VR_Rig.GetComponent<Rigidbody>().AddForce(moveVector, ForceMode.VelocityChange);
            //Debug.Log(moveSpeed);
            //Debug.Log(magnitude);
            //Debug.Log(VR_Rig.GetComponent<Rigidbody>().velocity);
        }
        else
        {
            VR_Rig.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private Vector3 ClampYToZero(Vector3 v)
    {       
        return new Vector3(v.x, 0, v.z);
    }
}
