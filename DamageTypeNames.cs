using System;
using System.Collections.Generic;

public static class DamageTypeNames
{
	private readonly static string[] Strings;

	private readonly static string[] Flags;

	private readonly static Dictionary<string, DamageTypeIndex> Values;

	private readonly static DamageTypeFlags Mask;

	private readonly static char[] SplitCharacters;

	static DamageTypeNames()
	{
		DamageTypeNames.SplitCharacters = new char[] { '|' };
		DamageTypeNames.Strings = new string[6];
		DamageTypeNames.Values = new Dictionary<string, DamageTypeIndex>(6);
		for (DamageTypeIndex i = DamageTypeIndex.damage_generic; i < DamageTypeIndex.damage_last; i = (DamageTypeIndex)((int)i + (int)DamageTypeIndex.damage_bullet))
		{
			Dictionary<string, DamageTypeIndex> values = DamageTypeNames.Values;
			string[] strings = DamageTypeNames.Strings;
			string str = i.ToString().Substring("damage_".Length);
			string str1 = str;
			strings[(int)i] = str;
			values.Add(str1, i);
		}
		uint num = 63;
		DamageTypeNames.Mask = (DamageTypeFlags)num;
		DamageTypeNames.Flags = new string[num];
		DamageTypeNames.Flags[0] = "none";
		for (uint j = 1; j < num; j++)
		{
			uint num1 = j;
			int num2 = 0;
			while (num2 < 6)
			{
				if ((j & 1 << (num2 & 31)) != 1 << (num2 & 31))
				{
					num2++;
				}
				else
				{
					string strings1 = DamageTypeNames.Strings[num2];
					UInt32 num3 = num1 & ~(1 << (num2 & 31 & 31));
					num1 = num3;
					if (num3 != 0)
					{
						while (true)
						{
							int num4 = num2 + 1;
							num2 = num4;
							if ((long)num4 >= (long)6)
							{
								break;
							}
							if ((j & 1 << (num2 & 31)) == 1 << (num2 & 31))
							{
								strings1 = string.Concat(strings1, "|", DamageTypeNames.Strings[num2]);
								num1 = num1 & ~(1 << (num2 & 31 & 31));
								if (num1 == 0)
								{
									break;
								}
							}
						}
						break;
					}
					else
					{
						break;
					}
				}
			}
		}
	}

	public static bool Convert(string name, out DamageTypeIndex index)
	{
		return DamageTypeNames.Values.TryGetValue(name, out index);
	}

	public static bool Convert(string[] names, out DamageTypeFlags flags)
	{
		DamageTypeIndex damageTypeIndex;
		for (int i = 0; i < (int)names.Length; i++)
		{
			if (DamageTypeNames.Values.TryGetValue(names[i], out damageTypeIndex))
			{
				flags = (DamageTypeFlags)((int)DamageTypeIndex.damage_bullet << (int)(damageTypeIndex & (DamageTypeIndex.damage_bullet | DamageTypeIndex.damage_melee | DamageTypeIndex.damage_explosion | DamageTypeIndex.damage_radiation | DamageTypeIndex.damage_cold | DamageTypeIndex.damage_last)));
				while (true)
				{
					int num = i + 1;
					i = num;
					if (num >= (int)names.Length)
					{
						break;
					}
					if (DamageTypeNames.Values.TryGetValue(names[i], out damageTypeIndex))
					{
						flags = (DamageTypeFlags)((int)flags | (int)DamageTypeIndex.damage_bullet << (int)(damageTypeIndex & (DamageTypeIndex.damage_bullet | DamageTypeIndex.damage_melee | DamageTypeIndex.damage_explosion | DamageTypeIndex.damage_radiation | DamageTypeIndex.damage_cold | DamageTypeIndex.damage_last)));
					}
				}
				return true;
			}
		}
		flags = (DamageTypeFlags)0;
		return false;
	}

	public static bool Convert(string name, out DamageTypeFlags flags)
	{
		DamageTypeIndex damageTypeIndex;
		if (DamageTypeNames.Values.TryGetValue(name, out damageTypeIndex))
		{
			flags = (DamageTypeFlags)((int)DamageTypeIndex.damage_bullet << (int)(damageTypeIndex & (DamageTypeIndex.damage_bullet | DamageTypeIndex.damage_melee | DamageTypeIndex.damage_explosion | DamageTypeIndex.damage_radiation | DamageTypeIndex.damage_cold | DamageTypeIndex.damage_last)));
			return true;
		}
		if (name.Length == 0 || name == "none")
		{
			flags = (DamageTypeFlags)0;
			return true;
		}
		return DamageTypeNames.Convert(name.Split(DamageTypeNames.SplitCharacters, StringSplitOptions.RemoveEmptyEntries), out flags);
	}

	public static bool Convert(DamageTypeIndex index, out DamageTypeFlags flags)
	{
		flags = (DamageTypeFlags)((int)DamageTypeIndex.damage_bullet << (int)(index & (DamageTypeIndex.damage_bullet | DamageTypeIndex.damage_melee | DamageTypeIndex.damage_explosion | DamageTypeIndex.damage_radiation | DamageTypeIndex.damage_cold | DamageTypeIndex.damage_last)));
		return ((int)flags & (int)DamageTypeNames.Mask) == (int)flags;
	}

	public static bool Convert(DamageTypeIndex index, out string name)
	{
		if (index != DamageTypeIndex.damage_generic && (index <= DamageTypeIndex.damage_generic || index >= DamageTypeIndex.damage_last))
		{
			name = null;
			return false;
		}
		name = DamageTypeNames.Strings[(int)index];
		return true;
	}
}