using UnityEngine;
using System.Collections;

/// <summary>
/// open-object: a wrapper to peel open a .NET object, 
/// revealing its members as strings with reflection
/// </summary>
public class openo
{
	object value;

	/// <summary>
	/// reveal! (with this.Get)
	/// </summary>
	openo o(string name)
	{
		return this.Get(name);
	}

	openo Get(string name)
	{
		{
			int index = 0;

			if (int.TryParse(name, out index))
			{
				return Get(index);
			}
		}
	}

	openo Get(int index)
	{
		// TODO
	}
}
