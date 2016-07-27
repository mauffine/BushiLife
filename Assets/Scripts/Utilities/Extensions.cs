using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq.Expressions;


/// <summary>
/// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
/// Provides a method for performing a deep copy of an object.
/// Binary Serialization is used to perform the copy.
/// </summary>
public static class ObjectExtensions
{
	/// <summary>
	/// Perform a deep Copy of the object.
	/// </summary>
	/// <typeparam name="T">The type of object being copied.</typeparam>
	/// <param name="source">The object instance to copy.</param>
	/// <returns>The copied object.</returns>
	public static T Clone<T>(this T source)
	{
		if (!typeof(T).IsSerializable)
		{
			throw new ArgumentException("The type must be serializable.", "source");
		}

		// Don't serialize a null object, simply return the default for that object
		if (System.Object.ReferenceEquals(source, null))
		{
			return default(T);
		}

		IFormatter formatter = new BinaryFormatter();
		Stream stream = new MemoryStream();
		using (stream)
		{
			formatter.Serialize(stream, source);
			stream.Seek(0, SeekOrigin.Begin);
			return (T)formatter.Deserialize(stream);
		}
	}

	public static bool IsA<T>(this T source, string stringType)
	{
		return The.Same(source.GetType(), The.Type(stringType));
		
	}
}

public static class Vector2Extensions
{
	public static IntVector2 ToIntVector2(this Vector2 vector2)
	{
		int[] intVector2 = new int[2];
		for (int i = 0; i < 2; ++i) intVector2[i] = Mathf.RoundToInt(vector2[i]);
		return new IntVector2(intVector2);
	}
}


public struct IntVector2
{
	public int x, y;

	public IntVector2(int[] raw)
	{
		this.x = raw[0];
		this.y = raw[1];
	}
}
