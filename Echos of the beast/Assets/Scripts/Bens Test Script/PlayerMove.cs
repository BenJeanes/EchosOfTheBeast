using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameObject anchor;
    private GameObject player;
    public GameObject progGo;
    public Camera c;
    bool moving;
    public float scalar;
    private float prog;
    

    void Awake()
    {
        moving = false;
        player = this.gameObject;
        scalar = 2f;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anchor.gameObject.SetActive(true);
            anchor.transform.position = c.ScreenToWorldPoint(Input.mousePosition);
            ClampY(anchor);
            moving = true;
            prog = 0;
        }
        if(Input.GetMouseButtonUp(0))
        {
            moving = false;            
            anchor.gameObject.SetActive(false);
            ClampY(anchor);
        }
        if(moving)
        {
            Vector3 dir = new Vector3(c.ScreenToWorldPoint(Input.mousePosition).x, 0.5f, c.ScreenToWorldPoint(Input.mousePosition).z);
            Vector3 distToMove = anchor.transform.position - player.transform.position;
            Debug.DrawLine(anchor.transform.position, player.transform.position, Color.blue);
            Debug.DrawRay(player.transform.position, anchor.transform.position - dir, Color.red);
            
            Debug.Log(prog);
            progGo.transform.position = distToMove * prog;

            //player.GetComponent<Rigidbody>().AddForce((anchor.transform.position - player.transform.position) * scalar);                  
        }
    }

    private void ClampY(GameObject o)
    {
        o.transform.position = new Vector3(o.transform.position.x, 0.5f, o.transform.position.z);
    }

    private void Lerp(Vector3 start, Vector3 end)
    {

    }

}
