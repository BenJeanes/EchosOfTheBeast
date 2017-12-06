using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
       

	}

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Minotaur")
        {
            SceneManager.LoadScene("LoseScreen");
        }
        else if (collision.gameObject.tag == "Stairs")
        {
            SceneManager.LoadScene("WinScreen");
        }
    }
}
