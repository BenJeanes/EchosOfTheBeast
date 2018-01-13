using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinotaurNav : MonoBehaviour
{
    //public variables
    public GameObject playerGO;

    //Minotaurs Movement Speed
    private float movementSpeed = 1.5f;

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
    public float soundSensitivity = 0.5f;
    public bool soundHeard = false;
    public float distanceToPlayer;
    public Vector3 targetLocation;
    private float playerSoundLevel;
	public float soundLevel;

    //Hunting State Variables
    public float time;
    public float pauseTime = 300.0f;

    //State
    public bool huntingState = false;
    public bool chargingState = false;
	
	//Sound Effects
	public AudioClip roar;
	public AudioClip footstep;
	private AudioSource source;

    private float stepInterval = 1.5f;

    // Use this for initialization
    void Start()
    {
        //Get References to the NavMeshAgent Component
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        playerGO = GameObject.Find("Player_Rig");
        
		//Get Reference to the AudioSource Component
		source = this.GetComponent<AudioSource>();

        //Assign the Reverse Patrol Points Array
        reversePatrolPoints = new GameObject[patrolPoints.Length];

        //Assign the Reverse Patrol Points as a reversed copy of the patrolPoints Array
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            reversePatrolPoints[patrolPoints.Length - 1 - i] = patrolPoints[i];
        }		
		//Start Footstep Noise
		StartCoroutine(Footstep());
    }

    void Update()
    {
        //Check for Player Sounds
        ListenForSound();
        
        //If the Minotaur doesn't hear anything, Patrol
        Patrol();

        //If Player Spotted, Charge at them
        Charge();
    }
	
	IEnumerator Footstep()
	{
		while(true)
		{
			source.clip = footstep;
			source.PlayOneShot(footstep);
            playerGO.GetComponentInChildren<EchoManager>().CreateEchoEffect(this.transform.position, 0.5f);
			yield return new WaitForSeconds(stepInterval);
            playerGO.GetComponentInChildren<EchoManager>().CreateEchoEffect(this.transform.position, 0.5f);
            yield return new WaitForSeconds(stepInterval);
		}
	}

    //Patrol State
    private void Patrol()
    {
        //If the Minotaur isn't hunting down a sound
        if (patrolPoints.Length > 0 && huntingState == false)
        {
            //Set Patrol Speed
            navMeshAgent.speed = 1.5f;
            stepInterval = 1.5f;
            //Go to the next Patrol Point
            navMeshAgent.SetDestination(patrolPoints[currentPatrolPoint].transform.position);

            //If at the patrol point or close enough to it
            if (transform.position == patrolPoints[currentPatrolPoint].transform.position || Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].transform.position) < 2f)
            {
                //Set next patrol point
                currentPatrolPoint++;
            }

            //If the Minotaur reaches the final patrol point
            if (currentPatrolPoint >= patrolPoints.Length)
            {
                //Copy over the Patrol and Reverse Patrol Point Arrays so the Minotaur reverses his path
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
        time++;

        //Continue Hunt State
        if(time >= pauseTime)
        {
            //Collect/Generate Patrol Points nearby 


            //After Hunting, stop Hunting
            time = 0;
            huntingState = false;
            soundHeard = false;
        }
    }

    //Charge State
    private void Charge()
    {
        //Do a check on if the Minotaur is actually charging
        if (chargingState == true)
        {
            //Get Players CURRENT Position
            Vector3 playerLocation = (GameObject.Find("Player_Rig")).transform.position;

            //Set Charge Speed
            navMeshAgent.speed = 1.5f;

            //Set target location = to the players location when charging started
            targetLocation = playerLocation;

            //Move to the target location
            navMeshAgent.SetDestination(targetLocation);

            //If at or near enough to the target location
            if (transform.position == targetLocation || Vector3.Distance(transform.position, targetLocation) < 2f)
            {
                //Stop charging
                chargingState = false;
            }
        }
    }

    private void ListenForSound()
    {
		Debug.Log("Listening");       

        //What is the 0-1f value of the players microphone input
        //playerSoundLevel = playerGO.GetComponentInChildren<MicrophoneInput>().SoundLevel;

        //The Vector3 location of the player
        Vector3 playerLocation = (GameObject.Find("Player_Rig")).transform.position;

        //How far away is the Minotaur from the Player
        distanceToPlayer = Vector3.Distance(transform.position, playerLocation);

        //The sound level of the player from the minotaurs location
        //float soundLevel = playerSoundLevel * Vector3.Distance(transform.position, playerLocation);

        soundLevel = playerGO.GetComponentInChildren<MicrophoneInput>().SoundLevel * Vector3.Distance(transform.position, playerLocation);
        //If the Sound Level is above the Minotaurs Sound Sensitivity AND the minotaur isnt charging OR if the Minotaur already has heard a sound AND isnt charging
        if (soundLevel >= soundSensitivity && chargingState == false || soundHeard == true && chargingState == false)
        {
            //If the minotaur hasnt already heard a sound
            if (soundHeard == false)
            {
				//Play Roar
				source.clip = roar;
				source.Play();
			
                //Check if Sound Location (Location where player made the sound) is hit by the RayCasts before a wall (the player is in visual range/in the same corridor as the minotaur)
                foreach (GameObject raycastOrigin in frontRaycast)
                {                    
                    RaycastHit hit;
                    Physics.Raycast(raycastOrigin.transform.position, transform.TransformDirection(Vector3.forward) * 10, out hit);
                    if(hit.collider != null)
                    {
                        if(hit.collider.gameObject.name == "Player_Rig")
                        {
                            chargingState = true;
                            break;
                        }
                    }					
                }
				
                //foreach (GameObject position in backRaycast)
                //{
                //    RaycastHit hit;
                //    Physics.Raycast(position.transform.position, transform.TransformDirection(Vector3.back), out hit);

                //    if (hit.collider.gameObject.name == "Player_Rig")
                //    {
                //        //Break and Charge
                //        chargingState = true;
                //        break;
                //    }
                //}
            }
			
            //Set the sound heard to true
            soundHeard = true;

            //if the Minotaur isnt charging or has already heard a sound, move to the sound location
            if (chargingState == false || soundHeard == true)
            {
                //Start Hunting State
                huntingState = true;

                //Set hunting speed
                navMeshAgent.speed = 15f;
                stepInterval = 0.8f;
                //Set target location as the location of the player when the sound was made
                targetLocation = playerLocation;

                //Move to the target location
                navMeshAgent.SetDestination(targetLocation);

                //If the Minotaur is close to or at the target location, start Hunt()
                if (transform.position == targetLocation || Vector3.Distance(transform.position, targetLocation) < 2f)
                {
                    Hunt();
                }
            }
        }
    }
}
