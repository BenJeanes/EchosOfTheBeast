using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveControllerInputTest : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device controller
    {
        get
        {
            return SteamVR_Controller.Input((int)trackedObj.index);
        }
    }

    public Light lighting;

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
    
    void Update ()
    {
        if (controller.GetAxis() != Vector2.zero)
        {
            Debug.Log(gameObject.name + controller.GetAxis());
        }        
        if (controller.GetHairTriggerDown())
        {
            Debug.Log(gameObject.name + " Trigger Press");
            if(lighting != null)
            {
                lighting.gameObject.SetActive(true);
            }
        }        
        if (controller.GetHairTriggerUp())
        {
            Debug.Log(gameObject.name + " Trigger Release");
            if (lighting != null)
            {
                lighting.gameObject.SetActive(false);
            }
        }        
        if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log(gameObject.name + " Grip Press");
        }        
        if (controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log(gameObject.name + " Grip Release");
        }
    }
}
