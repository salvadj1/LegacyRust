using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MultiValue<TValue> : IEnumerable, IEnumerable<TValue>, ICollection<TValue>, IList<TValue>, ICloneable
{
	private const int kCheckHashCountMin = 16;

	private const bool kIsReadOnly = false;

	private HashSet<TValue> hashSet;

	private List<TValue> list;

	private int count;

	public int Count
	{
		get
		{
			return this.count;
		}
	}

	public TValue this[int index]
	{
		get
		{
			return this.list[index];
		}
		set
		{
			this.list[index] = value;
		}
	}

	bool System.Collections.Generic.ICollection<TValue>.IsReadOnly
	{
		get
		{
			return false;
		}
	}

	private MultiValue(bool ignore)
	{
	}

	public MultiValue()
	{
		this.list = new List<TValue>();
		this.hashSet = new HashSet<TValue>();
	}

	private MultiValue(IEqualityComparer<TValue> comparer, MultiValue<TValue> mv)
	{
		this.list = new List<TValue>(mv.list);
		this.hashSet = new HashSet<TValue>(mv.hashSet, comparer);
		this.count = mv.count;
	}

	public MultiValue(IEnumerable<TValue> v)
	{
		MultiValue<TValue>.InitData initDatum = new MultiValue<TValue>.InitData();
		this.hashSet = new HashSet<TValue>();
		initDatum.mv = this;
		IEnumerator<TValue> enumerator = v.GetEnumerator();
		IEnumerator<TValue> enumerator1 = enumerator;
		initDatum.enumerator = enumerator;
		using (enumerator1)
		{
			initDatum.RecurseInit();
		}
	}

	public MultiValue(IEnumerable<TValue> v, IEqualityComparer<TValue> equalityComparer)
	{
		MultiValue<TValue>.InitData initDatum = new MultiValue<TValue>.InitData();
		this.hashSet = new HashSet<TValue>(equalityComparer);
		initDatum.mv = this;
		IEnumerator<TValue> enumerator = v.GetEnumerator();
		IEnumerator<TValue> enumerator1 = enumerator;
		initDatum.enumerator = enumerator;
		using (enumerator1)
		{
			initDatum.RecurseInit();
		}
	}

	public MultiValue(int capacity, IEqualityComparer<TValue> equalityComparer)
	{
		this.hashSet = new HashSet<TValue>(equalityComparer);
		this.list = new List<TValue>(capacity);
	}

	public MultiValue(IEqualityComparer<TValue> equalityComparer)
	{
		this.hashSet = new HashSet<TValue>(equalityComparer);
		this.list = new List<TValue>();
	}

	public bool Add(TValue item)
	{
		if (!this.hashSet.Add(item))
		{
			return false;
		}
		this.list.Add(item);
		MultiValue<TValue> multiValue = this;
		multiValue.count = multiValue.count + 1;
		return true;
	}

	public int AddRange(IEnumerable<TValue> value)
	{
		int num = 0;
		IEnumerator<TValue> enumerator = value.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				if (!this.Add(enumerator.Current))
				{
					continue;
				}
				num++;
			}
		}
		finally
		{
			if (enumerator == null)
			{
			}
			enumerator.Dispose();
		}
		return num;
	}

	public bool Clear()
	{
		if (this.count <= 0)
		{
			return false;
		}
		this.list.Clear();
		this.hashSet.Clear();
		this.count = 0;
		return true;
	}

	public MultiValue<TValue> Clone()
	{
		MultiValue<TValue> tValues = new MultiValue<TValue>(false)
		{
			hashSet = new HashSet<TValue>(this.hashSet),
			list = new List<TValue>(this.list),
			count = this.count
		};
		return tValues;
	}

	public bool Clone(IEqualityComparer<TValue> valueComparer, out MultiValue<TValue> val)
	{
		if (this.count == 0)
		{
			val = null;
			return false;
		}
		if (valueComparer == this.hashSet.Comparer)
		{
			val = this.Clone();
			return true;
		}
		val = new MultiValue<TValue>(this.list, valueComparer);
		if (val.count != 0)
		{
			return true;
		}
		val = null;
		return false;
	}

	public bool Contains(TValue item)
	{
		return this.hashSet.Contains(item);
	}

	public void CopyTo(TValue[] array, int arrayIndex)
	{
		this.list.CopyTo(array, arrayIndex);
	}

	public List<TValue>.Enumerator GetEnumerator()
	{
		return this.list.GetEnumerator();
	}

	public int IndexOf(TValue item)
	{
		if (this.count >= 16 && !this.hashSet.Contains(item))
		{
			return -1;
		}
		return this.list.IndexOf(item);
	}

	public int InsertOrMove(int index, TValue item)
	{
		int num;
		if (index == this.count)
		{
			if (this.hashSet.Add(item))
			{
				this.list.Add(item);
				MultiValue<TValue> multiValue = this;
				multiValue.count = multiValue.count + 1;
				return 1;
			}
			int num1 = this.list.IndexOf(item);
			num = this.count - num1;
			if (num == 1)
			{
				return 0;
			}
			if (num == 2)
			{
				this.list.Reverse(this.count - 2, 2);
			}
			else
			{
				this.list.RemoveAt(num1);
				this.list.Add(item);
			}
			return 2;
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index", "index < 0");
		}
		if (index > this.count)
		{
			throw new ArgumentOutOfRangeException("index", "index > count");
		}
		if (this.hashSet.Add(item))
		{
			this.list.Insert(index, item);
			MultiValue<TValue> multiValue1 = this;
			multiValue1.count = multiValue1.count + 1;
			return 1;
		}
		int num2 = this.list.IndexOf(item);
		int num3 = index - num2;
		num = num3;
		switch (num)
		{
			case -1:
			{
				this.list.Reverse(num2, 2);
				break;
			}
			case 0:
			{
				return 0;
			}
			case 1:
			{
				this.list.Reverse(index, 2);
				break;
			}
			default:
			{
				if (num3 <= -2)
				{
					for (int i = num2; i > index; i--)
					{
						this.list[i] = this.list[i - 1];
					}
				}
				else if (num3 >= 2)
				{
					for (int j = num2; j < index; j++)
					{
						this.list[j] = this.list[j + 1];
					}
				}
				this.list[index] = item;
				break;
			}
		}
		return 2;
	}

	public int Remove(TValue item)
	{
		if (!this.hashSet.Remove(item))
		{
			return 0;
		}
		if (!this.list.Remove(item))
		{
			this.hashSet.Add(item);
			return 0;
		}
		MultiValue<TValue> multiValue = this;
		int num = multiValue.count - 1;
		int num1 = num;
		multiValue.count = num;
		return (num1 != 0 ? 1 : 2);
	}

	public bool RemoveAt(int index)
	{
		TValue item = this.list[index];
		this.list.RemoveAt(index);
		this.hashSet.Remove(item);
		MultiValue<TValue> multiValue = this;
		int num = multiValue.count - 1;
		int num1 = num;
		multiValue.count = num;
		return num1 != 0;
	}

	public void Set(MultiValue<TValue> other)
	{
		if (other == this)
		{
			return;
		}
		this.Clear();
		foreach (TValue tValue in other)
		{
			this.Add(tValue);
		}
	}

	void System.Collections.Generic.ICollection<TValue>.Add(TValue item)
	{
		if (this.hashSet.Add(item))
		{
			this.list.Add(item);
			MultiValue<TValue> multiValue = this;
			multiValue.count = multiValue.count + 1;
		}
	}

	void System.Collections.Generic.ICollection<TValue>.Clear()
	{
		if (this.count > 0)
		{
			this.list.Clear();
			this.hashSet.Clear();
			this.count = 0;
		}
	}

	bool System.Collections.Generic.ICollection<TValue>.Remove(TValue value)
	{
		return this.Remove(value) != 0;
	}

	IEnumerator<TValue> System.Collections.Generic.IEnumerable<TValue>.GetEnumerator()
	{
		return ((IEnumerable<TValue>)this.list).GetEnumerator();
	}

	void System.Collections.Generic.IList<TValue>.Insert(int index, TValue value)
	{
		this.InsertOrMove(index, value);
	}

	void System.Collections.Generic.IList<TValue>.RemoveAt(int index)
	{
		TValue item = this.list[index];
		this.list.RemoveAt(index);
		this.hashSet.Remove(item);
		MultiValue<TValue> multiValue = this;
		multiValue.count = multiValue.count - 1;
	}

	IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)this.list).GetEnumerator();
	}

	object System.ICloneable.Clone()
	{
		return this.Clone();
	}

	private struct InitData
	{
		public MultiValue<TValue> mv;

		public IEnumerator<TValue> enumerator;

		public void RecurseInit()
		{
			while (this.enumerator.MoveNext())
			{
				TValue current = this.enumerator.Current;
				if (!this.mv.hashSet.Add(current))
				{
					continue;
				}
				MultiValue<TValue> tValues = this.mv;
				tValues.count = tValues.count + 1;
				this.RecurseInit();
				this.mv.list.Add(current);
				return;
			}
			this.mv.list = new List<TValue>(this.mv.count);
		}
	}

	public struct KeyPair<TKey> : IEnumerable, IEnumerable<TValue>, ICollection<TValue>, IList<TValue>
	{
		private readonly TKey key;

		private readonly DictionaryMultiValue<TKey, TValue> dict;

		public int Count
		{
			get
			{
				MultiValue<TValue> tValues;
				return (!this.GetMultiValue(out tValues) ? tValues.Count : 0);
			}
		}

		public DictionaryMultiValue<TKey, TValue> Dictionary
		{
			get
			{
				return this.dict;
			}
		}

		public TValue this[int i]
		{
			get
			{
				MultiValue<TValue> tValues;
				if (!this.GetMultiValue(out tValues))
				{
					return MultiValue<TValue>.KeyPair<TKey>.g.emptyList[i];
				}
				return tValues[i];
			}
			set
			{
				MultiValue<TValue> tValues;
				if (this.GetMultiValue(out tValues))
				{
					tValues[i] = value;
				}
				else
				{
					MultiValue<TValue>.KeyPair<TKey>.g.emptyList[i] = value;
				}
			}
		}

		public TKey Key
		{
			get
			{
				return this.key;
			}
		}

		bool System.Collections.Generic.ICollection<TValue>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public bool Valid
		{
			get
			{
				return this.dict != null;
			}
		}

		public KeyPair(DictionaryMultiValue<TKey, TValue> dict, TKey key)
		{
			this.dict = dict;
			this.key = key;
		}

		public bool Add(TValue value)
		{
			MultiValue<TValue> tValues;
			bool orCreateMultiValue = this.GetOrCreateMultiValue(out tValues);
			if (!tValues.Add(value))
			{
				return false;
			}
			if (!orCreateMultiValue)
			{
				this.Bind(tValues);
			}
			return true;
		}

		private void Bind(MultiValue<TValue> v)
		{
			this.dict.SetMultiValue(this.key, v);
		}

		public bool Clear(TValue value)
		{
			MultiValue<TValue> tValues;
			return (!this.GetMultiValue(out tValues) ? false : tValues.Clear());
		}

		public bool Contains(TValue value)
		{
			MultiValue<TValue> tValues;
			return (!this.GetMultiValue(out tValues) ? false : tValues.Contains(value));
		}

		public void CopyTo(TValue[] array, int arrayIndex)
		{
			MultiValue<TValue> tValues;
			if (!this.GetMultiValue(out tValues))
			{
				MultiValue<TValue>.KeyPair<TKey>.g.emptyList.CopyTo(array, arrayIndex);
			}
			else
			{
				tValues.CopyTo(array, arrayIndex);
			}
		}

		public List<TValue>.Enumerator GetEnumerator()
		{
			MultiValue<TValue> tValues;
			if (this.GetMultiValue(out tValues))
			{
				return tValues.GetEnumerator();
			}
			return MultiValue<TValue>.KeyPair<TKey>.g.emptyList.GetEnumerator();
		}

		private bool GetMultiValue(out MultiValue<TValue> v)
		{
			if (this.dict == null)
			{
				v = null;
			}
			return this.dict.GetMultiValue(this.key, out v);
		}

		private bool GetOrCreateMultiValue(out MultiValue<TValue> v)
		{
			if (this.dict == null)
			{
				throw new InvalidOperationException("The KeyPair is invalid");
			}
			return this.dict.GetOrCreateMultiValue(this.key, out v);
		}

		public int IndexOf(TValue item)
		{
			MultiValue<TValue> tValues;
			return (!this.GetMultiValue(out tValues) ? -1 : tValues.IndexOf(item));
		}

		public int InsertOrMove(int index, TValue item)
		{
			MultiValue<TValue> tValues;
			bool orCreateMultiValue = this.GetOrCreateMultiValue(out tValues);
			int num = tValues.InsertOrMove(index, item);
			if (num == 1 && !orCreateMultiValue)
			{
				this.Bind(tValues);
			}
			return num;
		}

		public int Remove(TValue value)
		{
			MultiValue<TValue> tValues;
			return (!this.GetMultiValue(out tValues) ? 0 : tValues.Remove(value));
		}

		public bool RemoveAt(int index)
		{
			MultiValue<TValue> tValues;
			return (!this.GetMultiValue(out tValues) ? false : tValues.RemoveAt(index));
		}

		void System.Collections.Generic.ICollection<TValue>.Add(TValue item)
		{
			MultiValue<TValue> tValues;
			bool orCreateMultiValue = this.GetOrCreateMultiValue(out tValues);
			((ICollection<TValue>)tValues).Add(item);
			if (!orCreateMultiValue && tValues.count != 0)
			{
				this.Bind(tValues);
			}
		}

		void System.Collections.Generic.ICollection<TValue>.Clear()
		{
			MultiValue<TValue> tValues;
			if (this.GetMultiValue(out tValues))
			{
				((ICollection<TValue>)tValues).Clear();
			}
		}

		bool System.Collections.Generic.ICollection<TValue>.Remove(TValue value)
		{
			MultiValue<TValue> tValues;
			return (!this.GetMultiValue(out tValues) ? false : ((ICollection<TValue>)tValues).Remove(value));
		}

		IEnumerator<TValue> System.Collections.Generic.IEnumerable<TValue>.GetEnumerator()
		{
			MultiValue<TValue> tValues;
			if (this.GetMultiValue(out tValues))
			{
				return ((IEnumerable<TValue>)tValues).GetEnumerator();
			}
			return ((IEnumerable<TValue>)MultiValue<TValue>.KeyPair<TKey>.g.emptyList).GetEnumerator();
		}

		void System.Collections.Generic.IList<TValue>.Insert(int index, TValue value)
		{
			MultiValue<TValue> tValues;
			bool orCreateMultiValue = this.GetOrCreateMultiValue(out tValues);
			((IList<TValue>)tValues).Insert(index, value);
			if (!orCreateMultiValue && tValues.count > 0)
			{
				this.Bind(tValues);
			}
		}

		void System.Collections.Generic.IList<TValue>.RemoveAt(int index)
		{
			MultiValue<TValue> tValues;
			if (this.GetMultiValue(out tValues))
			{
				((IList<TValue>)tValues).RemoveAt(index);
			}
			else
			{
				MultiValue<TValue>.KeyPair<TKey>.g.emptyList.RemoveAt(index);
			}
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			MultiValue<TValue> tValues;
			if (this.GetMultiValue(out tValues))
			{
				return ((IEnumerable)tValues).GetEnumerator();
			}
			return ((IEnumerable)MultiValue<TValue>.KeyPair<TKey>.g.emptyList).GetEnumerator();
		}

		private static class g
		{
			public readonly static List<TValue> emptyList;

			static g()
			{
				MultiValue<TValue>.KeyPair<TKey>.g.emptyList = new List<TValue>(0);
			}
		}
	}
}