using UnityEngine;
using System;
using System.Collections;

public class Item
{
	public class A : Set
	{
		public A(object value)
			: base(value)
		{

		}
	}

	public class Many : Set
	{
		public Many(params object[] many)
			: base(many)
		{

		}

		public Many(int number, params object[] many)
			: base(many.RangeSubset(0, number))
		{

		}

		/// <summary>
		/// Credit: http://stackoverflow.com/a/11065781
		/// </summary>
		public static implicit operator object[] (Many value)
		{
			return (object[])value.value;
		}
		
		/// <summary>
		/// Credit: http://stackoverflow.com/a/11065781
		/// </summary>
		public static implicit operator Many(object[] value)
		{
			return new Many(value);
		}

		public Many Subset(int[] indexes)
		{

		}

		public Many Subset(int start, int finish)
		{

		}

		public Many[]
	}

	public class Couple : Many
	{
		public openo Second
		{
			get
			{
				return this.o(1);
			}
		}

		public Couple(params object[] couple)
			: base(2, couple)
		{

		}

		public Couple(object value)
			: base(2, new object[] { value, value })
		{

		}
	}
}
