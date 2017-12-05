using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 10.0f;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 60.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        Debug.DrawLine(transform.position, transform.forward, Color.blue);
	}
}
