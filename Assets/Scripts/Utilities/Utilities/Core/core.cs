using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using MethodInfo = System.Reflection.MethodInfo;

public class The
{
	private static Dictionary<string, System.Type> typeCache;

	public static System.Type Type(string typeName)
	{
		System.Type t = null;

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
		return (b.IsAssignableFrom(a)
				|| b == a)
			|| (a.IsAssignableFrom(b)
				|| a == b);
	}

	public static bool Same(params System.Type[] types)
	{
		for (int i = 0; i < types.Length - 1; i++)
		{
			var isSame = false;
			for (int j = i + 1; i < types.Length; i++)
			{
				isSame = Same(types[i], types[i]);

				if (isSame)
					break;
			}

			if (!isSame)
				return false;
		}

		return true;
	}

	public static bool Same(params object[] objects)
	{
		var types = objects.Select(o => Same(o.GetType(), typeof(System.Type)) ? o : o.GetType()).ToArray();

		return Same(types);
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
