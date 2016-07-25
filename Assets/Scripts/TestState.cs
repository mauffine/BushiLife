using UnityEngine;
using System.Collections;

public class TestState : MonoBehaviour
{
	State input;
	State action;

	// Use this for initialization
	void Start()
	{
		action = new State(1f, new State.Body(0f,
				"running", 
				"attacking",
				"defending",
				"idle", 1f
				)
			);
		input = new State(1f, new State.Body(0f,
				"jump",
				"forwards",
				"backwards",
				"left",
				"right"				
				)
			);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
