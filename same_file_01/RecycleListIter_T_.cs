using System;
using System.Collections.Generic;

public struct RecycleListIter<T>
{
	private List<T>.Enumerator enumerator;

	public T Current
	{
		get
		{
			return this.enumerator.Current;
		}
	}

	internal RecycleListIter(List<T>.Enumerator enumerator)
	{
		this.enumerator = enumerator;
	}

	public void Dispose()
	{
		this.enumerator.Dispose();
	}

	public bool MoveNext()
	{
		return this.enumerator.MoveNext();
	}
}