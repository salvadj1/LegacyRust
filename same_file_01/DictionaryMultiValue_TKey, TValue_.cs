using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

public class DictionaryMultiValue<TKey, TValue> : IEnumerable, IEnumerable<MultiValue<TValue>.KeyPair<TKey>>
{
	private Dictionary<TKey, MultiValue<TValue>> dict;

	public readonly bool HasKeyComparer;

	public readonly IEqualityComparer<TValue> ValueComparer;

	public readonly bool HasValueComparer;

	public MultiValue<TValue>.KeyPair<TKey> this[TKey key]
	{
		get
		{
			return new MultiValue<TValue>.KeyPair<TKey>(this, key);
		}
		set
		{
			MultiValue<TValue> tValues;
			MultiValue<TValue> tValues1;
			MultiValue<TValue> tValues2;
			MultiValue<TValue> tValues3;
			MultiValue<TValue> tValues4;
			if (value.Dictionary == this)
			{
				if (this.AreEqual(value.Key, key))
				{
					if (this.GetMultiValue(value.Key, out tValues1))
					{
						if (this.GetMultiValue(key, out tValues))
						{
							tValues.Set(tValues1);
						}
						else if (tValues1.Count > 0)
						{
							this.dict.Add(value.Key, tValues1.Clone());
						}
					}
				}
				else if (value.Valid)
				{
					if (value.Dictionary.GetMultiValue(value.Key, out tValues3))
					{
						if (this.GetMultiValue(key, out tValues2))
						{
							tValues2.Set(tValues3);
						}
						else if (tValues3.Count > 0 && tValues3.Clone(this.ValueComparer, out tValues2))
						{
							this.dict.Add(value.Key, tValues2);
						}
					}
				}
				else if (value.Dictionary.GetMultiValue(value.Key, out tValues4))
				{
					tValues4.Clear();
				}
			}
		}
	}

	public IEqualityComparer<TKey> KeyComparer
	{
		get
		{
			return this.dict.Comparer;
		}
	}

	public DictionaryMultiValue(IEnumerable<KeyValuePair<TKey, TValue>> dict, IEqualityComparer<TKey> keyComp, IEqualityComparer<TValue> valComp)
	{
		this.HasKeyComparer = keyComp != null;
		this.HasValueComparer = valComp != null;
		this.ValueComparer = valComp;
		this.dict = (!this.HasKeyComparer ? new Dictionary<TKey, MultiValue<TValue>>() : new Dictionary<TKey, MultiValue<TValue>>(keyComp));
		this.AddRange(dict);
	}

	public bool Add(KeyValuePair<TKey, TValue> kv)
	{
		MultiValue<TValue> tValues;
		if (this.GetOrCreateMultiValue(kv.Key, out tValues))
		{
			return tValues.Add(kv.Value);
		}
		if (!tValues.Add(kv.Value))
		{
			return false;
		}
		this.dict.Add(kv.Key, tValues);
		return true;
	}

	public bool Add(TKey key, TValue value)
	{
		MultiValue<TValue> tValues;
		if (this.GetOrCreateMultiValue(key, out tValues))
		{
			return tValues.Add(value);
		}
		if (!tValues.Add(value))
		{
			return false;
		}
		this.dict.Add(key, tValues);
		return true;
	}

	public int AddRange(TKey key, IEnumerable<TValue> value)
	{
		MultiValue<TValue> tValues;
		if (this.GetOrCreateMultiValue(key, out tValues, value))
		{
			return tValues.AddRange(value);
		}
		int count = tValues.Count;
		if (count > 0)
		{
			this.dict.Add(key, tValues);
		}
		return count;
	}

	public int AddRange<TValueEnumerable>(KeyValuePair<TKey, TValueEnumerable> kv)
	where TValueEnumerable : IEnumerable<TValue>
	{
		MultiValue<TValue> tValues;
		if (this.GetOrCreateMultiValue(kv.Key, out tValues))
		{
			return tValues.AddRange(kv.Value);
		}
		int count = tValues.Count;
		if (count > 0)
		{
			this.dict.Add(kv.Key, tValues);
		}
		return count;
	}

