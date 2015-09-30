using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ODBGenericEnumerable<T, TEnumerator, TEnumerable> : IDisposable, IEnumerable, IEnumerable<T>
where T : UnityEngine.Object
where TEnumerator : struct, ODBEnumerator<T>
where TEnumerable : struct, ODBEnumerable<T, TEnumerator>
{
	private TEnumerable enumerable;

	private ODBGenericEnumerable<T, TEnumerator, TEnumerable> next;

	private bool disposed;

	private static ODBGenericEnumerable<T, TEnumerator, TEnumerable> recycle;

	static ODBGenericEnumerable()
	{
	}

	private ODBGenericEnumerable(ref TEnumerable enumerable)
	{
		this.enumerable = enumerable;
	}

	public void Dispose()
	{
		if (!this.disposed)
		{
			this.enumerable = default(TEnumerable);
			this.disposed = true;
			this.next = ODBGenericEnumerable<T, TEnumerator, TEnumerable>.recycle;
			ODBGenericEnumerable<T, TEnumerator, TEnumerable>.recycle = this;
		}
	}

	public TEnumerator GetEnumerator()
	{
		if (this.disposed)
		{
			throw new ObjectDisposedException("enumerable");
		}
		return this.enumerable.GetEnumerator();
	}

	public static ODBGenericEnumerable<T, TEnumerator, TEnumerable> Open(ref TEnumerable enumerable)
	{
		if (ODBGenericEnumerable<T, TEnumerator, TEnumerable>.recycle == null)
		{
			return new ODBGenericEnumerable<T, TEnumerator, TEnumerable>(ref enumerable);
		}
		ODBGenericEnumerable<T, TEnumerator, TEnumerable> ts = ODBGenericEnumerable<T, TEnumerator, TEnumerable>.recycle;
		ts.disposed = false;
		ODBGenericEnumerable<T, TEnumerator, TEnumerable>.recycle = ts.next;
		ts.enumerable = enumerable;
		return ts;
	}

	IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
	{
		TEnumerator enumerator = this.GetEnumerator();
		return ODBCachedEnumerator<T, TEnumerator>.Cache(ref enumerator);
	}

	IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		throw new NotSupportedException("Cannot use non generic IEnumerable interface with given object");
	}
}