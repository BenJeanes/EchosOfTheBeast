using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlaceAnchor : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObj;
    public GameObject anchor;
    public GameObject player;
    public GameObject VR_PlayArea;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    bool moving = false;
    Vector3 startpos;
    float speed = 5.0f;
    

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();       
    }

    // Update is called once per frame
    void Update ()
    {
        Debug.DrawRay(player.transform.position, (anchor.transform.position - player.transform.position) * 10, Color.red);     
        if (Controller.GetHairTriggerDown())
        {            
            anchor.transform.position = this.transform.position;
            moving = true;
            startpos = VR_PlayArea.transform.position;
        }
        if (Controller.GetHairTriggerUp())
        {
            float step = speed * Time.deltaTime;
            VR_PlayArea.transform.position = Vector3.MoveTowards(startpos, anchor.transform.position - player.transform.position, step);
        }
        if(moving)
        {          
            
        }

    }
}
