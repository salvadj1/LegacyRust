using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

public static class ArmorModelSlotUtility
{
	public const int Count = 4;

	public const ArmorModelSlotMask All = ArmorModelSlotMask.Feet | ArmorModelSlotMask.Legs | ArmorModelSlotMask.Torso | ArmorModelSlotMask.Head;

	public const ArmorModelSlot Last = ArmorModelSlot.Head;

	public const ArmorModelSlot First = ArmorModelSlot.Feet;

	public const ArmorModelSlotMask None = 0;

	public const ArmorModelSlot Begin = ArmorModelSlot.Feet;

	public const ArmorModelSlot End = 4;

	public static bool Contains(this ArmorModelSlotMask slotMask, ArmorModelSlot slot)
	{
		return ((int)slot >= 4 ? false : ((byte)slotMask & (byte)ArmorModelSlot.Legs << (byte)(slot & (ArmorModelSlot.Legs | ArmorModelSlot.Torso | ArmorModelSlot.Head))) != 0);
	}

	public static bool Contains(this ArmorModelSlot slot, ArmorModelSlotMask slotMask)
	{
		return ((int)slot >= 4 ? false : ((byte)slotMask & (byte)ArmorModelSlot.Legs << (byte)(slot & (ArmorModelSlot.Legs | ArmorModelSlot.Torso | ArmorModelSlot.Head))) != 0);
	}

	public static ArmorModelSlot[] EnumerateSlots(this ArmorModelSlotMask slotMask)
	{
		return ArmorModelSlotUtility.Mask2SlotArray.FlagToSlotArray[(int)(slotMask & (ArmorModelSlotMask.Feet | ArmorModelSlotMask.Legs | ArmorModelSlotMask.Torso | ArmorModelSlotMask.Head))];
	}

	public static ArmorModelSlot GetArmorModelSlotForClass<T>()
	where T : ArmorModel, new()
	{
		return ArmorModelSlotUtility.ClassToArmorModelSlot<T>.ArmorModelSlot;
	}

	public static Type GetArmorModelType(this ArmorModelSlot slot)
	{
		Type item;
		if ((int)slot >= 4)
		{
			item = null;
		}
		else
		{
			item = ArmorModelSlotUtility.ClassToArmorModelSlot.ArmorModelSlotToType[slot];
		}
		return item;
	}

	public static int GetMaskedSlotCount(this ArmorModelSlotMask slotMask)
	{
		uint num = (uint)(slotMask & (ArmorModelSlotMask.Feet | ArmorModelSlotMask.Legs | ArmorModelSlotMask.Torso | ArmorModelSlotMask.Head));
		int num1 = 0;
		while (num != 0)
		{
			num1++;
			num = num & num - 1;
		}
		return num1;
	}

	public static int GetMaskedSlotCount(this ArmorModelSlot slot)
	{
		return ((int)slot >= 4 ? 0 : 1);
	}

	public static string GetRendererName(this ArmorModelSlot slot)
	{
		return ((int)slot >= 4 ? "Armor Renderer" : ArmorModelSlotUtility.RendererNames.Array[(int)slot]);
	}

	public static int GetUnmaskedSlotCount(this ArmorModelSlotMask slotMask)
	{
		uint num = (uint)(~slotMask & (ArmorModelSlotMask.Feet | ArmorModelSlotMask.Legs | ArmorModelSlotMask.Torso | ArmorModelSlotMask.Head));
		int num1 = 0;
		while (num != 0)
		{
			num1++;
			num = num & num - 1;
		}
		return num1;
	}

	public static int GetUnmaskedSlotCount(this ArmorModelSlot slot)
	{
		return ((int)slot >= 4 ? 4 : 3);
	}

	public static ArmorModelSlot[] ToArray(this ArmorModelSlotMask slotMask)
	{
		ArmorModelSlot[] flagToSlotArray = ArmorModelSlotUtility.Mask2SlotArray.FlagToSlotArray[(int)(slotMask & (ArmorModelSlotMask.Feet | ArmorModelSlotMask.Legs | ArmorModelSlotMask.Torso | ArmorModelSlotMask.Head))];
		ArmorModelSlot[] armorModelSlotArray = new ArmorModelSlot[(int)flagToSlotArray.Length];
		for (int i = 0; i < (int)flagToSlotArray.Length; i++)
		{
			armorModelSlotArray[i] = flagToSlotArray[i];
		}
		return armorModelSlotArray;
	}

