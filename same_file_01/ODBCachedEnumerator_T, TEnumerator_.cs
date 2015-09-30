using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ODBCachedEnumerator<T, TEnumerator> : IDisposable, IEnumerator, ODBEnumerator<T>, IEnumerator<T>
where T : UnityEngine.Object
where TEnumerator : struct, ODBEnumerator<T>
{
	private ODBCachedEnumerator<T, TEnumerator> next;

	private static ODBCachedEnumerator<T, TEnumerator> recycle;

	private TEnumerator enumerator;

	private bool disposed;

	public T Current
	{
		get
		{
			return this.enumerator.ExplicitCurrent;
		}
	}

	T ODBEnumerator<T>.ExplicitCurrent
	{
		get
		{
			return this.Current;
		}
	}

	object System.Collections.IEnumerator.Current
	{
		get
		{
			throw new NotSupportedException("You must use the IEnumerator<> interface. as dispose is entirely neccisary");
		}
	}

	private ODBCachedEnumerator(ref TEnumerator enumerator)
	{
		this.enumerator = enumerator;
	}

	public static IEnumerator<T> Cache(ref TEnumerator enumerator)
	{
		if (ODBCachedEnumerator<T, TEnumerator>.recycle == null)
		{
			return new ODBCachedEnumerator<T, TEnumerator>(ref enumerator);
		}
		ODBCachedEnumerator<T, TEnumerator> oDBCachedEnumerator = ODBCachedEnumerator<T, TEnumerator>.recycle;
		ODBCachedEnumerator<T, TEnumerator>.recycle = oDBCachedEnumerator.next;
		oDBCachedEnumerator.disposed = false;
		oDBCachedEnumerator.enumerator = enumerator;
		oDBCachedEnumerator.next = null;
		return oDBCachedEnumerator;
	}

	public void Dispose()
	{
		if (!this.disposed)
		{
			this.disposed = true;
			this.next = ODBCachedEnumerator<T, TEnumerator>.recycle;
			ODBCachedEnumerator<T, TEnumerator>.recycle = this;
			this.enumerator.Dispose();
			this.enumerator = default(TEnumerator);
		}
	}

	public bool MoveNext()
	{
		return this.enumerator.MoveNext();
	}

	IEnumerator<T> ODBEnumerator<T>.ToGeneric()
	{
		return this;
	}

	public void Reset()
	{
		this.enumerator.Reset();
	}
}