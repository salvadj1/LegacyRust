using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public sealed class GrabBag<T> : IEnumerable, IList<T>, ICollection<T>, IEnumerable<T>
{
	private T[] _array;

	private int _length;

	public ArraySegment<T> ArraySegment
	{
		get
		{
			return new ArraySegment<T>(this._array, 0, this._length);
		}
	}

	public T[] Buffer
	{
		get
		{
			return this._array;
		}
	}

	public int Capacity
	{
		get
		{
			return (int)this._array.Length;
		}
	}

	public int Count
	{
		get
		{
			return this._length;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	public T this[int i]
	{
		get
		{
			return this._array[i];
		}
		set
		{
			this._array[i] = value;
		}
	}

	public GrabBag(int capacity)
	{
		this._array = new T[capacity];
		this._length = 0;
	}

	public GrabBag()
	{
		this._array = EmptyArray<T>.array;
		this._length = 0;
	}

	public GrabBag(T[] copy)
	{
		if (copy != null)
		{
			int length = (int)copy.Length;
			int num = length;
			this._length = length;
			if (num == 0)
			{
				this._length = 0;
				this._array = EmptyArray<T>.array;
				return;
			}
			this._length = (int)copy.Length;
			this._array = new T[this._length];
			Array.Copy(copy, this._array, this._length);
			return;
		}
		this._length = 0;
		this._array = EmptyArray<T>.array;
	}

	public GrabBag(GrabBag<T> copy)
	{
		if (copy == null || copy._length == 0)
		{
			this._length = 0;
			this._array = EmptyArray<T>.array;
		}
		else
		{
			this._length = copy._length;
			this._array = new T[this._length];
			Array.Copy(copy._array, this._array, this._length);
		}
	}

	public GrabBag(ICollection<T> collection)
	{
		this._array = collection.ToArray<T>();
		this._length = (int)this._array.Length;
	}

	public GrabBag(IEnumerable<T> collection)
	{
		this._array = collection.ToArray<T>();
		this._length = (int)this._array.Length;
	}

	public int Add(T item)
	{
		int num = this.Grow(1);
		this._array[num] = item;
		return num;
	}

	public void Clear()
	{
		while (this._length > 0)
		{
			GrabBag<T> grabBag = this;
			int num = grabBag._length - 1;
			int num1 = num;
			grabBag._length = num;
			this._array[num1] = default(T);
		}
	}

	public bool Contains(T item)
	{
		return Array.IndexOf<T>(this._array, item) != -1;
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		for (int i = 0; i < this._length; i++)
		{
			int num = arrayIndex;
			arrayIndex = num + 1;
			array[num] = this._array[i];
		}
	}

	public GrabBag<T>.Enumerator GetEnumerator()
	{
		GrabBag<T>.Enumerator enumerator = new GrabBag<T>.Enumerator();
		enumerator.array = this;
		enumerator.nonNull = true;
		enumerator.index = -1;
		return enumerator;
	}

	public int Grow(int count)
	{
		int num = this._length;
		int num1 = this._length + count - (int)this._array.Length;
		if (num1 > 0)
		{
			Array.Resize<T>(ref this._array, num1 / 2 * 4 + 1 + this._length);
		}
		GrabBag<T> grabBag = this;
		grabBag._length = grabBag._length + count;
		return num;
	}

	public int IndexOf(T item)
	{
		return (this._length != 0 ? Array.IndexOf<T>(this._array, item, 0, this._length) : -1);
	}

	public int IndexOf(T item, int start)
	{
		return (this._length != 0 ? Array.IndexOf<T>(this._array, item, start, this._length - start) : -1);
	}

	public int IndexOf(T item, int start, int count)
	{
		return (this._length != 0 ? Array.IndexOf<T>(this._array, item, start, count) : -1);
	}

	public void Insert(int index, T item)
	{
		int num = this.Grow(1);
		this._array[num] = this._array[index];
		this._array[index] = item;
	}

	public int LastIndexOf(T item)
	{
		return (this._length != 0 ? Array.LastIndexOf<T>(this._array, item, 0, this._length) : -1);
	}

	public int LastIndexOf(T item, int start)
	{
		return (this._length != 0 ? Array.LastIndexOf<T>(this._array, item, start, this._length - start) : -1);
	}

	public int LastIndexOf(T item, int start, int count)
	{
		return (this._length != 0 ? Array.LastIndexOf<T>(this._array, item, start, count) : -1);
	}

	public bool Remove(T item)
	{
		int num = Array.IndexOf<T>(this._array, item, 0, this._length);
		if (num == -1)
		{
			return false;
		}
		GrabBag<T> grabBag = this;
		int num1 = grabBag._length - 1;
		int num2 = num1;
		grabBag._length = num1;
		this._array[num] = this._array[num2];
		this._array[this._length] = default(T);
		return true;
	}

	public int RemoveAll(T item)
	{
		int num = 0;
		while (this.Remove(item))
		{
			num++;
		}
		return num;
	}

	public void RemoveAt(int index)
	{
		GrabBag<T> grabBag = this;
		int num = grabBag._length - 1;
		int num1 = num;
		grabBag._length = num;
		this._array[index] = this._array[num1];
		this._array[this._length] = default(T);
	}

	public void Reverse()
	{
		if (this._length > 0)
		{
			Array.Reverse(this._array, 0, this._length);
		}
	}

	public void Reverse(int start, int count)
	{
		if (this._length > 0)
		{
			Array.Reverse(this._array, start, count);
		}
	}

	public void Shrink()
	{
		if (this._length < (int)this._array.Length)
		{
			Array.Resize<T>(ref this._array, this._length);
		}
	}

	public void Sort()
	{
		if (this._length != 0)
		{
			Array.Sort<T>(this._array, 0, this._length);
		}
	}

	public void Sort(int start, int count)
	{
		if (this._length != 0)
		{
			Array.Sort<T>(this._array, start, count);
		}
	}

	public void Sort(IComparer<T> comparer)
	{
		if (this._length != 0)
		{
			Array.Sort<T>(this._array, 0, this._length, comparer);
		}
	}

	public void Sort(IComparer<T> comparer, int start, int count)
	{
		if (this._length != 0)
		{
			Array.Sort<T>(this._array, start, count, comparer);
		}
	}

	public void SortAsKey<V>(V[] values)
	{
		Array.Sort<T, V>(this._array, values, 0, this._length);
	}

	public void SortAsKey<V>(V[] values, IComparer<T> comparer)
	{
		Array.Sort<T, V>(this._array, values, 0, this._length, comparer);
	}

	public void SortAsKey<V>(V[] values, int start, int count)
	{
		Array.Sort<T, V>(this._array, values, start, count);
	}

	public void SortAsKey<V>(V[] values, int start, int count, IComparer<T> comparer)
	{
		Array.Sort<T, V>(this._array, values, start, count, comparer);
	}

	public void SortAsValue<K>(K[] keys)
	{
		Array.Sort<K, T>(keys, this._array, 0, this._length);
	}

	public void SortAsValue<K>(K[] keys, IComparer<K> comparer)
	{
		Array.Sort<K, T>(keys, this._array, 0, this._length, comparer);
	}

	public void SortAsValue<K>(K[] keys, int start, int count)
	{
		Array.Sort<K, T>(keys, this._array, start, count);
	}

	public void SortAsValue<K>(K[] keys, int start, int count, IComparer<K> comparer)
	{
		Array.Sort<K, T>(keys, this._array, start, count, comparer);
	}

	void System.Collections.Generic.ICollection<T>.Add(T item)
	{
		int num = this.Grow(1);
		this._array[num] = item;
	}

	IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
	{
		IEnumerator<T> klassEnumerator;
		if (this._length != 0)
		{
			klassEnumerator = new GrabBag<T>.KlassEnumerator(this);
		}
		else
		{
			klassEnumerator = EmptyArray<T>.emptyEnumerator;
		}
		return klassEnumerator;
	}

	IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		IEnumerator klassEnumerator;
		if (this._length != 0)
		{
			klassEnumerator = new GrabBag<T>.KlassEnumerator(this);
		}
		else
		{
			klassEnumerator = EmptyArray<T>.emptyEnumerator;
		}
		return klassEnumerator;
	}

	public T[] ToArray()
	{
		if (this._length == 0)
		{
			return EmptyArray<T>.array;
		}
		T[] tArray = new T[this._length];
		Array.Copy(this._array, tArray, this._length);
		return tArray;
	}

	public override string ToString()
	{
		return string.Format(GrabBag<T>.StringGetter.Format, this.Count, this.Capacity);
	}

	public struct Enumerator : IDisposable, IEnumerator, IEnumerator<T>
	{
		public GrabBag<T> array;

		public int index;

		public bool nonNull;

		public T Current
		{
			get
			{
				return this.array._array[this.index];
			}
		}

		object System.Collections.IEnumerator.Current
		{
			get
			{
				return this.array._array[this.index];
			}
		}

		public void Dispose()
		{
			this = new GrabBag<T>.Enumerator();
		}

		public bool MoveNext()
		{
			bool flag;
			if (!this.nonNull)
			{
				flag = false;
			}
			else
			{
				GrabBag<T>.Enumerator enumerator = this;
				int num = enumerator.index + 1;
				int num1 = num;
				enumerator.index = num;
				flag = num1 < this.array._length;
			}
			return flag;
		}

		public void Reset()
		{
			this.index = -1;
		}
	}

	private class KlassEnumerator : IDisposable, IEnumerator, IEnumerator<T>
	{
		public GrabBag<T> array;

		public int index;

		public T Current
		{
			get
			{
				return this.array._array[this.index];
			}
		}

		object System.Collections.IEnumerator.Current
		{
			get
			{
				return this.array._array[this.index];
			}
		}

		public KlassEnumerator(GrabBag<T> array)
		{
			this.array = array;
			this.index = -1;
		}

		public void Dispose()
		{
			this.array = null;
		}

		public bool MoveNext()
		{
			GrabBag<T>.KlassEnumerator klassEnumerator = this;
			int num = klassEnumerator.index + 1;
			int num1 = num;
			klassEnumerator.index = num;
			return num1 < this.array._length;
		}

		public void Reset()
		{
			this.index = -1;
		}
	}

	private static class StringGetter
	{
		public readonly static string Format;

		static StringGetter()
		{
			GrabBag<T>.StringGetter.Format = string.Concat("[DynArray<", typeof(T).Name, ">: Count={0}, Capacity={1}]");
		}
	}
}