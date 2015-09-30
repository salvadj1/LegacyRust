using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

[DebuggerDisplay("Count = {Count}")]
public sealed class LockedList<T> : IEnumerable, IList, ICollection, ICollection<T>, IList<T>, IEnumerable<T>, IEquatable<List<T>>
{
	private readonly List<T> list;

	public int Capacity
	{
		get
		{
			return this.list.Capacity;
		}
	}

	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	public static LockedList<T> Empty
	{
		get
		{
			return LockedList<T>.EmptyInstance.List;
		}
	}

	private IList<T> ilist
	{
		get
		{
			return this.list;
		}
	}

	public T this[int index]
	{
		get
		{
			return this.list[index];
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	private IList olist
	{
		get
		{
			return this.list;
		}
	}

	int System.Collections.Generic.ICollection<T>.Count
	{
		get
		{
			return this.ilist.Count;
		}
	}

	bool System.Collections.Generic.ICollection<T>.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	T System.Collections.Generic.IList<T>.this[int index]
	{
		get
		{
			return this.ilist[index];
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	int System.Collections.ICollection.Count
	{
		get
		{
			return this.olist.Count;
		}
	}

	bool System.Collections.ICollection.IsSynchronized
	{
		get
		{
			return this.olist.IsSynchronized;
		}
	}

	object System.Collections.ICollection.SyncRoot
	{
		get
		{
			return this.olist.SyncRoot;
		}
	}

	bool System.Collections.IList.IsFixedSize
	{
		get
		{
			return false;
		}
	}

	bool System.Collections.IList.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	object System.Collections.IList.this[int index]
	{
		get
		{
			return this.olist[index];
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	private LockedList()
	{
		this.list = new List<T>(0);
	}

	public LockedList(List<T> list)
	{
		if (object.ReferenceEquals(list, null))
		{
			throw new ArgumentNullException("list");
		}
		this.list = list;
	}

	public int BinarySearch(T item)
	{
		return this.list.BinarySearch(item);
	}

	public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
	{
		return this.list.BinarySearch(index, count, item, comparer);
	}

	public int BinarySearch(T item, IComparer<T> comparer)
	{
		return this.list.BinarySearch(item, comparer);
	}

	public bool Contains(T item)
	{
		return this.list.Contains(item);
	}

	public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
	{
		return this.list.ConvertAll<TOutput>(converter);
	}

	public void CopyTo(T[] array)
	{
		this.list.CopyTo(array);
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		this.list.CopyTo(array, arrayIndex);
	}

	public void CopyTo(int index, T[] array, int arrayIndex, int count)
	{
		this.list.CopyTo(index, array, arrayIndex, count);
	}

	public bool Equals(List<T> list)
	{
		return this.list.Equals(list);
	}

	public override bool Equals(object obj)
	{
		bool flag;
		if (!(obj is LockedList<T>))
		{
			flag = (!(obj is List<T>) ? false : this.list.Equals(obj));
		}
		else
		{
			flag = this.list.Equals(((LockedList<T>)obj).list);
		}
		return flag;
	}

	public T Find(Predicate<T> match)
	{
		return this.list.Find(match);
	}

	public List<T> FindAll(Predicate<T> match)
	{
		return this.list.FindAll(match);
	}

	public int FindIndex(Predicate<T> match)
	{
		return this.list.FindIndex(match);
	}

	public T FindLast(Predicate<T> match)
	{
		return this.list.FindLast(match);
	}

	public int FindLastIndex(Predicate<T> match)
	{
		return this.list.FindLastIndex(match);
	}

	public void ForEach(Action<T> action)
	{
		this.list.ForEach(action);
	}

	public List<T>.Enumerator GetEnumerator()
	{
		return this.list.GetEnumerator();
	}

	public override int GetHashCode()
	{
		return this.list.GetHashCode();
	}

	public List<T> GetRange(int index, int count)
	{
		return this.list.GetRange(index, count);
	}

	public int IndexOf(T item)
	{
		return this.list.IndexOf(item);
	}

	public int IndexOf(T item, int index)
	{
		return this.list.IndexOf(item, index);
	}

	public int IndexOf(T item, int index, int count)
	{
		return this.list.IndexOf(item, index, count);
	}

	public int LastIndexOf(T item)
	{
		return this.list.LastIndexOf(item);
	}

	public int LastIndexOf(T item, int index)
	{
		return this.list.LastIndexOf(item, index);
	}

	public int LastIndexOf(T item, int index, int count)
	{
		return this.list.LastIndexOf(item, index, count);
	}

	void System.Collections.Generic.ICollection<T>.Add(T item)
	{
		throw new NotSupportedException();
	}

	void System.Collections.Generic.ICollection<T>.Clear()
	{
		throw new NotSupportedException();
	}

	bool System.Collections.Generic.ICollection<T>.Contains(T item)
	{
		return this.ilist.Contains(item);
	}

	void System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex)
	{
		this.ilist.CopyTo(array, arrayIndex);
	}

	bool System.Collections.Generic.ICollection<T>.Remove(T item)
	{
		throw new NotSupportedException();
	}

	IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
	{
		return this.ilist.GetEnumerator();
	}

	int System.Collections.Generic.IList<T>.IndexOf(T item)
	{
		return this.list.IndexOf(item);
	}

	void System.Collections.Generic.IList<T>.Insert(int index, T item)
	{
		throw new NotSupportedException();
	}

	void System.Collections.Generic.IList<T>.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	void System.Collections.ICollection.CopyTo(Array array, int index)
	{
		this.olist.CopyTo(array, index);
	}

	IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.olist.GetEnumerator();
	}

	int System.Collections.IList.Add(object value)
	{
		throw new NotSupportedException();
	}

	void System.Collections.IList.Clear()
	{
		throw new NotSupportedException();
	}

	bool System.Collections.IList.Contains(object value)
	{
		return this.olist.Contains(value);
	}

	int System.Collections.IList.IndexOf(object value)
	{
		return this.olist.IndexOf(value);
	}

	void System.Collections.IList.Insert(int index, object value)
	{
		throw new NotSupportedException();
	}

	void System.Collections.IList.Remove(object value)
	{
		throw new NotSupportedException();
	}

	void System.Collections.IList.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	public T[] ToArray()
	{
		return this.list.ToArray();
	}

	public List<T> ToList()
	{
		return this.list.GetRange(0, this.list.Count);
	}

	public override string ToString()
	{
		return this.list.ToString();
	}

	public bool TrueForAll(Predicate<T> match)
	{
		return this.list.TrueForAll(match);
	}

	private static class EmptyInstance
	{
		public readonly static LockedList<T> List;

		static EmptyInstance()
		{
			LockedList<T>.EmptyInstance.List = new LockedList<T>();
		}
	}
}