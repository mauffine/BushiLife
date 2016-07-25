using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using MethodInfo = System.Reflection.MethodInfo;

/// <summary>
/// open-object: a wrapper to peel open a .NET object, 
/// revealing its members as strings with reflection
/// </summary>
public class openo
{
	public object value;

	/// <summary>
	/// open-function!
	/// </summary>
	public class f : openo
	{
		public string name;
		public openo parent;

		public f(string name, openo parent)
			: base(
				  The.Method(
					  name,
					  parent.Type()
					  ))
		{

		}

		public override openo Get(openo request)
		{
			var result = The.Result(this.parent, (MethodInfo)this.value, (object[])request.value);

			return new openo(result);
		}
	}

	public class tuple : openo
	{
		public tuple(object[] values) : base(values)
		{

		}
	}

	public openo(object value)
	{
		this.value = value; 
	}

	/// <summary>
	/// reveal! (with this.Get)
	/// </summary>
	public virtual openo o(openo request)
	{
		return this.Get(request);
	}

	public virtual openo Get<T>(T request)
	{
		if (The.Same(request, typeof(openo)))
		{
			return this.Get(request as openo);
		}
		else if (The.Same(request, typeof(string)))
		{
			return this.Get(request as string);
		}
		else
		{
			return null;
		}
	}

	public virtual openo Get(openo request)
	{
		if (request.IsA("string"))
		{
			return this.Get((string)request.value);
		}
		else //TODO: if (request.IsA("int"))
		{
			return null;
		}
	}

	public virtual openo Get(string name)
	{
		{
			int index = 0;

			if (int.TryParse(name, out index))
			{
				return this.Get(index);
			}
		}

		return null;
	}

//public virtual openo Get(int index)
//{
//	// TODO
//}

	public System.Type Type()
	{
		return this.value.GetType();
	}

	//public f Method(string name)
	//{
	//
	//}

	bool IsA(System.Type type)
	{
		return type.IsAssignableFrom(this.Type()) || type == this.Type();
	}

	bool IsA(string type)
	{
		return this.IsA(type.GetType());
	}


}

