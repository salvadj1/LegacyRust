using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class ODBSet<T> : ODBList<T>
where T : UnityEngine.Object
{
	public ODBSet()
	{
	}

	public ODBSet(IEnumerable<T> collection) : base(collection)
	{
	}

	public bool Add(T item)
	{
		return base.DoAdd(item);
	}

	public bool Add(T item, out ODBNode<T> node)
	{
		return base.DoAdd(item, out node);
	}

	public void Clear()
	{
		base.DoClear();
	}

	public void ExceptWith(ODBList<T> list)
	{
		base.DoExceptWith(list);
	}

	public void IntersectWith(ODBList<T> list)
	{
		base.DoIntersectWith(list);
	}

	public bool Remove(T item)
	{
		return base.DoRemove(item);
	}

	public bool Remove(ref ODBNode<T> node)
	{
		return base.DoRemove(ref node);
	}

	public void SymmetricExceptWith(ODBList<T> list)
	{
		base.DoSymmetricExceptWith(list);
	}

	public void UnionWith(ODBList<T> list)
	{
		base.DoUnionWith(list);
	}
}