using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ODBForwardEnumerator<T> : IDisposable, IEnumerator, ODBEnumerator<T>, IEnumerator<T>
where T : UnityEngine.Object
{
	private ODBSibling<T> sib;

	public T Current;

	T ODBEnumerator<T>.ExplicitCurrent
	{
		get
		{
			return this.Current;
		}
	}

	T System.Collections.Generic.IEnumerator<T>.Current
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
			return this.Current;
		}
	}

	public ODBForwardEnumerator(ODBNode<T> node)
	{
		this.sib.has = true;
		this.sib.item = node;
		this.Current = (T)null;
	}

	public ODBForwardEnumerator(ODBList<T> list) : this(list.first)
	{
	}

	public ODBForwardEnumerator(ODBSibling<T> sibling)
	{
		this.sib = sibling;
		this.Current = (T)null;
	}

	public void Dispose()
	{
		this.sib = new ODBSibling<T>();
		this.Current = (T)null;
	}

	public bool MoveNext()
	{
		if (!this.sib.has)
		{
			return false;
		}
		ODBNode<T> oDBNode = this.sib.item;
		this.Current = oDBNode.self;
		this.sib = oDBNode.n;
		return true;
	}

	void System.Collections.IEnumerator.Reset()
	{
		throw new NotSupportedException();
	}

	public IEnumerator<T> ToGeneric()
	{
		ODBForwardEnumerator<T> oDBForwardEnumerator = this;
		return ODBCachedEnumerator<T, ODBForwardEnumerator<T>>.Cache(ref oDBForwardEnumerator);
	}
}