	public static ArmorModelSlotMask ToMask(this ArmorModelSlot slot)
	{
		return (ArmorModelSlotMask)((byte)ArmorModelSlot.Legs << (byte)(slot & (ArmorModelSlot.Legs | ArmorModelSlot.Torso | ArmorModelSlot.Head)) & (byte)(ArmorModelSlot.Legs | ArmorModelSlot.Torso | ArmorModelSlot.Head));
	}

	public static ArmorModelSlotMask ToNotMask(this ArmorModelSlot slot)
	{
		return (ArmorModelSlotMask)(~((byte)ArmorModelSlot.Legs << (byte)(slot & (ArmorModelSlot.Legs | ArmorModelSlot.Torso | ArmorModelSlot.Head))) & (ArmorModelSlot.Legs | ArmorModelSlot.Torso | ArmorModelSlot.Head));
	}

	private static class ClassToArmorModelSlot
	{
		public readonly static Dictionary<ArmorModelSlot, Type> ArmorModelSlotToType;

		static ClassToArmorModelSlot()
		{
			List<Type> types = new List<Type>();
			Type[] typeArray = typeof(ArmorModelSlotUtility.ClassToArmorModelSlot).Assembly.GetTypes();
			for (int i = 0; i < (int)typeArray.Length; i++)
			{
				Type type = typeArray[i];
				if (type.IsSubclassOf(typeof(ArmorModel)) && !type.IsAbstract && type.IsDefined(typeof(ArmorModelSlotClassAttribute), false))
				{
					types.Add(type);
				}
			}
			ArmorModelSlotUtility.ClassToArmorModelSlot.ArmorModelSlotToType = new Dictionary<ArmorModelSlot, Type>(types.Count);
			foreach (Type type1 in types)
			{
				ArmorModelSlotClassAttribute customAttribute = (ArmorModelSlotClassAttribute)Attribute.GetCustomAttribute(type1, typeof(ArmorModelSlotClassAttribute));
				ArmorModelSlotUtility.ClassToArmorModelSlot.ArmorModelSlotToType.Add(customAttribute.ArmorModelSlot, type1);
			}
		}
	}

	private static class ClassToArmorModelSlot<T>
	where T : ArmorModel, new()
	{
		public readonly static ArmorModelSlot ArmorModelSlot;

		static ClassToArmorModelSlot()
		{
			ArmorModelSlotUtility.ClassToArmorModelSlot<T>.ArmorModelSlot = ((ArmorModelSlotClassAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ArmorModelSlotClassAttribute))).ArmorModelSlot;
		}
	}

	private static class Mask2SlotArray
	{
		public readonly static ArmorModelSlot[][] FlagToSlotArray;

		static Mask2SlotArray()
		{
			ArmorModelSlotUtility.Mask2SlotArray.FlagToSlotArray = new ArmorModelSlot[16][];
			for (int i = 0; i <= 15; i++)
			{
				int num = 0;
				for (int j = 0; j < 4; j++)
				{
					if ((i & 1 << (j & 31)) == 1 << (j & 31))
					{
						num++;
					}
				}
				ArmorModelSlotUtility.Mask2SlotArray.FlagToSlotArray[i] = new ArmorModelSlot[num];
				int num1 = 0;
				for (int k = 0; k < 4; k++)
				{
					if ((i & 1 << (k & 31)) == 1 << (k & 31))
					{
						int num2 = num1;
						num1 = num2 + 1;
						ArmorModelSlotUtility.Mask2SlotArray.FlagToSlotArray[i][num2] = (ArmorModelSlot)((byte)k);
					}
				}
			}
		}
	}

	private static class RendererNames
	{
		public readonly static string[] Array;

		static RendererNames()
		{
			ArmorModelSlotUtility.RendererNames.Array = new string[4];
			for (ArmorModelSlot i = ArmorModelSlot.Feet; (int)i < 4; i = (ArmorModelSlot)((byte)i + (byte)ArmorModelSlot.Legs))
			{
				ArmorModelSlotUtility.RendererNames.Array[(int)i] = string.Format("{0} Renderer", i);
			}
		}
	}
}