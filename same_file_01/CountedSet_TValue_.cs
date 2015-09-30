using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class CountedSet<TValue> : IEnumerable, IEnumerable<TValue>, ICollection<TValue>
{
	private Dictionary<TValue, CountedSet<TValue>.Node> index;

	private uint totalRetains;

	private uint nodeCount;

	private static TValue[] empty;

	public int Count
	{
		get
		{
			return (int)this.nodeCount;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			return true;
		}
	}

	public int this[TValue value]
	{
		get
		{
			CountedSet<TValue>.Node node;
			return (!this.index.TryGetValue(value, out node) ? -1 : (int)(node.count - 1));
		}
	}

	static CountedSet()
	{
		CountedSet<TValue>.empty = new TValue[0];
	}

	public CountedSet(IEnumerable<TValue> values, IEqualityComparer<TValue> comparer)
	{
		this.index = new Dictionary<TValue, CountedSet<TValue>.Node>(comparer);
		IEnumerator<TValue> enumerator = values.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				this.Retain(enumerator.Current);
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

	public bool Contains(TValue value)
	{
		return this.index.ContainsKey(value);
	}

	private static EqualityComparer<CountedSet<TValue>.Node> ConvertEqualityComparer(IEqualityComparer<TValue> comparer)
	{
		if (comparer == null || comparer == CountedSet<TValue>.DefaultComparer.Singleton.Value.Comparer)
		{
			return CountedSet<TValue>.DefaultComparer.Singleton.Value;
		}
		return new CountedSet<TValue>.CustomComparer(comparer);
	}

	public Dictionary<TValue, CountedSet<TValue>.Node>.KeyCollection.Enumerator GetEnumerator()
	{
		return this.index.Keys.GetEnumerator();
	}

	public int Release(TValue value)
	{
		CountedSet<TValue>.Node node;
		if (!this.index.TryGetValue(value, out node))
		{
			return -1;
		}
		bool flag = node.Release();
		CountedSet<TValue> countedSet = this;
		countedSet.totalRetains = countedSet.totalRetains - 1;
		if (flag)
		{
			this.index.Remove(value);
			CountedSet<TValue> countedSet1 = this;
			countedSet1.nodeCount = countedSet1.nodeCount - 1;
		}
		return (int)node.count;
	}

	public TValue[] ReleaseAll()
	{
		TValue[] tValueArray;
		CountedSet<TValue>.ReleaseRecursor releaseRecursor = new CountedSet<TValue>.ReleaseRecursor(this);
		try
		{
			releaseRecursor.Run();
			tValueArray = releaseRecursor.array;
		}
		finally
		{
			((IDisposable)(object)releaseRecursor).Dispose();
		}
		return tValueArray;
	}

	public int Retain(TValue value)
	{
		CountedSet<TValue>.Node node;
		if (!this.index.TryGetValue(value, out node))
		{
			Dictionary<TValue, CountedSet<TValue>.Node> tValues = this.index;
			CountedSet<TValue>.Node node1 = new CountedSet<TValue>.Node()
			{
				v = value
			};
			node = node1;
			tValues[value] = node1;
			CountedSet<TValue> countedSet = this;
			countedSet.nodeCount = countedSet.nodeCount + 1;
		}
		uint num = node.count;
		node.Retain();
		CountedSet<TValue> countedSet1 = this;
		countedSet1.totalRetains = countedSet1.totalRetains + 1;
		return (int)num;
	}

	public void RetainAll()
	{
		foreach (CountedSet<TValue>.Node value in this.index.Values)
		{
			value.Retain();
			CountedSet<TValue> countedSet = this;
			countedSet.totalRetains = countedSet.totalRetains + 1;
		}
	}

	void System.Collections.Generic.ICollection<TValue>.Add(TValue item)
	{
		((ICollection<TValue>)this.index.Keys).Add(item);
	}

	void System.Collections.Generic.ICollection<TValue>.Clear()
	{
		((ICollection<TValue>)this.index.Keys).Clear();
	}

	void System.Collections.Generic.ICollection<TValue>.CopyTo(TValue[] array, int arrayIndex)
	{
		((ICollection<TValue>)this.index.Keys).CopyTo(array, arrayIndex);
	}

	bool System.Collections.Generic.ICollection<TValue>.Remove(TValue item)
	{
		throw new NotSupportedException();
	}

	IEnumerator<TValue> System.Collections.Generic.IEnumerable<TValue>.GetEnumerator()
	{
		return ((IEnumerable<TValue>)this.index.Keys).GetEnumerator();
	}

	IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)this.index.Keys).GetEnumerator();
	}

	private class CustomComparer : EqualityComparer<CountedSet<TValue>.Node>, IDisposable
	{
		private IEqualityComparer<TValue> comparer;

		public CustomComparer(IEqualityComparer<TValue> comparer)
		{
			this.comparer = comparer;
		}

		public void Dispose()
		{
			if (this.comparer is IDisposable)
			{
				((IDisposable)this.comparer).Dispose();
			}
			this.comparer = null;
		}

		public override bool Equals(CountedSet<TValue>.Node x, CountedSet<TValue>.Node y)
		{
			return this.comparer.Equals(x.v, y.v);
		}

		public override int GetHashCode(CountedSet<TValue>.Node obj)
		{
			return this.comparer.GetHashCode(obj.v);
		}
	}

	private class DefaultComparer : EqualityComparer<CountedSet<TValue>.Node>
	{
		public readonly EqualityComparer<TValue> Comparer;

		private DefaultComparer()
		{
			this.Comparer = EqualityComparer<TValue>.Default;
		}

		public override bool Equals(CountedSet<TValue>.Node x, CountedSet<TValue>.Node y)
		{
			return this.Comparer.Equals(x.v, y.v);
		}

		public override int GetHashCode(CountedSet<TValue>.Node obj)
		{
			return this.Comparer.GetHashCode(obj.v);
		}

		public static class Singleton
		{
			public readonly static CountedSet<TValue>.DefaultComparer Value;

			static Singleton()
			{
				CountedSet<TValue>.DefaultComparer.Singleton.Value = new CountedSet<TValue>.DefaultComparer();
			}
		}
	}

	public class Node
	{
		public TValue v;

		public bool done;

		public uint count;

		public uint ReferenceCount
		{
			get
			{
				return this.count + 1;
			}
		}

		public bool Released
		{
			get
			{
				return this.done;
			}
		}

		public bool Retained
		{
			get
			{
				return !this.done;
			}
		}

		public Node()
		{
		}

		public bool Release()
		{
			bool flag;
			if (this.done)
			{
				flag = false;
			}
			else
			{
				CountedSet<TValue>.Node node = this;
				UInt32 num = node.count - 1;
				uint num1 = num;
				node.count = num;
				flag = num1 == 0;
			}
			return flag;
		}

		public bool Retain()
		{
			if (this.done)
			{
				return false;
			}
			CountedSet<TValue>.Node node = this;
			uint num = node.count;
			uint num1 = num;
			node.count = num + 1;
			return num1 == 0;
		}
	}

	private struct ReleaseRecursor : IDisposable
	{
		private CountedSet<TValue> s;

		private Dictionary<TValue, CountedSet<TValue>.Node> dict;

		private Dictionary<TValue, CountedSet<TValue>.Node>.ValueCollection.Enumerator enumerator;

		public TValue[] array;

		private int count;

		private bool disposed;

		public ReleaseRecursor(CountedSet<TValue> v)
		{
			this.s = v;
			this.dict = this.s.index;
			this.enumerator = this.dict.Values.GetEnumerator();
			this.array = CountedSet<TValue>.empty;
			this.count = 0;
			this.disposed = false;
		}

		public void Dispose()
		{
			if (!this.disposed)
			{
				this.disposed = true;
				this.enumerator.Dispose();
			}
		}

		public void Run()
		{
			if (!this.enumerator.MoveNext())
			{
				this.Dispose();
				if (this.count > 0)
				{
					this.array = new TValue[this.count];
				}
				CountedSet<TValue>.ReleaseRecursor releaseRecursor = this;
				releaseRecursor.count = releaseRecursor.count - 1;
			}
			else
			{
				CountedSet<TValue>.Node current = this.enumerator.Current;
				if (!current.Release())
				{
					CountedSet<TValue> tValues = this.s;
					tValues.totalRetains = tValues.totalRetains - 1;
				}
				else
				{
					CountedSet<TValue> tValues1 = this.s;
					tValues1.totalRetains = tValues1.totalRetains - 1;
					CountedSet<TValue>.ReleaseRecursor releaseRecursor1 = this;
					releaseRecursor1.count = releaseRecursor1.count + 1;
					this.Run();
					this.dict.Remove(current.v);
					CountedSet<TValue> tValues2 = this.s;
					tValues2.nodeCount = tValues2.nodeCount - 1;
					TValue[] tValueArray = this.array;
					CountedSet<TValue>.ReleaseRecursor releaseRecursor2 = this;
					int num = releaseRecursor2.count;
					int num1 = num;
					releaseRecursor2.count = num - 1;
					tValueArray[num1] = current.v;
				}
			}
		}
	}
}