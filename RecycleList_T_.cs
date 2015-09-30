using System;
using System.Collections;
using System.Collections.Generic;

public class RecycleList<T> : List<T>, IDisposable
{
	private bool bound;

	private static int binCount;

	private static LinkedList<RecycleList<T>> bin;

	static RecycleList()
	{
		RecycleList<T>.binCount = 0;
		RecycleList<T>.bin = new LinkedList<RecycleList<T>>();
	}

	internal RecycleList()
	{
	}

	public static void Bin(ref RecycleList<T> list)
	{
		if (list != null)
		{
			if (list.bound)
			{
				RecycleList<T>.bin.AddLast(list);
				list.bound = false;
			}
			list = null;
		}
	}

	public RecycleList<T> Clone()
	{
		return RecycleList<T>.Make<RecycleList<T>>(this);
	}

	public void Dispose()
	{
		RecycleList<T> recycleList = this;
		RecycleList<T>.Bin(ref recycleList);
	}

	public static RecycleList<T> Make()
	{
		RecycleList<T> recycleList;
		if (RecycleList<T>.binCount <= 0)
		{
			recycleList = new RecycleList<T>();
		}
		else
		{
			recycleList = RecycleList<T>.bin.First.Value;
			RecycleList<T>.bin.RemoveFirst();
			RecycleList<T>.binCount = RecycleList<T>.binCount - 1;
		}
		recycleList.bound = true;
		return recycleList;
	}

	public static RecycleList<T> Make<TClassEnumerable>(TClassEnumerable enumerable)
	where TClassEnumerable : class, IEnumerable<T>
	{
		RecycleList<T> recycleList = RecycleList<T>.Make();
		recycleList.AddRange(enumerable);
		return recycleList;
	}

	public static RecycleList<T> MakeFromValuedEnumerator<TEnumerator>(ref TEnumerator enumerator)
	where TEnumerator : struct, IEnumerator<T>
	{
		RecycleList<T> recycleList = RecycleList<T>.Make();
		while (enumerator.MoveNext())
		{
			recycleList.Add((T)enumerator.Current);
		}
		enumerator.Dispose();
		return recycleList;
	}

	public RecycleListIter<T> MakeIter()
	{
		return new RecycleListIter<T>(base.GetEnumerator());
	}

	public static RecycleList<T> MakeValueEnumerable<TStructEnumerable>(ref TStructEnumerable enumerable)
	where TStructEnumerable : struct, IEnumerable<T>
	{
		RecycleList<T> recycleList = RecycleList<T>.Make();
		recycleList.AddRange(enumerable);
		return recycleList;
	}
}