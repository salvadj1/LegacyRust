using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ScriptableObjectArrayBase<T> : ScriptableObject, IEnumerable, ICollection<T>, IList<T>, IEnumerable<T>
{
	[SerializeField]
	private T[] _array;

	public T[] array
	{
		get
		{
			return this._array ?? ScriptableObjectArrayBase<T>.konst.empty;
		}
	}

	public T this[int i]
	{
		get
		{
			return this.array[i];
		}
	}

	public int Length
	{
		get
		{
			return (this._array != null ? (int)this._array.Length : 0);
		}
	}

	int System.Collections.Generic.ICollection<T>.Count
	{
		get
		{
			return this.array.Count;
		}
	}

	bool System.Collections.Generic.ICollection<T>.IsReadOnly
	{
		get
		{
			return this.array.IsReadOnly;
		}
	}

	T System.Collections.Generic.IList<T>.this[int index]
	{
		get
		{
			return this.array[index];
		}
		set
		{
			this.array[index] = value;
		}
	}

	public ScriptableObjectArrayBase()
	{
	}

	public ScriptableObjectArrayBase<T>.Enumerator GetEnumerator()
	{
		return new ScriptableObjectArrayBase<T>.Enumerator(this._array);
	}

	void System.Collections.Generic.ICollection<T>.Add(T item)
	{
		this.array.Add(item);
	}

	void System.Collections.Generic.ICollection<T>.Clear()
	{
		this.array.Clear();
	}

	bool System.Collections.Generic.ICollection<T>.Contains(T item)
	{
		return Array.IndexOf<T>(this.array, item) != -1;
	}

	void System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex)
	{
		this.array.CopyTo(array, arrayIndex);
	}

	bool System.Collections.Generic.ICollection<T>.Remove(T item)
	{
		return this.array.Remove(item);
	}

	IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
	{
		return this.array.GetEnumerator();
	}

	int System.Collections.Generic.IList<T>.IndexOf(T item)
	{
		return this.array.IndexOf(item);
	}

	void System.Collections.Generic.IList<T>.Insert(int index, T item)
	{
		this.array.Insert(index, item);
	}

	void System.Collections.Generic.IList<T>.RemoveAt(int index)
	{
		this.array.RemoveAt(index);
	}

	IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.array.GetEnumerator();
	}

	public struct Enumerator : IDisposable, IEnumerator, IEnumerator<T>
	{
		private T[] array;

		private int i;

		public T Current
		{
			get
			{
				return this.array[this.i];
			}
		}

		object System.Collections.IEnumerator.Current
		{
			get
			{
				return this.array[this.i];
			}
		}

		public Enumerator(T[] array)
		{
			this.array = array ?? ScriptableObjectArrayBase<T>.konst.empty;
			this.i = -1;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			ScriptableObjectArrayBase<T>.Enumerator enumerator = this;
			int num = enumerator.i + 1;
			int num1 = num;
			enumerator.i = num;
			return num1 < (int)(this.array ?? ScriptableObjectArrayBase<T>.konst.empty).Length;
		}

		public void Reset()
		{
			this.i = -1;
		}
	}

	private static class konst
	{
		public readonly static T[] empty;

		static konst()
		{
			ScriptableObjectArrayBase<T>.konst.empty = new T[0];
		}
	}
}