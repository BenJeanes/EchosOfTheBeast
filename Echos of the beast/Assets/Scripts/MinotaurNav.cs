using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinotaurNav : MonoBehaviour 
{
    //public variables

    //Minotaurs Movement Speed
    private int movementSpeed = 9;

	//Destination Target
	[SerializeField]
	Transform destination;

	//Array of GameObjects that make up the Patrol Route
	[SerializeField]
	GameObject[] patrolPoints;

    //Array that contains patrolPoints[] reversed
    private GameObject[] reversePatrolPoints;
	public int currentPatrolPoint;

    //NavMeshAgent, controls the movement of the Minotaur
    private NavMeshAgent navMeshAgent;

    //Variables for Sound Detection
    public float soundSensitivity = 50f;
    public float distanceToPlayer;

    //State
    public bool huntingState = false;
    

	// Use this for initialization
	void Start () 
	{
		navMeshAgent = this.GetComponent<NavMeshAgent>();
		reversePatrolPoints = new GameObject [patrolPoints.Length];

		for (int i = 0; i < patrolPoints.Length; i++)
		{
			reversePatrolPoints [patrolPoints.Length - 1 - i] = patrolPoints [i];
		}

		if (navMeshAgent == null) 
		{
			Debug.LogError ("The NavMeshAgent is not attached to " + gameObject.name);
		} 
	}

	void Update()
	{
		Patrol();
        ListenForSound();

        Debug.Log(Vector3.Distance(transform.position, (GameObject.Find("Player_Rig")).transform.position));
	}
	
//	private void SetDestination()
//	{
//		if (destination != null) 
//		{
//			Vector3 targetVector = destination.transform.position;
//			navMeshAgent.SetDestination (targetVector);
//		}
//	}

    //Patrol State
	private void Patrol()
	{
        if (patrolPoints.Length > 0 && huntingState == false) 
		{
			navMeshAgent.SetDestination (patrolPoints [currentPatrolPoint].transform.position);
			if (transform.position == patrolPoints [currentPatrolPoint].transform.position || Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].transform.position) < 0.2f) 
			{
				currentPatrolPoint++;
			}

			if (currentPatrolPoint >= patrolPoints.Length) 
			{
				GameObject[] temp = patrolPoints;

				patrolPoints = null;

				patrolPoints = reversePatrolPoints;

				reversePatrolPoints = temp;

				currentPatrolPoint = 0;
			}
		}
	}

    //Hunt State
    private void Hunt()
    {
        //Code here

        //After Hunting, stop Hunting
        huntingState = false;
    }

    private void ListenForSound()
    {
        //What is the 0-1f value of the players microphone input
        float playerSoundLevel = MicrophoneInput.normalizedMicrophoneInput;

        //The Vector3 location of the player
        Vector3 playerLocation = (GameObject.Find("Player_Rig")).transform.position;

        //How far away is the Minotaur from the Player
        distanceToPlayer = Vector3.Distance(transform.position, playerLocation);

        //The sound level of the player from the minotaurs location
        float soundLevel = playerSoundLevel * Vector3.Distance(transform.position, playerLocation);

        if (soundLevel >= soundSensitivity)
        {
            //Start Hunting State
            huntingState = true;
            movementSpeed = 15;

            navMeshAgent.SetDestination(playerLocation);
            if (transform.position == playerLocation || Vector3.Distance(transform.position, playerLocation) < 0.2f)
            {
                Hunt();
            }
        }
    }
}
