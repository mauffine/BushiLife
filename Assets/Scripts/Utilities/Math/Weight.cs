using UnityEngine;
using System.Collections;

[System.Serializable]
public class Weight
{
	public float value;

	public Weight(float value)
	{
		this.value = value;
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
}
