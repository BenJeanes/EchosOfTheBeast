using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveHelper : MonoBehaviour {

    public GameObject player;
    public GameObject[] controllers; 

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = SetPositionToPlayerWaist(player.transform.position);
	}

    private Vector3 SetPositionToPlayerWaist(Vector3 playerPosition)
    {
        const float offset = 0.5f;
        Vector3 waistPos = new Vector3(playerPosition.x, playerPosition.y - offset, playerPosition.z);
        return waistPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "ViveController")
        {
            Debug.Log("Got One!");
        }
    }
}
