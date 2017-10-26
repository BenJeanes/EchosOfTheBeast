using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlaceAnchor : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObj;
    public GameObject anchor;
    public GameObject player;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    bool moving = false;
    

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();       
    }

    // Update is called once per frame
    void Update ()
    {
        if (Controller.GetHairTriggerDown())
        {
            if (!moving)
            {
                moving = true;
                anchor.transform.position = Controller.transform.pos;
                Vector3 fromPlayerToAnchor = player.transform.position - anchor.transform.position;
                Debug.Log(string.Format("Movement vector = ({0},{1})", fromPlayerToAnchor.x, fromPlayerToAnchor.z));
            }
        }
        if (Controller.GetHairTriggerUp())
        {
            if(moving)
            {
                moving = false;
            }
        }

    }
}
