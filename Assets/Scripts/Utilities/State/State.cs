using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class State
{

	Weight value;
	public Dictionary<string, State> body;

	public bool IsEmpty
	{
		get
		{
			return this.body.Keys.Count == 0;
		}
	}

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

	public State(float weight, Dictionary<string, State> state)
	{
		this.Init(weight, state);
	}

	public State(Weight weight, Dictionary<string, State> state)
	{
		this.Init(weight, state);
	}

	public void Init(Weight weight, Dictionary<string, State> state)
	{
		this.value = weight;
		this.body = state;
	}

	public void Init(float weight, Dictionary<string, State> state)
	{
		Init(new Weight(weight), state);
	}

	/// </summary>
	/// <returns>null if name is invalid.</returns>
	/// <summary>
	public State Get(string name)
	{
		var result = this.RawGet(name);

		if (result != null)
		{
			return result.Scaled(value);
		}

		return result;
	}

	/// </summary>
	/// <returns>null if name is invalid.</returns>
	/// <summary>
	private State RawGet(string name)
	{
		if (this.body.ContainsKey(name))
		{
			return this.body[name];
		}
		else
		{ 
			return null;
		}
	}

	public bool Has(float accuracy, State state)
	{
		state.value.Scale(accuracy);

		//TODO

		return false;

	}

	public void Add(string name, State subState)
	{
		Apply(name, subState, "Add");
	}

	public void Apply(string name, State subState, string operation)
	{
		var state = this.RawGet(name);

		if (state != null)
		{
			state.Apply(subState, "Add");
		}
		else
		{
			var newState = new State(0f, new Dictionary<string, State>());

			this.body.Add(name, newState);

			this.Apply(name, subState, operation);
		}
	}

	public void Add(State other)
	{
		Apply(other, "Add");
	}

	public void Apply(State other, string operation)
	{
		this.value.Do(operation)(other.value);

		if (!other.IsEmpty)
		{
			var otherBody = other.ScaledBody(this.value.Inverse);

			AddBody(otherBody);
		}
	}

	void AddBody(List<KeyValuePair<string, State>> body)
	{
		foreach (KeyValuePair<string, State> namedState in body)
		{
			Add(namedState.Key, namedState.Value);
		}
	}

	void ApplyBody(List<KeyValuePair<string, State>> body, string operation)
	{
		foreach (KeyValuePair<string, State> namedState in body)
		{
			Apply(namedState.Key, namedState.Value, operation);
		}
	}



	public void Subtract(State other)
	{

	}

	public static State Add(State left, State right)
	{
		left = left.Clone();

		left.Add(right);

		return left;
	}

	//public static State Intersect(State left, State right)
	//{
	//	var intersection = left.Add(right);
	//}

	public void Scale(Weight scale)
	{
		this.value.Scale(scale);
	}

	public List<KeyValuePair<string, State>> ScaledBody(Weight scale)
	{
		scale.Scale(this.value);

		var result = new List<KeyValuePair<string, State>>(this.body.Keys.Count);

		foreach (KeyValuePair<string, State> namedState in this.body)
		{
			var newState = namedState.Clone();
			newState.Value.Scale(scale);

			result.Add(newState);
		}

		return result;
	}

	public State Scaled(Weight scale)
	{
		var result = this.Clone();

		result.Scale(scale);

		return result;
	}
}
