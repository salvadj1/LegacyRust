using System;
using System.Collections;
using System.Collections.Generic;
using uLink;

internal class ContextMenuData
{
	[NonSerialized]
	public readonly int options_length;

	[NonSerialized]
	public readonly ContextMenuItemData[] options;

	private readonly static BitStreamCodec.Serializer serializer;

	private readonly static BitStreamCodec.Deserializer deserializer;

	static ContextMenuData()
	{
		ContextMenuData.serializer = new BitStreamCodec.Serializer(ContextMenuData.Serialize);
		ContextMenuData.deserializer = new BitStreamCodec.Deserializer(ContextMenuData.Deserialize);
		BitStreamCodec.Add<ContextMenuData>(ContextMenuData.deserializer, ContextMenuData.serializer);
	}

	public ContextMenuData(int optionCount, ContextMenuItemData[] data)
	{
		this.options_length = optionCount;
		this.options = data;
	}

	public ContextMenuData(IEnumerable<ContextActionPrototype> prototypeEnumerable)
	{
		if (!(prototypeEnumerable is ICollection<ContextActionPrototype>))
		{
			this.options = ContextMenuData.ToArray(prototypeEnumerable, out this.options_length);
		}
		else
		{
			this.options = new ContextMenuItemData[((ICollection<ContextActionPrototype>)prototypeEnumerable).Count];
			int num = 0;
			foreach (ContextActionPrototype contextActionPrototype in prototypeEnumerable)
			{
				int num1 = num;
				num = num1 + 1;
				this.options[num1] = new ContextMenuItemData(contextActionPrototype);
			}
			if (num < (int)this.options.Length)
			{
				Array.Resize<ContextMenuItemData>(ref this.options, (int)this.options.Length);
			}
			this.options_length = (int)this.options.Length;
		}
	}

	private static object Deserialize(BitStream stream, params object[] codecOptions)
	{
		byte[] numArray;
		int num;
		ContextMenuItemData[] contextMenuItemDataArray;
		int num1 = stream.Read<int>(codecOptions);
		if (num1 != 0)
		{
			contextMenuItemDataArray = new ContextMenuItemData[num1];
		}
		else
		{
			contextMenuItemDataArray = null;
		}
		ContextMenuItemData[] contextMenuItemDatum = contextMenuItemDataArray;
		for (int i = 0; i < num1; i++)
		{
			int num2 = stream.Read<int>(codecOptions);
			stream.ReadByteArray_MinimalCalls(out numArray, out num, codecOptions);
			contextMenuItemDatum[i] = new ContextMenuItemData(num2, num, numArray);
		}
		return new ContextMenuData(num1, contextMenuItemDatum);
	}

	private static void Serialize(BitStream stream, object value, params object[] codecOptions)
	{
		ContextMenuData contextMenuDatum = (ContextMenuData)value;
		stream.Write<int>(contextMenuDatum.options_length, codecOptions);
		for (int i = 0; i < contextMenuDatum.options_length; i++)
		{
			stream.Write<int>(contextMenuDatum.options[i].name, codecOptions);
			stream.WriteByteArray_MinimumCalls(contextMenuDatum.options[i].utf8_text, 0, contextMenuDatum.options[i].utf8_length, codecOptions);
		}
	}

	private static ContextMenuItemData[] ToArray(IEnumerable<ContextActionPrototype> enumerable, out int length)
	{
		ContextMenuItemData[] contextMenuItemDataArray;
		using (IEnumerator<ContextActionPrototype> enumerator = enumerable.GetEnumerator())
		{
			ContextMenuData.EnumerableConverter enumerableConverter = new ContextMenuData.EnumerableConverter();
			ContextMenuData.EnumerableConverter enumerator1 = enumerableConverter;
			enumerator1.enumerator = enumerable.GetEnumerator();
			enumerableConverter = enumerator1;
			enumerableConverter.R();
			length = enumerableConverter.length;
			contextMenuItemDataArray = enumerableConverter.array;
		}
		return contextMenuItemDataArray;
	}

	private struct EnumerableConverter
	{
		public IEnumerator<ContextActionPrototype> enumerator;

		public int length;

		public int spot;

		public ContextMenuItemData[] array;

		public void R()
		{
			if (this.enumerator.MoveNext())
			{
				ContextMenuData.EnumerableConverter enumerableConverter = this;
				enumerableConverter.length = enumerableConverter.length + 1;
				ContextActionPrototype current = this.enumerator.Current;
				this.R();
				ContextMenuData.EnumerableConverter enumerableConverter1 = this;
				int num = enumerableConverter1.spot - 1;
				int num1 = num;
				enumerableConverter1.spot = num;
				this.array[num1] = new ContextMenuItemData(current);
			}
			else if (this.length != 0)
			{
				this.array = new ContextMenuItemData[this.length];
				this.spot = this.length;
			}
			else
			{
				this.array = null;
			}
		}
	}
}