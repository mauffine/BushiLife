using UnityEngine;
using System.Collections;
using System;

public class Item
{
	public class A : Set
	{
		public openo value;

		public override int Size
		{
			get
			{
				return 1;
			}
		}

		public override Set Get(int index)
		{
			if (index == 0)
				return this;
			else
				throw new IndexOutOfRangeException("Item.A is only one item");
		}
	}
}
