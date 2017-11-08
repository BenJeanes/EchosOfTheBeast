using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameObject anchor;
    private GameObject player;
    public Camera c;
    bool moving;    

    void Awake()
    {
        moving = false;
        player = this.gameObject;        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anchor.gameObject.SetActive(true);            
            moving = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            moving = false;            
            anchor.gameObject.SetActive(false);            
        }
        if(moving)
        {
            anchor.transform.position = ClampY(c.ScreenToWorldPoint(Input.mousePosition));                      
            player.transform.position = Vector3.MoveTowards(player.transform.position, anchor.transform.position, 0.1f);                      
        }
    }
    private Vector3 ClampY(Vector3 v)
    {
        v = new Vector3(v.x, 0.5f, v.z);
        return v;
    }
}
