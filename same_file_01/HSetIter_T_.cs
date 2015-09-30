using System;
using System.Collections;
using System.Collections.Generic;

public struct HSetIter<T> : IDisposable, IEnumerator, IEnumerator<T>
{
	private HashSet<T>.Enumerator enumerator;

	public T Current
	{
		get
		{
			return this.enumerator.Current;
		}
	}

	object System.Collections.IEnumerator.Current
	{
		get
		{
			return this.enumerator.Current;
		}
	}

	public HSetIter(HashSet<T>.Enumerator enumerator)
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

	public void Reset()
	{
		throw new NotSupportedException();
	}
}