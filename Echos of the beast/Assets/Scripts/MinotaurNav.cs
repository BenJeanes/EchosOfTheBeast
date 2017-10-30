using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinotaurNav : MonoBehaviour 
{
	//public variables

	//Destination Target
	[SerializeField]
	Transform destination;

	//Array of GameObjects that make up the Patrol Route
	[SerializeField]
	GameObject[] patrolPoints;

	//Array that contains patrolPoints[] reversed
	private GameObject[] reversePatrolPoints;

	public int currentPatrolPoint;
	private NavMeshAgent navMeshAgent;


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
		Patrol ();
	}
	
//	private void SetDestination()
//	{
//		if (destination != null) 
//		{
//			Vector3 targetVector = destination.transform.position;
//			navMeshAgent.SetDestination (targetVector);
//		}
//	}

	private void Patrol()
	{
		if (patrolPoints.Length > 0) 
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
}
