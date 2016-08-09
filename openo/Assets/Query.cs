//using UnityEngine;
//using System.Collections;

//public abstract class IQuery
//{
//	protected string query;

//	public class Result : Item.A
//	{

//	}

//	public abstract Set Get(string subquery = "");
//}


//public class RootQuery : IQuery
//{
//	object parent;

//	public RootQuery(string query, object parent)
//	{
//		this.parent = parent;
//		this.query = query;
//	}

//	public override Set Get(string subquery = "")
//	{
//		return null; //(new Result(parent)).Get(query).Get(subquery);
//	}
//}

//public class Query : IQuery
//{
//	Query parent;

//	public Query(string query, Query parent)
//	{
//		this.parent = parent;
//		this.query = query;
//	}

//	public override Set Get(string subquery = "")
//	{
//		return parent.Get(query).Get(subquery);
//	}
//}
