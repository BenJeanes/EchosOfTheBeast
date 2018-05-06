using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnRoof : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
