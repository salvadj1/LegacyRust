using System;
using UnityEngine;

public struct ODBItem<TItem> : IEquatable<TItem>
where TItem : UnityEngine.Object
{
	internal ODBNode<TItem> node;

	internal ODBItem(ODBNode<TItem> node)
	{
		this.node = node;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return (this.node == null ? true : !(TItem)this.node.self);
		}
		if (obj is ODBItem<TItem>)
		{
			return ((ODBItem<TItem>)obj).node == this.node;
		}
		if (!(obj is UnityEngine.Object))
		{
			return obj.Equals(this.node);
		}
		return (this.node == null || !(TItem)this.node.self ? false : (TItem)this.node.self == (UnityEngine.Object)obj);
	}

	public bool Equals(TItem obj)
	{
		if (obj)
		{
			return obj.Equals(this);
		}
		return (this.node == null ? true : !(TItem)this.node.self);
	}

	public override int GetHashCode()
	{
		return this.node.GetHashCode();
	}

	public static bool operator ==(ODBItem<TItem> L, ODBItem<TItem> R)
	{
		return L.node == R.node;
	}

	public static bool operator ==(ODBItem<TItem> L, TItem R)
	{
		return L.Equals(R);
	}

	public static bool operator ==(TItem L, ODBItem<TItem> R)
	{
		return R.Equals(L);
	}

	public static implicit operator TItem(ODBItem<TItem> item)
	{
		if (item.node == null)
		{
			return (TItem)null;
		}
		return item.node.self;
	}

	public static bool operator !=(ODBItem<TItem> L, ODBItem<TItem> R)
	{
		return L.node != R.node;
	}

	public static bool operator !=(ODBItem<TItem> L, TItem R)
	{
		return !L.Equals(R);
	}

	public static bool operator !=(TItem L, ODBItem<TItem> R)
	{
		return !R.Equals(L);
	}

	public override string ToString()
	{
		return this.node.ToString();
	}
}