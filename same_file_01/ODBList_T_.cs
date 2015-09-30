using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ODBList<T> : IEnumerable, IEnumerable<T>, ICollection<T>, ODBEnumerable<T, ODBForwardEnumerator<T>>
where T : UnityEngine.Object
{
	protected readonly HSet<T> hashSet;

	public ODBSibling<T> first;

	public ODBSibling<T> last;

	public int count;

	public bool any;

	private readonly bool isReadOnly;

	public ODBForwardEnumerable<T> forward
	{
		get
		{
			return new ODBForwardEnumerable<T>(this);
		}
	}

	public ODBReverseEnumerable<T> reverse
	{
		get
		{
			return new ODBReverseEnumerable<T>(this);
		}
	}

	int System.Collections.Generic.ICollection<T>.Count
	{
		get
		{
			return this.count;
		}
	}

	bool System.Collections.Generic.ICollection<T>.IsReadOnly
	{
		get
		{
			return this.isReadOnly;
		}
	}

	protected ODBList()
	{
		this.hashSet = new HSet<T>();
	}

	protected ODBList(IEnumerable<T> collection) : this()
	{
		IEnumerator<T> enumerator = collection.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				this.DoAdd(enumerator.Current);
			}
		}
		finally
		{
			if (enumerator == null)
			{
			}
			enumerator.Dispose();
		}
	}

	protected ODBList(bool isReadOnly) : this()
	{
		this.isReadOnly = isReadOnly;
	}

	protected ODBList(bool isReadOnly, IEnumerable<T> collection) : this(collection)
	{
		this.isReadOnly = isReadOnly;
	}

	public bool Contains(T item)
	{
		return (!this.any ? false : this.hashSet.Contains(item));
	}

	public bool Contains(ODBNode<T> item)
	{
		return (!this.any ? false : item.list == this);
	}

	public int CopyTo(T[] array)
	{
		return this.CopyTo(array, 0, this.count);
	}

	public int CopyTo(T[] array, int arrayIndex)
	{
		return this.CopyTo(array, arrayIndex, this.count);
	}

	public int CopyTo(T[] array, int arrayIndex, int count)
	{
		if (!this.any)
		{
			return 0;
		}
		ODBNode<T> oDBNode = this.first.item;
		int num = -1;
		if (count > this.count)
		{
			count = this.count;
		}
		while (true)
		{
			int num1 = num + 1;
			num = num1;
			if (num1 >= count)
			{
				break;
			}
			int num2 = arrayIndex;
			arrayIndex = num2 + 1;
			array[num2] = oDBNode.self;
			oDBNode = oDBNode.n.item;
		}
		return num;
	}

	protected bool DoAdd(T item)
	{
		if (!item)
		{
			throw new MissingReferenceException("You cannot pass a missing or null item into the list");
		}
		if (!this.hashSet.Add(item))
		{
			return false;
		}
		ODBNode<T>.New(this, item);
		return true;
	}

	protected bool DoAdd(T item, out ODBNode<T> node)
	{
		if (!item)
		{
			throw new MissingReferenceException("You cannot pass a missing or null item into the list");
		}
		if (!this.hashSet.Add(item))
		{
			node = null;
			return false;
		}
		node = ODBNode<T>.New(this, item);
		return true;
	}

	protected void DoClear()
	{
		if (this.any)
		{
			this.hashSet.Clear();
			do
			{
				this.first.item.Dispose();
			}
			while (this.any);
		}
	}

	protected void DoExceptWith(ODBList<T> list)
	{
		if (!this.any || !list.any)
		{
			return;
		}
		if (list != this)
		{
			ODBSibling<T> oDBSibling = list.first;
			do
			{
				T t = oDBSibling.item.self;
				oDBSibling = oDBSibling.item.n;
				if (!this.hashSet.Remove(t))
				{
					continue;
				}
				this.KnownFind(t).Dispose();
			}
			while (oDBSibling.has);
		}
		else
		{
			this.DoClear();
		}
	}

	protected void DoIntersectWith(ODBList<T> list)
	{
		if (this.any)
		{
			if (!list.any)
			{
				this.DoClear();
			}
			else if (list != this)
			{
				this.hashSet.IntersectWith(list.hashSet);
				int count = this.hashSet.Count;
				if (count != 0)
				{
					ODBSibling<T> oDBSibling = this.first;
					do
					{
						ODBNode<T> oDBNode = oDBSibling.item;
						oDBSibling = oDBSibling.item.n;
						if (this.hashSet.Contains(oDBNode.self))
						{
							continue;
						}
						oDBNode.Dispose();
						if (this.count != count)
						{
							continue;
						}
						break;
					}
					while (oDBSibling.has);
				}
				else
				{
					while (this.any)
					{
						this.first.item.Dispose();
					}
				}
			}
		}
	}

	protected bool DoRemove(ref ODBNode<T> node)
	{
		if (!this.any || node.list != this)
		{
			return false;
		}
		this.hashSet.Remove(node.self);
		node.Dispose();
		node = null;
		return true;
	}

	protected bool DoRemove(T item)
	{
		if (!this.any || !this.hashSet.Remove(item))
		{
			return false;
		}
		this.KnownFind(item).Dispose();
		return true;
	}

	protected void DoSymmetricExceptWith(ODBList<T> list)
	{
		if (this.any)
		{
			if (list.any)
			{
				if (list != this)
				{
					ODBSibling<T> oDBSibling = list.first;
					do
					{
						T t = oDBSibling.item.self;
						oDBSibling = oDBSibling.item.n;
						if (!this.hashSet.Remove(t))
						{
							this.hashSet.Add(t);
							ODBNode<T>.New(this, t);
						}
						else
						{
							this.KnownFind(t).Dispose();
						}
					}
					while (oDBSibling.has);
				}
				else
				{
					this.DoClear();
				}
			}
		}
		else if (list.any)
		{
			ODBSibling<T> oDBSibling1 = list.first;
			do
			{
				T t1 = oDBSibling1.item.self;
				oDBSibling1 = oDBSibling1.item.n;
				this.hashSet.Add(t1);
				ODBNode<T>.New(this, t1);
			}
			while (oDBSibling1.has);
		}
	}

	protected void DoUnionWith(ODBList<T> list)
	{
		if (!list.any || list == this)
		{
			return;
		}
		ODBSibling<T> oDBSibling = list.first;
		do
		{
			T t = oDBSibling.item.self;
			oDBSibling = oDBSibling.item.n;
			if (!this.hashSet.Add(t))
			{
				continue;
			}
			ODBNode<T>.New(this, t);
		}
		while (oDBSibling.has);
	}

	public RecycleList<T> ExceptList(ODBList<T> list)
	{
		return this.hashSet.ExceptList(list.hashSet);
	}

	public RecycleList<T> ExceptList(IEnumerable<T> e)
	{
		return this.hashSet.ExceptList(e);
	}

	public ODBForwardEnumerator<T> GetEnumerator()
	{
		return new ODBForwardEnumerator<T>(this);
	}

	public RecycleList<T> IntersectList(ODBList<T> list)
	{
		return this.hashSet.IntersectList(list.hashSet);
	}

	public RecycleList<T> IntersectList(IEnumerable<T> e)
	{
		return this.hashSet.IntersectList(e);
	}

	protected ODBNode<T> KnownFind(T item)
	{
		ODBSibling<T> oDBSibling = this.first;
		do
		{
			if (oDBSibling.item.self == item)
			{
				return oDBSibling.item;
			}
			oDBSibling = oDBSibling.item.n;
		}
		while (oDBSibling.has);
		throw new ArgumentException("item was not found", "item");
	}

	public RecycleList<T> OperList(HSetOper oper, ODBList<T> list)
	{
		return this.hashSet.OperList(oper, list.hashSet);
	}

	public RecycleList<T> OperList(HSetOper oper, IEnumerable<T> collection)
	{
		return this.hashSet.OperList(oper, collection);
	}

	public RecycleList<T> SymmetricExceptList(ODBList<T> list)
	{
		return this.hashSet.SymmetricExceptList(list.hashSet);
	}

	public RecycleList<T> SymmetricExceptList(IEnumerable<T> e)
	{
		return this.hashSet.SymmetricExceptList(e);
	}

	void System.Collections.Generic.ICollection<T>.Add(T item)
	{
		if (this.isReadOnly)
		{
			throw new NotSupportedException("Read Only");
		}
		if (!this.DoAdd(item))
		{
			throw new ArgumentException(string.Concat("The list already contains the given item ", item), "item");
		}
	}

	void System.Collections.Generic.ICollection<T>.Clear()
	{
		if (this.isReadOnly)
		{
			throw new NotSupportedException("Read Only");
		}
		this.DoClear();
	}

	void System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex)
	{
		this.CopyTo(array, arrayIndex);
	}

	bool System.Collections.Generic.ICollection<T>.Remove(T item)
	{
		if (this.isReadOnly)
		{
			throw new NotSupportedException("Read Only");
		}
		return this.DoRemove(item);
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

	public T[] ToArray()
	{
		T[] tArray = new T[this.count];
		this.CopyTo(tArray, 0, this.count);
		return tArray;
	}

	public IEnumerable<T> ToGeneric()
	{
		return this;
	}

	public RecycleList<T> UnionList(ODBList<T> list)
	{
		return this.hashSet.UnionList(list.hashSet);
	}

	public RecycleList<T> UnionList(IEnumerable<T> e)
	{
		return this.hashSet.UnionList(e);
	}
}