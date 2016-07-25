using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Weight
{
	public float value;

	public delegate void Operator(float weight);

	private Dictionary<string, Operator> operations = new Dictionary<string, Operator>();

	public Weight(float value)
	{
		this.Init(value);
	}

	/// <summary>
	/// Credit: http://stackoverflow.com/a/11065781
	/// </summary>
	/// <param name="weight"></param>
	public static implicit operator float(Weight weight)
	{
		return weight.value;
	}

	/// <summary>
	/// Credit: http://stackoverflow.com/a/11065781
	/// </summary>
	/// <param name="weight"></param>
	public static implicit operator Weight(float weight)
	{
		return new Weight(weight);
	}

	public Operator Do(string operation)
	{
		return operations[operation];
	}

	public static bool operator >(Weight left, Weight right)
	{
		return (float)left > (float)right;
	}

	public static bool operator <(Weight left, Weight right)
	{
		return (float)left < (float)right;
	}

	public void Init(float value)
	{
		this.value = value;

		operations = new Dictionary<string, Operator>()
		{
			{ "Add", this.Add },
			{ "Remove", this.Remove },
			{ "Scale", this.Scale },
			{ "Distance", this.Distance }
		};
	}

	public float Inverse
	{
		get
		{
			return new Weight(1f / this.value);
		}
	}

	public void Scale(float weight)
	{
		this.value *= weight;
	}

	public void Add(float weight)
	{
		this.value += weight;
	}

	public void Remove(float weight)
	{
		this.Add(-weight);
	}

	public void Distance(float weight)
	{
		float result = Mathf.Abs(this.value - weight);

		value = result;
	}

	public Weight Scaled(float scale)
	{
		var result = this.Clone();

		result.Scale(scale);

		return result;
	}
}
