using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[Serializable]
public class ArmorModelCollection<T> : IEnumerable, IEnumerable<T>, IEnumerable<KeyValuePair<ArmorModelSlot, T>>
{
	public T feet;

	public T legs;

	public T torso;

	public T head;

	public T this[ArmorModelSlot slot]
	{
		get
		{
			switch (slot)
			{
				case ArmorModelSlot.Feet:
				{
					return this.feet;
				}
				case ArmorModelSlot.Legs:
				{
					return this.legs;
				}
				case ArmorModelSlot.Torso:
				{
					return this.torso;
				}
				case ArmorModelSlot.Head:
				{
					return this.head;
				}
			}
			return default(T);
		}
		set
		{
			switch (slot)
			{
				case ArmorModelSlot.Feet:
				{
					this.feet = value;
					break;
				}
				case ArmorModelSlot.Legs:
				{
					this.legs = value;
					break;
				}
				case ArmorModelSlot.Torso:
				{
					this.torso = value;
					break;
				}
				case ArmorModelSlot.Head:
				{
					this.head = value;
					break;
				}
			}
		}
	}

	public ArmorModelCollection()
	{
	}

	public ArmorModelCollection(T defaultValue)
	{
		for (ArmorModelSlot i = ArmorModelSlot.Feet; (int)i < 4; i = (ArmorModelSlot)((byte)i + (byte)ArmorModelSlot.Legs))
		{
			this[i] = defaultValue;
		}
	}

	public ArmorModelCollection(ArmorModelMemberMap<T> map) : this()
	{
		for (ArmorModelSlot i = ArmorModelSlot.Feet; (int)i < 4; i = (ArmorModelSlot)((byte)i + (byte)ArmorModelSlot.Legs))
		{
			this[i] = map[i];
		}
	}

	public void Clear(T value)
	{
		for (ArmorModelSlot i = ArmorModelSlot.Feet; (int)i < 4; i = (ArmorModelSlot)((byte)i + (byte)ArmorModelSlot.Legs))
		{
			this[i] = value;
		}
	}

	public void Clear()
	{
		for (ArmorModelSlot i = ArmorModelSlot.Feet; (int)i < 4; i = (ArmorModelSlot)((byte)i + (byte)ArmorModelSlot.Legs))
		{
			this[i] = default(T);
		}
	}

	public void CopyFrom(T[] array, int offset)
	{
		for (int i = 0; i < 4; i++)
		{
			int num = offset;
			offset = num + 1;
			this[(ArmorModelSlot)((byte)i)] = array[num];
		}
	}

	public int CopyTo(T[] array, int offset, int maxCount)
	{
		for (int i = 0; i < 4; i++)
		{
			int num = offset;
			offset = num + 1;
			array[num] = this[(ArmorModelSlot)((byte)i)];
		}
		return offset;
	}

	public ArmorModelCollection<T>.Enumerator GetEnumerator()
	{
		return new ArmorModelCollection<T>.Enumerator(this);
	}

	IEnumerator<KeyValuePair<ArmorModelSlot, T>> System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ArmorModelSlot,T>>.GetEnumerator()
	{
		return new ArmorModelCollection<T>.Enumerator(this);
	}

	IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
	{
		return new ArmorModelCollection<T>.Enumerator(this);
	}

	IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return new ArmorModelCollection<T>.Enumerator(this);
	}

	public ArmorModelMemberMap<T> ToMemberMap()
	{
		ArmorModelMemberMap<T> item = new ArmorModelMemberMap<T>();
		for (ArmorModelSlot i = ArmorModelSlot.Feet; (int)i < 4; i = (ArmorModelSlot)((byte)i + (byte)ArmorModelSlot.Legs))
		{
			item[i] = this[i];
		}
		return item;
	}

	public struct Enumerator : IDisposable, IEnumerator, IEnumerator<T>, IEnumerator<KeyValuePair<ArmorModelSlot, T>>
	{
		private ArmorModelCollection<T> collection;

		private int index;

		public T Current
		{
			get
			{
				return (this.index <= 0 || this.index >= 4 ? default(T) : this.collection[(ArmorModelSlot)((byte)this.index)]);
			}
		}

		KeyValuePair<ArmorModelSlot, T> System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<ArmorModelSlot,T>>.Current
		{
			get
			{
				if (this.index <= 0 || this.index >= 4)
				{
					throw new InvalidOperationException();
				}
				return new KeyValuePair<ArmorModelSlot, T>((ArmorModelSlot)((byte)this.index), this.collection[(ArmorModelSlot)((byte)this.index)]);
			}
		}

		object System.Collections.IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		internal Enumerator(ArmorModelCollection<T> collection)
		{
			this.collection = collection;
			this.index = -1;
		}

		public void Dispose()
		{
			this = new ArmorModelCollection<T>.Enumerator();
		}

		public bool MoveNext()
		{
			ArmorModelCollection<T>.Enumerator enumerator = this;
			int num = enumerator.index + 1;
			int num1 = num;
			enumerator.index = num;
			return num1 < 4;
		}

		public void Reset()
		{
			this.index = -1;
		}
	}
}