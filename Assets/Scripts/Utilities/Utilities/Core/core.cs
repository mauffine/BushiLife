﻿using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

using MethodInfo = System.Reflection.MethodInfo;



public static class Utility<T>
{
	static Utility()
	{
		Create = Expression.Lambda<System.Func<T>>(Expression.New(typeof(T).GetConstructor(System.Type.EmptyTypes))).Compile();
	}
	public static System.Func<T> Create { get; private set; }
}

public class An
{
	public static T Instance<T>(params object[] parms)
	{
		object instance = null;

		var constructorInfo = typeof(T).GetConstructor(The.Types(parms));
		if (constructorInfo != null)
		{
			if (parms.Length == 0)
				return Utility<T>.Create();
			else
				instance = constructorInfo.Invoke(parms);
		}
		else
		{
			instance = default(T);
		}

		return (T)instance;
	}
}

public class A
{
	public static T New<T>(params object[] parms)
	{
		return An.Instance<T>(parms);
	}

	public delegate T Make<T>(params object[] parms);

	public static Make<State> State = New<State>;
	public static Make<Weight> Weight = New<Weight>;
	public static Make<State.Body> Body = New<State.Body>;
}

public class The
{
	private static Dictionary<string, System.Type> typeCache = new Dictionary<string, System.Type>();
	private static Dictionary<string, string> aliases = new Dictionary<string, string>();
	
	public static void Add(string alias, System.Type type)
	{
		aliases.Add(alias, type.FullName);
	}

	static The()
	{

	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="typeName"></param>
	/// <param name="_depth">For internal function recursion</param>
	/// <returns></returns>
	public static System.Type Type(string typeName, int _depth=0)
	{
		System.Type t = null;

		if (_depth > 10)
			throw new System.Exception("Too deep bro");

		if (aliases.ContainsKey(typeName))
		{
			return Type(aliases[typeName], _depth + 1);
		}

		lock (typeCache)
		{
			if (!typeCache.TryGetValue(typeName, out t))
			{
				foreach (System.Reflection.Assembly a in System.AppDomain.CurrentDomain.GetAssemblies())
				{
					t = a.GetType(typeName);
					if (t != null)
						break;
				}
				if (t == null)
					return Type("System." + typeName, _depth=10);
				typeCache[typeName] = t; // perhaps null
			}
		}
		
		return t;
	}

	public static object Method(string name, System.Type type)
	{
		var info = type.GetMethod(name,
			System.Reflection.BindingFlags.Instance
				| System.Reflection.BindingFlags.NonPublic
				| System.Reflection.BindingFlags.Static
				| System.Reflection.BindingFlags.Public);

		return info;
	}


	public static bool Same(System.Type a, System.Type b)
	{
		return (
					a.FullName == b.FullName)
					|| b.IsAssignableFrom(a)
					|| (a.IsAssignableFrom(b)
				);
	}

	public static bool Same(params System.Type[] types)
	{
		for (int i = 0; i < types.Length - 1; i++)
		{
			var isSame = false;
			for (int j = i + 1; i < types.Length; i++)
			{
				isSame = Same(types[i], types[j]);

				if (isSame)
					break;
			}

			if (!isSame)
				return false;
		}

		return true;
	}

	public static System.Type[] Types(params object[] objects)
	{
		var types = objects.Select(o => (Same(o.GetType(), typeof(System.Type)) ? (System.Type)o : o.GetType())).ToArray();

		return types;
	}

	public static bool Same(params object[] objects)
	{
		return Same(The.Types(objects));
	}

	public static object Result(object self, MethodInfo method, object[] parms)
	{
		return method.Invoke(self, parms);
	}

	public static openo O(params object[] request)
	{
		return Openo(request);
	}

	public static openo Openo(params object[] request)
	{
		return new openo.tuple(request);
	}

	public static openo Openo(object request)
	{
		return new openo(request);
	}
}
