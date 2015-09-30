using System;
using UnityEngine;

public class ODBNode<T> : IDisposable
where T : UnityEngine.Object
{
	public T self;

	public ODBSibling<T> n;

	public ODBSibling<T> p;

	public ODBList<T> list;

	private bool hasList;

	private static ODBNode<T>.Recycler recycle;

	public ODBForwardEnumerable<T> afterExclusive
	{
		get
		{
			return new ODBForwardEnumerable<T>(this.n);
		}
	}

	public ODBForwardEnumerable<T> afterInclusive
	{
		get
		{
			return new ODBForwardEnumerable<T>(this);
		}
	}

	public ODBReverseEnumerable<T> beforeExclusive
	{
		get
		{
			return new ODBReverseEnumerable<T>(this.p);
		}
	}

	public ODBReverseEnumerable<T> beforeInclusive
	{
		get
		{
			return new ODBReverseEnumerable<T>(this);
		}
	}

	private ODBNode()
	{
	}

	public void Dispose()
	{
		if (this.hasList)
		{
			if (this.n.has)
			{
				if (!this.p.has)
				{
					this.n.item.p = new ODBSibling<T>();
					this.list.first = this.n;
					ODBList<T> ts = this.list;
					ts.count = ts.count - 1;
				}
				else
				{
					this.p.item.n = this.n;
					this.n.item.p = this.p;
					this.p = new ODBSibling<T>();
					ODBList<T> ts1 = this.list;
					ts1.count = ts1.count - 1;
				}
			}
			else if (!this.p.has)
			{
				this.list.count = 0;
				this.list.any = false;
				this.list.first = new ODBSibling<T>();
				this.list.last = new ODBSibling<T>();
			}
			else
			{
				this.p.item.n = new ODBSibling<T>();
				this.list.last = this.p;
				this.p = new ODBSibling<T>();
				ODBList<T> ts2 = this.list;
				ts2.count = ts2.count - 1;
			}
			this.hasList = false;
			this.list = null;
			ODBNode<T>.recycle.Push(this);
		}
	}

	public static ODBNode<T> New(ODBList<T> list, T self)
	{
		ODBNode<T> oDBNode;
		if (!ODBNode<T>.recycle.Pop(out oDBNode))
		{
			oDBNode = new ODBNode<T>();
		}
		oDBNode.Setup(list, self);
		return oDBNode;
	}

	private void Setup(ODBList<T> list, T self)
	{
		ODBSibling<T> oDBSibling = new ODBSibling<T>();
		this.self = self;
		this.list = list;
		this.hasList = true;
		this.n = new ODBSibling<T>();
		if (!list.any)
		{
			list.count = 1;
			list.any = true;
			oDBSibling.has = true;
			oDBSibling.item = this;
			list.first = oDBSibling;
			list.last = oDBSibling;
		}
		else
		{
			this.p = list.last;
			this.p.item.n.item = this;
			this.p.item.n.has = true;
			list.last.item = this;
			ODBList<T> ts = list;
			ts.count = ts.count + 1;
		}
	}

	private struct Recycler
	{
		public ODBNode<T> items;

		public int count;

		public bool any;

		public bool Pop(out ODBNode<T> o)
		{
			o = this.items;
			if (!this.any)
			{
				return false;
			}
			ODBNode<T>.Recycler recycler = this;
			int num = recycler.count - 1;
			int num1 = num;
			recycler.count = num;
			if (num1 != 0)
			{
				this.items = o.n.item;
			}
			else
			{
				this.any = false;
				this.items = null;
			}
			return true;
		}

		public void Push(ODBNode<T> item)
		{
			item.list = null;
			item.self = (T)null;
			if (!this.any)
			{
				item.n = new ODBSibling<T>();
				this.items = item;
				this.count = 1;
				this.any = true;
			}
			else
			{
				item.n.item = this.items;
				item.n.has = true;
				this.items = item;
				ODBNode<T>.Recycler recycler = this;
				recycler.count = recycler.count + 1;
			}
		}
	}
}