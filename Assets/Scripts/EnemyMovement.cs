using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour {
	NavMeshAgent agent;

	void Start()
	{
		this.agent = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		this.UpdateDestination();
	}

	void UpdateDestination()
	{
		var players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
		players.Sort((t1, t2) => SocketContainer.Closer(t1.transform, t2.transform, this.transform));

		this.agent.SetDestination(players[0].transform.position);
	}
}
