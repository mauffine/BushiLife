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
					  parent.value.GetType()
					  ))
		{

		}

		public f(string name, object function, openo parent)
			: base(
				  The.Method(
					  name,
					  parent.Type()
					  ))
		{

		}

		public override openo Get(object request)
		{
			object result;

			if (request is object[])
				result = The.Result(this.parent, (MethodInfo)this.value, (object[])request);
			else
				result = The.Result(this.parent, (MethodInfo)this.value, request);

			return new openo(result);
		}
	}

	public class tuple : openo
	{
		public tuple(params object[] values) : base(values)
		{
		}
	}


	/// <summary>
	/// Credit: http://stackoverflow.com/a/11065781
	/// </summary>
	public static implicit operator openo(object[] request)
	{
		return new tuple(request);
	}

	/// <summary>
	/// Credit: http://stackoverflow.com/a/11065781
	/// </summary>
	public static implicit operator openo(string request)
	{
		return new openo(request);
	}

	public static implicit operator string(openo o)
	{
		return o.value.ToString();
	}

	public static implicit operator int(openo o)
	{
		return System.Convert.ToInt32(o.value);
	}

	public static implicit operator float(openo o)
	{
		return System.Convert.ToSingle(o.value);
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

	/// <summary>
	/// reveal! (with this.Get)
	/// </summary>
	public static openo O(openo request)
	{
		return new openo(request);
	}

	public virtual openo Get(params object[] request)
	{
		return Get((openo)request);
	}

	public virtual openo Get(object request)
	{
		openo result = null;


		if (request is string)
		{
			var strRequest = (string)request;

			result = TryGetVar(strRequest);
			if (result != null)
				return result;

			result = TryGetFunction(strRequest);
			if (result != null)
				return result;

			int intRequest;

			if (int.TryParse(strRequest, out intRequest))
			{
				result = TryGetIndex(intRequest);
				if (result != null)
					return result;
			}
		}
		else if (request is int)
		{
			result = TryGetIndex((int)request);
			if (result != null)
				return result;
		}

		return null;
	}

	public virtual openo Get(openo request)
	{
		return Get(request.value);
	}

	private openo TryGetIndex(int name)
	{
		return null;
	}

	private openo TryGetFunction(string name)
	{
		var function = The.Method(name, value.GetType());

		if (function != null)
		{
			return new f(name, function, this);
		}

		return null;
	}

	private openo TryGetVar(string name)
	{
		var function = The.Method(name, value.GetType());

		if (function != null)
		{
			return new f(name, function, this);
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

	bool IsA(object o)
	{
		return The.Same(this, o);
	}

	bool IsA(string type)
	{
		return this.IsA(type.GetType());
	}


}

