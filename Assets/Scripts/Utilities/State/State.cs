using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class State
{
	Weight weight;
	public Dictionary<string, State> state;

	public State(object weight, Dictionary<string, State> state)
	{
		if (The.Same(weight.GetType(), typeof(Weight)))
		{
			this.Init(weight as Weight, state);
		}
		else
		{
			this.Init(float.Parse(weight.ToString()), state);
		}
	}

	public void Init(Weight weight, Dictionary<string, State> state)
	{
		this.weight = weight;
		this.state = state;
	}

	public void Init(float weight, Dictionary<string, State> state)
	{
		Init(new Weight(weight), state);
	}

	public bool Has(float accuracy, State state)
	{
		state.weight.Scale(accuracy);

//TODO

		return false;

	}

	public void Add(State other)
	{
		foreach (var state in other.state)
		{

		}
	}

	public static State Add(State left, State right)
	{
		left = left.Clone();

		left.Add(right);

		return left;
	}
}
