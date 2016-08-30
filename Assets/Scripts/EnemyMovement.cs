using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour {
	NavMeshAgent agent;
    AIController ai;
    public float maxLeadDist;

	void Start()
	{
	}

    public void Init(AIController ai)
    {
        this.agent = GetComponent<NavMeshAgent>();
        this.ai = ai;
    }

	void Update()
	{
		this.UpdateDestination();

        this.ai.target = this.transform.position;
        if ((this.ai.transform.position - this.transform.position).magnitude > this.maxLeadDist)
        {
            Teleport(this.ai.transform.position);
        }
	}

	void UpdateDestination()
	{
		var players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
		players.Sort((t1, t2) => SocketContainer.Closer(t1.transform, t2.transform, this.transform));

        if (this.agent.enabled)
    		this.agent.SetDestination(players[0].transform.position);
	}

    void Teleport(Vector3 position)
    {
        this.agent.enabled = false;
        this.transform.position = position;
        this.agent.enabled = true;
    }
}
