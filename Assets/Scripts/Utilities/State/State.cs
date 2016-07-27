using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class State : Weight
{
	[System.Serializable]
	public class Body : Dictionary<string, State>
	{
		public string defaultOperator = "Add";

		public static Body Empty
		{
			get
			{
				return new Body();
			}
		}

		public Body(params object[] args)
		{
			PushMany(args);
		}

		public Body()
		{
		}

		public void PushMany(params object[] args)
		{
			PushMany("", 1f, new Queue<object>(args));
		}

		private void PushMany(string name, State state, Queue<object> args)
		{
			Push(name, state);

			if (args.Peek().IsA("string"))
			{
				name = (string)args.Dequeue();	
			}
			else
			{
				state = (State)((Weight)args.Dequeue());
			}

			PushMany(name, state, args);
		}

		public void Push(string name, State value)
		{
			if (name != "")
				this.Add(name, value);
		}

		public void Push(string name)
		{
			if (name != "")
				this.Add(name, 1f);
		}

		public void Apply(string name, State value, string strOperator="Add")
		{
			if (name != "")
			{
				if (this.ContainsKey(name))
				{
					this[name].Do(strOperator)(value);
				}
			}
		}
	}

	public Body body = Body.Empty;

	public void Clamp(Weight min, Weight max)
	{
		foreach (var namedSubstate in body)
		{
			if (namedSubstate.Value < min || namedSubstate.Value > max)
			{
				body.Remove(namedSubstate.Key);
			}
		}
	}

	public bool IsEmpty
	{
		get
		{
			return this.body.Keys.Count == 0;
		}
	}

	public State(float weight, Body body) : base(weight)
	{
		this.Init(body);
	}

	/// <summary>
	/// Credit: http://stackoverflow.com/a/11065781
	/// </summary>
	/// <param name="weight"></param>
	public static implicit operator State(float weight)
	{
		return new State(weight);
	}

	public State(float weight) : base(weight)
	{
	}

	public new State Scaled(float scale)
	{
		return (State)base.Scaled(scale);
	}

	public void Init(Body body)
	{
		this.body = body;
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

	public bool Has(Vector2 accuracyRange, State state)
	{
		var diff = State.Apply(state, this, "Remove");

		diff.Clamp(accuracyRange[0], accuracyRange[1]);

		// this has state if state is in this (with accuracy of accuracyRange)
		return diff.body.Count == state.body.Count;
	}

	public bool Has(Vector2 accuracyRange, params object[] body)
	{
		var state = new State(1f, new State.Body(body));

		return Has(accuracyRange, state);
	}

	public bool Has(Weight accuracy, params object[] body)
	{
		var state = new State(1f, new State.Body(body));

		return Has(accuracy, state);
	}

	public bool Has(Weight accuracy, State state)
	{
		return this.Has(new Vector2(-accuracy, accuracy), state);
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
			state.Apply(subState, operation);
		}
		else
		{
			State newState = 0f;

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
		this.Do(operation)(other.value);

		if (!other.IsEmpty)
		{
			var otherBody = other.ScaledBody(this.Inverse);

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
		this.Apply(other, "Remove");
	}

	public static State Add(State left, State right)
	{
		left = left.Clone();

		left.Add(right);

		return left;
	}

	public static State Apply(State left, State right, string operation)
	{
		left = left.Clone();

		left.Apply(right, operation);

		return left;
	}

	public List<KeyValuePair<string, State>> ScaledBody(float scale)
	{
		scale *= value;

		var result = new List<KeyValuePair<string, State>>(this.body.Keys.Count);

		foreach (KeyValuePair<string, State> namedState in this.body)
		{
			var newState = namedState.Clone();
			newState.Value.Scale(scale);

			result.Add(newState);
		}

		return result;
	}
}
