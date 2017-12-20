using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchpadMovement : MonoBehaviour {

    [SerializeField]
    private Transform playerRig;

    private Valve.VR.EVRButtonId touchpad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index);} }
    private SteamVR_TrackedObject trackedObj;

    private Vector2 axis;
    private Vector3 moveVector;

	// Use this for initialization
	void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
        if (controller == null)
        {
            Debug.Log("Controller Disconnected");
        }
        var device = SteamVR_Controller.Input((int)trackedObj.index);

        if(controller.GetTouch(touchpad))
        {
            axis = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
            moveVector = (transform.right * axis.x + transform.forward * axis.y) * Time.deltaTime;
            moveVector = new Vector3(moveVector.x, 0, moveVector.z);
            if (playerRig != null)
            {
                playerRig.Translate(moveVector);
            }
        }		
	}
}
