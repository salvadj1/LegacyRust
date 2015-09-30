using System;
using System.Collections.Generic;

public class HSet<T> : HashSet<T>
{
	private static HSet<T> temp;

	static HSet()
	{
		HSet<T>.temp = new HSet<T>();
	}

	public HSet()
	{
	}

	public HSet(IEnumerable<T> collection) : base(collection)
	{
	}

	public HSet(IEqualityComparer<T> comparer) : base(comparer)
	{
	}

	public HSet(IEnumerable<T> collection, IEqualityComparer<T> comparer) : base(collection, comparer)
	{
	}

	public RecycleList<T> ExceptList(IEnumerable<T> exceptWith)
	{
		RecycleList<T> list = null;
		try
		{
			HSet<T>.temp.UnionWith(this);
			HSet<T>.temp.ExceptWith(exceptWith);
			list = HSet<T>.temp.ToList();
		}
		finally
		{
			HSet<T>.temp.Clear();
		}
		return list;
	}

	public new HSetIter<T> GetEnumerator()
	{
		return new HSetIter<T>(base.GetEnumerator());
	}

	public RecycleList<T> IntersectList(IEnumerable<T> intersectWith)
	{
		RecycleList<T> list = null;
		try
		{
			HSet<T>.temp.UnionWith(this);
			HSet<T>.temp.IntersectWith(intersectWith);
			list = HSet<T>.temp.ToList();
		}
		finally
		{
			HSet<T>.temp.Clear();
		}
		return list;
	}

	public RecycleList<T> OperList(HSetOper oper, IEnumerable<T> collection)
	{
		switch (oper)
		{
			case HSetOper.Union:
			{
				return this.UnionList(collection);
			}
			case HSetOper.Intersect:
			{
				return this.IntersectList(collection);
			}
			case HSetOper.Except:
			{
				return this.ExceptList(collection);
			}
			case HSetOper.SymmetricExcept:
			{
				return this.SymmetricExceptList(collection);
			}
		}
		throw new ArgumentException(string.Concat("Don't know what to do with ", oper), "oper");
	}

	public RecycleList<T> SymmetricExceptList(IEnumerable<T> exceptWith)
	{
		RecycleList<T> list = null;
		try
		{
			HSet<T>.temp.UnionWith(this);
			HSet<T>.temp.SymmetricExceptWith(exceptWith);
			list = HSet<T>.temp.ToList();
		}
		finally
		{
			HSet<T>.temp.Clear();
		}
		return list;
	}

	private RecycleList<T> ToList()
	{
		HSetIter<T> enumerator = this.GetEnumerator();
		return RecycleList<T>.MakeFromValuedEnumerator<HSetIter<T>>(ref enumerator);
	}

	public RecycleList<T> UnionList(IEnumerable<T> unionWith)
	{
		RecycleList<T> list = null;
		try
		{
			HSet<T>.temp.UnionWith(this);
			HSet<T>.temp.UnionWith(unionWith);
			list = HSet<T>.temp.ToList();
		}
		finally
		{
			HSet<T>.temp.Clear();
		}
		return list;
	}
}