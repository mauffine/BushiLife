using UnityEngine;
using System.Collections;

public class Location
{
	public Vector3 position = Vector3.zero;
	public Quaternion rotation = Quaternion.identity;
	public Vector3 scale = Vector3.one;

	public static Location FromTransform(Transform transform)
	{
		var result = new Location();

		result.position = transform.position;
		result.rotation = transform.rotation;
		result.scale = transform.localScale;

		return result;
	}
}
