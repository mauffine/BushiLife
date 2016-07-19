using UnityEngine;
using System.Collections;

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
