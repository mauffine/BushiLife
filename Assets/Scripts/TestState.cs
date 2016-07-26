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
		input = new State(1f, new State.Body(1f,
				"jump",
				"forwards",
				"backwards",
				0f,
					"left",
					"right"				
				)
			);

		if (input.Has(0.3f, "jump", "backwards"))
		{
			Debug.Log("Jump & Backwards");
		}

		if (input.Has(0.3f, "left", "right"))
		{
			Debug.Log("Left & Right");
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
