using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinotaurNav : MonoBehaviour
{
    //public variables

    //Minotaurs Movement Speed
    private int movementSpeed = 9;

    //Array of Front Raycast Positions
    [SerializeField]
    GameObject[] frontRaycast;

    //Array of Back Raycast Positions
    [SerializeField]
    GameObject[] backRaycast;

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
    public Vector3 targetLocation;

    //Hunting State Variables
    public float pauseTime = 3.0f;

    //State
    private bool huntingState = false;
    private bool chargingState = false;


    // Use this for initialization
    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        reversePatrolPoints = new GameObject[patrolPoints.Length];

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            reversePatrolPoints[patrolPoints.Length - 1 - i] = patrolPoints[i];
        }

        if (navMeshAgent == null)
        {
            Debug.LogError("The NavMeshAgent is not attached to " + gameObject.name);
        }
    }

    void Update()
    {
        Patrol();
        ListenForSound();
        Footstep();

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

    private void Footstep()
    {

    }

    //Patrol State
    private void Patrol()
    {
        if (patrolPoints.Length > 0 && huntingState == false)
        {
            navMeshAgent.SetDestination(patrolPoints[currentPatrolPoint].transform.position);
            if (transform.position == patrolPoints[currentPatrolPoint].transform.position || Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].transform.position) < 0.2f)
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
        //First Minotaur pauses at the hunt location
        float time = 0;
        while(time < pauseTime)
        {
            time += Time.deltaTime;
        }

        //Continue Hunt State
        if(time >= pauseTime)
        {
            //Collect/Generate Patrol Points nearby 


            //After Hunting, stop Hunting
            huntingState = false;
        }
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
            //Check if Sound Location (Location where player made the sound) is hit by the RayCasts before a wall (the player is in visual range/in the same corridor as the minotaur)
            foreach (GameObject position in frontRaycast)
            {
                RaycastHit hit;
                Physics.Raycast(position.transform.position, transform.TransformDirection(Vector3.forward), out hit);
                
                if(hit.collider.gameObject.name == "Player_Rig")
                {
                    //Break and Charge
                    chargingState = true;
                    break;
                }
            }

            foreach (GameObject position in backRaycast)
            {
                RaycastHit hit;
                Physics.Raycast(position.transform.position, transform.TransformDirection(Vector3.back), out hit);

                if (hit.collider.gameObject.name == "Player_Rig")
                {
                    //Break and Charge
                    chargingState = true;
                    break;
                }
            }

            if(chargingState == false)
            {
                //Start Hunting State
                huntingState = true;
                movementSpeed = 15;

                targetLocation = playerLocation;

                navMeshAgent.SetDestination(targetLocation);

                if (transform.position == targetLocation || Vector3.Distance(transform.position, targetLocation) < 0.2f)
                {
                    Hunt();
                }
            }
        }
    }
}