	public int AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
	{
		int num = 0;
		IEnumerator<KeyValuePair<TKey, TValue>> enumerator = pairs.GetEnumerator();
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

	public int AddRange<TValueEnumerable>(IEnumerable<KeyValuePair<TKey, TValueEnumerable>> pairs)
	where TValueEnumerable : IEnumerable<TValue>
	{
		int num = 0;
		IEnumerator<KeyValuePair<TKey, TValueEnumerable>> enumerator = pairs.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				num = num + this.AddRange<TValueEnumerable>(enumerator.Current);
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

	private bool AreEqual(TKey l, TKey r)
	{
		IEqualityComparer<TKey> comparer = this.dict.Comparer;
		return (comparer.GetHashCode(l) != comparer.GetHashCode(r) ? false : comparer.Equals(l, r));
	}

	public bool Clear(TKey key)
	{
		MultiValue<TValue> tValues;
		return (!this.GetMultiValue(key, out tValues) ? false : tValues.Clear());
	}

	public bool Clear(TKey key, bool erase)
	{
		return (!this.Clear(key) ? false : this.dict.Remove(key));
	}

	public bool Contains(TKey key, TValue value)
	{
		MultiValue<TValue> tValues;
		return (!this.GetMultiValue(key, out tValues) ? false : tValues.Contains(value));
	}

	public bool Contains(KeyValuePair<TKey, TValue> kv)
	{
		return this.Contains(kv.Key, kv.Value);
	}

	public bool ContainsKey(TKey key)
	{
		return this.dict.ContainsKey(key);
	}

	public bool ContainsValue(TKey key, TValue value)
	{
		return this.Contains(key, value);
	}

	public bool ContainsValue(TValue value)
	{
		bool flag;
		Dictionary<TKey, MultiValue<TValue>>.ValueCollection.Enumerator enumerator = this.dict.Values.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.Contains(value))
				{
					continue;
				}
				flag = true;
				return flag;
			}
			return false;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return flag;
	}

	private MultiValue<TValue> CreateMultiValue()
	{
		if (!this.HasValueComparer)
		{
			return new MultiValue<TValue>();
		}
		return new MultiValue<TValue>(this.ValueComparer);
	}

	private MultiValue<TValue> CreateMultiValue(IEnumerable<TValue> enumerable)
	{
		if (!this.HasValueComparer)
		{
			return new MultiValue<TValue>(enumerable);
		}
		return new MultiValue<TValue>(enumerable, this.ValueComparer);
	}

	[DebuggerHidden]
	public IEnumerator<MultiValue<TValue>.KeyPair<TKey>> GetEnumerator()
	{
		DictionaryMultiValue<TKey, TValue>.<GetEnumerator>c__Iterator23 variable = null;
		return variable;
	}

	internal bool GetMultiValue(TKey key, out MultiValue<TValue> v)
	{
		return this.dict.TryGetValue(key, out v);
	}

	internal bool GetOrCreateMultiValue(TKey key, out MultiValue<TValue> v)
	{
		if (this.dict.TryGetValue(key, out v))
		{
			return true;
		}
		v = this.CreateMultiValue();
		return false;
	}

	internal bool GetOrCreateMultiValue(TKey key, out MultiValue<TValue> v, IEnumerable<TValue> enumerable)
	{
		if (this.dict.TryGetValue(key, out v))
		{
			return true;
		}
		v = this.CreateMultiValue(enumerable);
		return false;
	}

	public bool Remove(TKey key)
	{
		MultiValue<TValue> tValues;
		return (!this.GetMultiValue(key, out tValues) || !this.dict.Remove(key) ? false : tValues.Clear());
	}

	public bool RemoveAt(TKey key, int index)
	{
		MultiValue<TValue> tValues;
		return (!this.GetMultiValue(key, out tValues) ? false : tValues.RemoveAt(index));
	}

	internal void SetMultiValue(TKey key, MultiValue<TValue> mv)
	{
		this.dict.Add(key, mv);
	}

	IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	public int ValueCount(TKey key)
	{
		MultiValue<TValue> tValues;
		return (!this.GetMultiValue(key, out tValues) ? 0 : tValues.Count);
	}
}