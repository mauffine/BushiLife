using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DynamicGrid : MonoBehaviour
{
	public List<Vector2> sizes = new List<Vector2>();

	//Unity doesn't know how to serialize a Dictionary
	private Dictionary<int, Vector2> _sizes = new Dictionary<int, Vector2>();
	public int size;

	void Awake()
	{
		_sizes = new Dictionary<int, Vector2>();

		foreach (var size in this.sizes)
		{
			var intSize = size.ToIntVector2();

			this.Add(intSize.x, intSize.y);
		}
	}

	public void Add(int width, int height)
	{
		var newSize = width * height;
		var dimensions = new Vector2(width, height);

		if (_sizes.ContainsKey(width * height))
		{
			_sizes[newSize] = dimensions;
		}
		else
		{
			_sizes.Add(newSize, dimensions);
		}
	}

	public Rect GetRect(int index)
	{
		var dimensions = this._sizes[this.size].ToIntVector2();

		var rect = new Rect(
			GetPos(index), 
			CellSize());

		return rect;
	}

	private Vector2 CellSize()
	{
		var dimensions = this._sizes[this.size];

		return new Vector2(1f / dimensions.x, 1f / dimensions.y);
	}

	private Vector2 GetPos(int index)
	{
		var gridSize = this._sizes[this.size].ToIntVector2();

		int columns = gridSize.x;
		int rows = gridSize.y;

		int row = (index - (index % columns)) / columns;
		int column = index - (row * columns);

		return new Vector2((float)column / (float)columns,
			(float)row / (float)rows);
	}
}
