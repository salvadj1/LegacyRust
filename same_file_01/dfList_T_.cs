using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class dfList<T> : IDisposable, IEnumerable, ICollection<T>, IList<T>, IEnumerable<T>
{
	private const int DEFAULT_CAPACITY = 128;

	private static Queue<object> pool;

	private T[] items;

	private int count;

	internal int Capacity
	{
		get
		{
			return (int)this.items.Length;
		}
	}

	public int Count
	{
		get
		{
			return this.count;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	public T this[int index]
	{
		get
		{
			if (index < 0 || index > this.count - 1)
			{
				throw new IndexOutOfRangeException();
			}
			return this.items[index];
		}
		set
		{
			if (index < 0 || index > this.count - 1)
			{
				throw new IndexOutOfRangeException();
			}
			this.items[index] = value;
		}
	}

	internal T[] Items
	{
		get
		{
			return this.items;
		}
	}

	static dfList()
	{
		dfList<T>.pool = new Queue<object>();
	}

	internal dfList()
	{
	}

	internal dfList(IList<T> listToClone)
	{
		this.AddRange(listToClone);
	}

	internal dfList(int capacity)
	{
		this.EnsureCapacity(capacity);
	}

	public void Add(T item)
	{
		this.EnsureCapacity(this.count + 1);
		T[] tArray = this.items;
		dfList<T> _dfList = this;
		int num = _dfList.count;
		int num1 = num;
		_dfList.count = num + 1;
		tArray[num1] = item;
	}

	public void AddRange(dfList<T> list)
	{
		this.EnsureCapacity(this.count + list.Count);
		Array.Copy(list.items, 0, this.items, this.count, list.Count);
		dfList<T> count = this;
		count.count = count.count + list.Count;
	}

	public void AddRange(IList<T> list)
	{
		this.EnsureCapacity(this.count + list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			T[] item = this.items;
			dfList<T> _dfList = this;
			int num = _dfList.count;
			int num1 = num;
			_dfList.count = num + 1;
			item[num1] = list[i];
		}
	}

	public void AddRange(T[] list)
	{
		this.EnsureCapacity(this.count + (int)list.Length);
		Array.Copy(list, 0, this.items, this.count, (int)list.Length);
		dfList<T> length = this;
		length.count = length.count + (int)list.Length;
	}

	public bool Any(Func<T, bool> predicate)
	{
		for (int i = 0; i < this.count; i++)
		{
			if (predicate(this.items[i]))
			{
				return true;
			}
		}
		return false;
	}

	public void Clear()
	{
		Array.Clear(this.items, 0, (int)this.items.Length);
		this.count = 0;
	}

	public dfList<T> Clone()
	{
		dfList<T> ts = dfList<T>.Obtain(this.count);
		Array.Copy(this.items, ts.items, this.count);
		ts.count = this.count;
		return ts;
	}

	public dfList<T> Concat(dfList<T> list)
	{
		dfList<T> ts = dfList<T>.Obtain(this.count + list.count);
		ts.AddRange(this);
		ts.AddRange(list);
		return ts;
	}

	public bool Contains(T item)
	{
		if (item == null)
		{
			for (int i = 0; i < this.count; i++)
			{
				if (this.items[i] == null)
				{
					return true;
				}
			}
			return false;
		}
		EqualityComparer<T> @default = EqualityComparer<T>.Default;
		for (int j = 0; j < this.count; j++)
		{
			if (@default.Equals(this.items[j], item))
			{
				return true;
			}
		}
		return false;
	}

	public dfList<TResult> Convert<TResult>()
	{
		dfList<TResult> tResults = dfList<TResult>.Obtain(this.count);
		for (int i = 0; i < this.count; i++)
		{
			tResults.Add((TResult)Convert.ChangeType(this.items[i], typeof(TResult)));
		}
		return tResults;
	}

	public void CopyTo(T[] array)
	{
		this.CopyTo(array, 0);
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		Array.Copy(this.items, 0, array, arrayIndex, this.count);
	}

	public void CopyTo(int sourceIndex, T[] dest, int destIndex, int length)
	{
		if (sourceIndex + length > this.count)
		{
			throw new IndexOutOfRangeException("sourceIndex");
		}
		if (dest == null)
		{
			throw new ArgumentNullException("dest");
		}
		if (destIndex + length > (int)dest.Length)
		{
			throw new IndexOutOfRangeException("destIndex");
		}
		Array.Copy(this.items, sourceIndex, dest, destIndex, length);
	}

	public T Dequeue()
	{
		if (this.count == 0)
		{
			throw new IndexOutOfRangeException();
		}
		T t = this.items[0];
		this.RemoveAt(0);
		return t;
	}

	public void Dispose()
	{
		this.Release();
	}

	public void Enqueue(T item)
	{
		this.Add(item);
	}

	public void EnsureCapacity(int Size)
	{
		if ((int)this.items.Length < Size)
		{
			int size = Size / 128 * 128 + 128;
			Array.Resize<T>(ref this.items, size);
		}
	}

	public T First()
	{
		if (this.count == 0)
		{
			throw new IndexOutOfRangeException();
		}
		return this.items[0];
	}

	public T FirstOrDefault()
	{
		if (this.count <= 0)
		{
			return default(T);
		}
		return this.items[0];
	}

	public T FirstOrDefault(Func<T, bool> predicate)
	{
		for (int i = 0; i < this.count; i++)
		{
			if (predicate(this.items[i]))
			{
				return this.items[i];
			}
		}
		return default(T);
	}

	public void ForEach(Action<T> action)
	{
		int num = 0;
		while (num < this.Count)
		{
			int num1 = num;
			num = num1 + 1;
			action(this.items[num1]);
		}
	}

	public IEnumerator<T> GetEnumerator()
	{
		return dfList<T>.PooledEnumerator.Obtain(this, null);
	}

	public dfList<T> GetRange(int index, int length)
	{
		dfList<T> ts = dfList<T>.Obtain(length);
		this.CopyTo(0, ts.items, index, length);
		return ts;
	}

	public int IndexOf(T item)
	{
		return Array.IndexOf<T>(this.items, item, 0, this.count);
	}

	public void Insert(int index, T item)
	{
		this.EnsureCapacity(this.count + 1);
		if (index < this.count)
		{
			Array.Copy(this.items, index, this.items, index + 1, this.count - index);
		}
		this.items[index] = item;
		dfList<T> _dfList = this;
		_dfList.count = _dfList.count + 1;
	}

	public void InsertRange(int index, T[] array)
	{
		if (array == null)
		{
			throw new ArgumentNullException("items");
		}
		if (index < 0 || index > this.count)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		this.EnsureCapacity(this.count + (int)array.Length);
		if (index < this.count)
		{
			Array.Copy(this.items, index, this.items, index + (int)array.Length, this.count - index);
		}
		array.CopyTo(this.items, index);
		dfList<T> length = this;
		length.count = length.count + (int)array.Length;
	}

	public void InsertRange(int index, dfList<T> list)
	{
		if (list == null)
		{
			throw new ArgumentNullException("items");
		}
		if (index < 0 || index > this.count)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		this.EnsureCapacity(this.count + list.count);
		if (index < this.count)
		{
			Array.Copy(this.items, index, this.items, index + list.count, this.count - index);
		}
		Array.Copy(list.items, 0, this.items, index, list.count);
		dfList<T> _dfList = this;
		_dfList.count = _dfList.count + list.count;
	}

	public T Last()
	{
		if (this.count == 0)
		{
			throw new IndexOutOfRangeException();
		}
		return this.items[this.count - 1];
	}

	public T LastOrDefault()
	{
		if (this.count == 0)
		{
			return default(T);
		}
		return this.items[this.count - 1];
	}

	public T LastOrDefault(Func<T, bool> predicate)
	{
		T t = default(T);
		for (int i = 0; i < this.count; i++)
		{
			if (predicate(this.items[i]))
			{
				t = this.items[i];
			}
		}
		return t;
	}

	public int Matching(Func<T, bool> predicate)
	{
		int num = 0;
		for (int i = 0; i < this.count; i++)
		{
			if (predicate(this.items[i]))
			{
				num++;
			}
		}
		return num;
	}

	public static dfList<T> Obtain()
	{
		return (dfList<T>.pool.Count <= 0 ? new dfList<T>() : (dfList<T>)dfList<T>.pool.Dequeue());
	}

	internal static dfList<T> Obtain(int capacity)
	{
		dfList<T> ts = dfList<T>.Obtain();
		ts.EnsureCapacity(capacity);
		return ts;
	}

	public void Release()
	{
		this.Clear();
		dfList<T>.pool.Enqueue(this);
	}

	public bool Remove(T item)
	{
		int num = this.IndexOf(item);
		if (num == -1)
		{
			return false;
		}
		this.RemoveAt(num);
		return true;
	}

	public void RemoveAll(Predicate<T> predicate)
	{
		int num = 0;
		while (num < this.count)
		{
			if (!predicate(this.items[num]))
			{
				num++;
			}
			else
			{
				this.RemoveAt(num);
			}
		}
	}

	public void RemoveAt(int index)
	{
		if (index >= this.count)
		{
			throw new ArgumentOutOfRangeException();
		}
		dfList<T> _dfList = this;
		_dfList.count = _dfList.count - 1;
		if (index < this.count)
		{
			Array.Copy(this.items, index + 1, this.items, index, this.count - index);
		}
		this.items[this.count] = default(T);
	}

	public void RemoveRange(int index, int length)
	{
		if (index < 0 || length < 0 || this.count - index < length)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (this.count > 0)
		{
			dfList<T> _dfList = this;
			_dfList.count = _dfList.count - length;
			if (index < this.count)
			{
				Array.Copy(this.items, index + length, this.items, index, this.count - index);
			}
			Array.Clear(this.items, this.count, length);
		}
	}

	public void Reverse()
	{
		Array.Reverse(this.items, 0, this.count);
	}

	public dfList<TResult> Select<TResult>(Func<T, TResult> selector)
	{
		dfList<TResult> tResults = dfList<TResult>.Obtain(this.count);
		for (int i = 0; i < this.count; i++)
		{
			tResults.Add(selector(this.items[i]));
		}
		return tResults;
	}

	public void Sort()
	{
		Array.Sort<T>(this.items, 0, this.count, null);
	}

	public void Sort(IComparer<T> comparer)
	{
		Array.Sort<T>(this.items, 0, this.count, comparer);
	}

	public void Sort(Comparison<T> comparison)
	{
		if (comparison == null)
		{
			throw new ArgumentNullException("comparison");
		}
		if (this.count > 0)
		{
			using (dfList<T>.FunctorComparer functorComparer = dfList<T>.FunctorComparer.Obtain(comparison))
			{
				Array.Sort<T>(this.items, 0, this.count, functorComparer);
			}
		}
	}

	IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return dfList<T>.PooledEnumerator.Obtain(this, null);
	}

	public T[] ToArray()
	{
		T[] tArray = new T[this.count];
		Array.Copy(this.items, tArray, this.count);
		return tArray;
	}

	public T[] ToArray(int index, int length)
	{
		T[] tArray = new T[this.count];
		if (this.count > 0)
		{
			this.CopyTo(index, tArray, 0, length);
		}
		return tArray;
	}

	public void TrimExcess()
	{
		Array.Resize<T>(ref this.items, this.count);
	}

	public dfList<T> Where(Func<T, bool> predicate)
	{
		dfList<T> ts = dfList<T>.Obtain(this.count);
		for (int i = 0; i < this.count; i++)
		{
			if (predicate(this.items[i]))
			{
				ts.Add(this.items[i]);
			}
		}
		return ts;
	}

	private class FunctorComparer : IDisposable, IComparer<T>
	{
		private static Queue<dfList<T>.FunctorComparer> pool;

		private Comparison<T> comparison;

		static FunctorComparer()
		{
			dfList<T>.FunctorComparer.pool = new Queue<dfList<T>.FunctorComparer>();
		}

		public FunctorComparer()
		{
		}

		public int Compare(T x, T y)
		{
			return this.comparison(x, y);
		}

		public void Dispose()
		{
			this.Release();
		}

		public static dfList<T>.FunctorComparer Obtain(Comparison<T> comparison)
		{
			dfList<T>.FunctorComparer functorComparer = (dfList<T>.FunctorComparer.pool.Count <= 0 ? new dfList<T>.FunctorComparer() : dfList<T>.FunctorComparer.pool.Dequeue());
			functorComparer.comparison = comparison;
			return functorComparer;
		}

		public void Release()
		{
			this.comparison = null;
			if (!dfList<T>.FunctorComparer.pool.Contains(this))
			{
				dfList<T>.FunctorComparer.pool.Enqueue(this);
			}
		}
	}

	private class PooledEnumerator : IDisposable, IEnumerator, IEnumerable, IEnumerable<T>, IEnumerator<T>
	{
		private static Queue<dfList<T>.PooledEnumerator> pool;

		private dfList<T> list;

		private Func<T, bool> predicate;

		private int currentIndex;

		private T currentValue;

		private bool isValid;

		public T Current
		{
			get
			{
				if (!this.isValid)
				{
					throw new InvalidOperationException("The enumerator is no longer valid");
				}
				return this.currentValue;
			}
		}

		object System.Collections.IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		static PooledEnumerator()
		{
			dfList<T>.PooledEnumerator.pool = new Queue<dfList<T>.PooledEnumerator>();
		}

		public PooledEnumerator()
		{
		}

		public void Dispose()
		{
			this.Release();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this;
		}

		public bool MoveNext()
		{
			if (!this.isValid)
			{
				throw new InvalidOperationException("The enumerator is no longer valid");
			}
			while (this.currentIndex < this.list.Count)
			{
				dfList<T> ts = this.list;
				dfList<T>.PooledEnumerator pooledEnumerator = this;
				int num = pooledEnumerator.currentIndex;
				int num1 = num;
				pooledEnumerator.currentIndex = num + 1;
				T item = ts[num1];
				if (this.predicate == null || this.predicate(item))
				{
					this.currentValue = item;
					return true;
				}
			}
			this.Release();
			this.currentValue = default(T);
			return false;
		}

		public static dfList<T>.PooledEnumerator Obtain(dfList<T> list, Func<T, bool> predicate = null)
		{
			dfList<T>.PooledEnumerator ts = (dfList<T>.PooledEnumerator.pool.Count <= 0 ? new dfList<T>.PooledEnumerator() : dfList<T>.PooledEnumerator.pool.Dequeue());
			ts.ResetInternal(list, predicate);
			return ts;
		}

		public void Release()
		{
			if (this.isValid)
			{
				this.isValid = false;
				dfList<T>.PooledEnumerator.pool.Enqueue(this);
			}
		}

		public void Reset()
		{
			throw new NotImplementedException();
		}

		private void ResetInternal(dfList<T> list, Func<T, bool> predicate = null)
		{
			this.isValid = true;
			this.list = list;
			this.predicate = predicate;
			this.currentIndex = 0;
			this.currentValue = default(T);
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this;
		}
	}
}