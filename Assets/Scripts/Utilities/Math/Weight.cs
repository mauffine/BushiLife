using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Weight
{
	public float value;

	public delegate void Operator(Weight weight);

	private Dictionary<string, Operator> operations = new Dictionary<string, Operator>();

	public Weight(float value)
	{
		this.Init(value);
	}

	public Weight(Weight value)
	{
		this.Init(value.value);
	}

	public Operator Do(string operation)
	{
		return operations[operation];
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

	public Weight Inverse
	{
		get
		{
			return new Weight(1f / this.value);
		}
	}

	public static Weight Make(float value)
	{
		Weight newWeight = new Weight(value);

		return newWeight;
	}

	public static Weight Make(object value)
	{
		Weight newWeight = null;

		if (The.Same(value.GetType(), typeof(Weight)))
		{
			newWeight = new Weight(value as Weight);
		}
		else
		{
			var strValue = value.ToString();
			var flValue = float.Parse(strValue);

			newWeight = new Weight(flValue);
		}

		return newWeight;
	}

	public void Scale(Weight weight)
	{
		this.Scale(weight.value);
	}

	public void Scale(float weight)
	{
		this.value *= weight;
	}

	public void Add(Weight weight)
	{
		this.Add(weight.value);
	}

	public void Add(float weight)
	{
		this.value += weight;
	}

	public void Remove(Weight weight)
	{
		this.Add(weight.value);
	}

	public void Remove(float weight)
	{
		this.Add(-weight);
	}

	public void Distance(Weight other)
	{
		float result = Mathf.Abs(this.value - other.value);

		value = result;
	}
}
