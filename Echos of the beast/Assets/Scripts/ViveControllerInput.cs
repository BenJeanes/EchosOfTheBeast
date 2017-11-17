using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveControllerInput : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    
    public GameObject VR_Camera;
    public GameObject VR_Rig;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private bool moving;
    private float controllerMagnitude;

    public float slowThreshold;
    public float fastThreshold;

    public float slowSpeed;
    public float fastSpeed;
    private float moveSpeed;
    private Vector3 moveVector;

    void Awake()
    {
        moving = false;
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void Update()
    {
        controllerMagnitude = Controller.velocity.magnitude;

        if (Controller.GetHairTriggerDown())
        {
            moving = true;
        }
        if(Controller.GetHairTriggerUp())
        {
            moving = false;
        }

        if(moving)
        {
            if(controllerMagnitude > fastThreshold)
            {
                Debug.Log("Moving FAST");
                moveSpeed = fastSpeed;

            }
            else if(controllerMagnitude > slowThreshold)
            {
                Debug.Log("Moving SLOW");
                moveSpeed = slowSpeed;
            }
            else
            {
                Debug.Log("Not Moving");
                moveSpeed = 0;
            }
            moveVector = VR_Camera.transform.forward * moveSpeed;
            moveVector.y = 0;
            VR_Rig.GetComponent<Rigidbody>().velocity = moveVector;
        }
    }
}
