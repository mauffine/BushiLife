using UnityEngine;
using System.Collections;

public class PointNClickMovement : MonoBehaviour {
	NavMeshAgent agent;

	void Start()
	{
        this.agent = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		RaycastHit hit;
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
                this.agent.SetDestination(hit.point);

		}
	}
}
