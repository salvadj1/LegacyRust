using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ObjectDB<Object> : ODBList<Object>, IEnumerable, ICollection<Object>, IEnumerable<Object>, ODBEnumerable<Object, ODBForwardEnumerator<Object>>
where Object : UnityEngine.Object
{
	bool System.Collections.Generic.ICollection<Object>.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	public ObjectDB() : base(true)
	{
	}

	public bool Contains(ref ODBItem<Object> value)
	{
		return base.Contains(value.node);
	}

	public ODBItem<Object> Register(Object value)
	{
		ODBNode<Object> oDBNode;
		if (!base.DoAdd(value, out oDBNode))
		{
			throw new ArgumentException(string.Concat(value.ToString(), " was already registered"), "value");
		}
		return new ODBItem<Object>(oDBNode);
	}

	void System.Collections.Generic.ICollection<Object>.Add(Object value)
	{
		throw new NotSupportedException("Use register and you must keep track of the return value");
	}

	void System.Collections.Generic.ICollection<Object>.Clear()
	{
		throw new NotSupportedException("Clear would be catastrophic to design. you must manually unregister everything");
	}

	bool System.Collections.Generic.ICollection<Object>.Remove(Object value)
	{
		throw new NotSupportedException("You must call unregister using the return value from Register");
	}

	public void Unregister(ref ODBItem<Object> value)
	{
		if (!base.DoRemove(ref value.node))
		{
			throw new ArgumentException(string.Concat(value.node.ToString(), " does not belong to this list"), "value");
		}
	}
}