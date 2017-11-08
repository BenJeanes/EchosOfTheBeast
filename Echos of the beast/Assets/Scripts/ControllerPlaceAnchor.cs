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

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    bool moving = false;

    Vector3 controllerPosAtTrigDown;
    Vector3 camPosAtTrigDown;
    Vector3 rigPosAtTrigDown;

    Vector3 change;    

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();       
    }    
    private void Update()
    {
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
            //Debug.Log(VR_Rig.GetComponent<Rigidbody>().velocity.magnitude);
            //VR_Rig.GetComponent<Rigidbody>().AddForce(VR_Camera.transform.forward * (2*VR_Rig.GetComponent<Rigidbody>().velocity.magnitude));
        }
        if(moving)
        {            
            change = controllerPosAtTrigDown - this.transform.position;
            change = ClampYToZero(change);
            change *= 100;
            Debug.Log(Controller.velocity.magnitude);
            if(Controller.velocity.magnitude > 0.01f)
            {
                VR_Rig.GetComponent<Rigidbody>().AddForce(change * (1 + this.GetComponent<Rigidbody>().velocity.magnitude));
            }                        
        }
    }

    private Vector3 ClampYToZero(Vector3 v)
    {       
        return new Vector3(v.x, 0, v.z);
    }
}
