using UnityEngine;
using System.Collections;
using System;

public abstract class Set
{
	public virtual int Size
	{
		get
		{
			return 0;
		}
	}

	public Set First
	{
		get
		{
			return this.Get(0);
		}
	}

	public Set Last
	{
		get
		{
			return this.Get(Size - 1);
		}
	}

	public abstract Set Get(int index);

	public virtual Set Get(string name)
	{
		if (name == "")
			return this;
		else
			throw new Exception("Invalid name");
	}
}