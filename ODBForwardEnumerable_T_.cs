using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ODBForwardEnumerable<T> : IEnumerable, ODBEnumerable<T, ODBForwardEnumerator<T>>, IEnumerable<T>
where T : UnityEngine.Object
{
	private ODBSibling<T> sibling;

	public ODBForwardEnumerable(ODBNode<T> node)
	{
		this.sibling.has = true;
		this.sibling.item = node;
	}

	public ODBForwardEnumerable(ODBList<T> list) : this(list.last)
	{
	}

	public ODBForwardEnumerable(ODBSibling<T> sibling)
	{
		this.sibling = sibling;
	}

	public ODBForwardEnumerator<T> GetEnumerator()
	{
		return new ODBForwardEnumerator<T>(this.sibling);
	}

	IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
	{
		ODBForwardEnumerator<T> enumerator = this.GetEnumerator();
		return ODBCachedEnumerator<T, ODBForwardEnumerator<T>>.Cache(ref enumerator);
	}

	IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	public IEnumerable<T> ToGeneric()
	{
		ODBForwardEnumerable<T> ts = this;
		return ODBGenericEnumerable<T, ODBForwardEnumerator<T>, ODBForwardEnumerable<T>>.Open(ref ts);
	}
}