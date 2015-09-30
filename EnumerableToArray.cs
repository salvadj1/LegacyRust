using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class EnumerableToArray
{
	public static T[] ToArray<T>(this T[] array)
	{
		int length = (int)array.Length;
		if (length == 0)
		{
			return EmptyArray<T>.array;
		}
		T[] tArray = new T[length];
		Array.Copy(array, tArray, length);
		return tArray;
	}

	public static T[] ToArray<T>(this IEnumerable<T> enumerable)
	{
		T[] enumeratorToArray;
		if (enumerable is ICollection<T>)
		{
			ICollection<T> ts = (ICollection<T>)enumerable;
			T[] tArray = new T[ts.Count];
			ts.CopyTo(tArray, 0);
			return tArray;
		}
		using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
		{
			enumeratorToArray = (new EnumerableToArray.EnumeratorToArray<T>(enumerator)).array;
		}
		return enumeratorToArray;
	}

	public static T[] ToArray<T>(this ICollection<T> collection)
	{
		T[] tArray = new T[collection.Count];
		collection.CopyTo(tArray, 0);
		return tArray;
	}

	private struct EnumeratorToArray<T>
	{
		public T[] array;

		private IEnumerator<T> enumerator;

		private int len;

		public EnumeratorToArray(IEnumerator<T> enumerator)
		{
			this.array = null;
			this.enumerator = enumerator;
			this.len = 0;
			if (!enumerator.MoveNext())
			{
				this.array = EmptyArray<T>.array;
			}
			else
			{
				this.Fill();
			}
			this.enumerator = null;
		}

		private void Fill()
		{
			EnumerableToArray.EnumeratorToArray<T> enumeratorToArray = this;
			int num = enumeratorToArray.len;
			int num1 = num;
			enumeratorToArray.len = num + 1;
			int num2 = num1;
			T current = this.enumerator.Current;
			if (!this.enumerator.MoveNext())
			{
				this.array = new T[this.len];
			}
			else
			{
				this.Fill();
			}
			this.array[num2] = current;
		}
	}
}