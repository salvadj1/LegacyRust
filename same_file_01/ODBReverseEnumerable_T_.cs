using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ODBReverseEnumerable<T> : IEnumerable, ODBEnumerable<T, ODBReverseEnumerator<T>>, IEnumerable<T>
where T : UnityEngine.Object
{
	private ODBSibling<T> sibling;

	public ODBReverseEnumerable(ODBNode<T> node)
	{
		this.sibling.has = true;
		this.sibling.item = node;
	}

	public ODBReverseEnumerable(ODBList<T> list) : this(list.last)
	{
	}

	public ODBReverseEnumerable(ODBSibling<T> sibling)
	{
		this.sibling = sibling;
	}

	public ODBReverseEnumerator<T> GetEnumerator()
	{
		return new ODBReverseEnumerator<T>(this.sibling);
	}

	IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
	{
		ODBReverseEnumerator<T> enumerator = this.GetEnumerator();
		return ODBCachedEnumerator<T, ODBReverseEnumerator<T>>.Cache(ref enumerator);
	}

	IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	public IEnumerable<T> ToGeneric()
	{
		ODBReverseEnumerable<T> ts = this;
		return ODBGenericEnumerable<T, ODBReverseEnumerator<T>, ODBReverseEnumerable<T>>.Open(ref ts);
	}
}