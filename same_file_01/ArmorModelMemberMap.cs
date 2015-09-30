using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[Serializable]
public struct ArmorModelMemberMap : IEnumerable, IEnumerable<ArmorModel>, IEnumerable<KeyValuePair<ArmorModelSlot, ArmorModel>>
{
	public ArmorModelFeet feet;

	public ArmorModelLegs legs;

	public ArmorModelTorso torso;

	public ArmorModelHead head;

	public ArmorModel this[ArmorModelSlot slot]
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
			return null;
		}
		set
		{
			switch (slot)
			{
				case ArmorModelSlot.Feet:
				{
					this.feet = (ArmorModelFeet)value;
					break;
				}
				case ArmorModelSlot.Legs:
				{
					this.legs = (ArmorModelLegs)value;
					break;
				}
				case ArmorModelSlot.Torso:
				{
					this.torso = (ArmorModelTorso)value;
					break;
				}
				case ArmorModelSlot.Head:
				{
					this.head = (ArmorModelHead)value;
					break;
				}
			}
		}
	}

	public ArmorModelMemberMap(ArmorModelMemberMap<ArmorModel> map)
	{
		this = new ArmorModelMemberMap();
		for (ArmorModelSlot i = ArmorModelSlot.Feet; (int)i < 4; i = (ArmorModelSlot)((byte)i + (byte)ArmorModelSlot.Legs))
		{
			this[i] = map[i];
		}
	}

	public void CopyFrom(ArmorModel[] array, int offset)
	{
		for (int i = 0; i < 4; i++)
		{
			int num = offset;
			offset = num + 1;
			this[(ArmorModelSlot)((byte)i)] = array[num];
		}
	}

	public int CopyTo(ArmorModel[] array, int offset, int maxCount)
	{
		for (int i = 0; i < 4; i++)
		{
			int num = offset;
			offset = num + 1;
			array[num] = this[(ArmorModelSlot)((byte)i)];
		}
		return offset;
	}

	public T GetArmorModel<T>()
	where T : ArmorModel, new()
	{
		return (T)this[ArmorModelSlotUtility.GetArmorModelSlotForClass<T>()];
	}

	public ArmorModel GetArmorModel(ArmorModelSlot slot)
	{
		return this[slot];
	}

	public ArmorModelMemberMap.Enumerator GetEnumerator()
	{
		return new ArmorModelMemberMap.Enumerator(this);
	}

	public static explicit operator ArmorModelMemberMap(ArmorModelMemberMap<ArmorModel> generic)
	{
		ArmorModelMemberMap item = new ArmorModelMemberMap();
		for (ArmorModelSlot i = ArmorModelSlot.Feet; (int)i < 4; i = (ArmorModelSlot)((byte)i + (byte)ArmorModelSlot.Legs))
		{
			item[i] = generic[i];
		}
		return item;
	}

	public static implicit operator ArmorModelMemberMap<ArmorModel>(ArmorModelMemberMap self)
	{
		ArmorModelMemberMap<ArmorModel> item = new ArmorModelMemberMap<ArmorModel>();
		for (ArmorModelSlot i = ArmorModelSlot.Feet; (int)i < 4; i = (ArmorModelSlot)((byte)i + (byte)ArmorModelSlot.Legs))
		{
			item[i] = self[i];
		}
		return item;
	}

	public void SetArmorModel<T>(T armorModel)
	where T : ArmorModel, new()
	{
		this[ArmorModelSlotUtility.GetArmorModelSlotForClass<T>()] = armorModel;
	}

	IEnumerator<ArmorModel> System.Collections.Generic.IEnumerable<ArmorModel>.GetEnumerator()
	{
		return new ArmorModelMemberMap.Enumerator(this);
	}

	IEnumerator<KeyValuePair<ArmorModelSlot, ArmorModel>> System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ArmorModelSlot,ArmorModel>>.GetEnumerator()
	{
		return new ArmorModelMemberMap.Enumerator(this);
	}

	IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return new ArmorModelMemberMap.Enumerator(this);
	}

	public ArmorModelMemberMap<ArmorModel> ToGenericArmorModelMap()
	{
		ArmorModelMemberMap<ArmorModel> item = new ArmorModelMemberMap<ArmorModel>();
		for (ArmorModelSlot i = ArmorModelSlot.Feet; (int)i < 4; i = (ArmorModelSlot)((byte)i + (byte)ArmorModelSlot.Legs))
		{
			item[i] = this[i];
		}
		return item;
	}

	public struct Enumerator : IDisposable, IEnumerator, IEnumerator<ArmorModel>, IEnumerator<KeyValuePair<ArmorModelSlot, ArmorModel>>
	{
		private ArmorModelMemberMap collection;

		private int index;

		public ArmorModel Current
		{
			get
			{
				ArmorModel item;
				if (this.index <= 0 || this.index >= 4)
				{
					item = null;
				}
				else
				{
					item = this.collection[(ArmorModelSlot)((byte)this.index)];
				}
				return item;
			}
		}

		KeyValuePair<ArmorModelSlot, ArmorModel> System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<ArmorModelSlot,ArmorModel>>.Current
		{
			get
			{
				if (this.index <= 0 || this.index >= 4)
				{
					throw new InvalidOperationException();
				}
				return new KeyValuePair<ArmorModelSlot, ArmorModel>((ArmorModelSlot)((byte)this.index), this.collection[(ArmorModelSlot)((byte)this.index)]);
			}
		}

		object System.Collections.IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		internal Enumerator(ArmorModelMemberMap collection)
		{
			this.collection = collection;
			this.index = -1;
		}

		public void Dispose()
		{
			this = new ArmorModelMemberMap.Enumerator();
		}

		public bool MoveNext()
		{
			ArmorModelMemberMap.Enumerator enumerator = this;
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