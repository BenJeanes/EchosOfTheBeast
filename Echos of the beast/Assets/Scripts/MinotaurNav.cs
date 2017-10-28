using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinotaurNav : MonoBehaviour 
{
	//public variables
	[SerializeField]
	Transform destination;

	private NavMeshAgent navMeshAgent;


	// Use this for initialization
	void Start () 
	{
		navMeshAgent = this.GetComponent<NavMeshAgent>();

		if (navMeshAgent == null) {
			Debug.LogError ("The NavMeshAgent is not attached to " + gameObject.name);
		} 
		else 
		{
			SetDestination ();
		}
	}
	
	private void SetDestination()
	{
		if (destination != null) 
		{
			Vector3 targetVector = destination.transform.position;
			navMeshAgent.SetDestination (targetVector);
		}
	}
}
