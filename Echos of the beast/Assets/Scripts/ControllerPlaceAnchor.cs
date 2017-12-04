using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlaceAnchor : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObj;
    public GameObject anchor;

    public GameObject VR_Camera;
    public GameObject VR_Rig;

    public float scalar;

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
            //Debug.Log(string.Format("{0} * {1} * (1 + {2})", change, scalar, this.GetComponent<Rigidbody>().velocity.magnitude));
        }
        if(moving)
        {
            
            change = controllerPosAtTrigDown - this.transform.position;
            change = ClampYToZero(change);
            VR_Rig.GetComponent<Rigidbody>().AddForce(change * scalar * (1 + this.GetComponent<Rigidbody>().velocity.magnitude));
            if(VR_Rig.GetComponent<Rigidbody>().velocity.magnitude > 0.5f)
            {
                //Debug.Log(VR_Rig.GetComponent<Rigidbody>().velocity.magnitude);
            }
        }
    }

    private Vector3 ClampYToZero(Vector3 v)
    {
        return v = new Vector3(v.x, 0, v.z);
    }
}
