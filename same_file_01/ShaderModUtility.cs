using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class ShaderModUtility
{
	public static IEnumerable<ShaderMod.KV> MergeKeyValues(this ShaderMod[] mods, ShaderMod.Replacement replacement)
	{
		return null;
	}

	public static int Replace(this ShaderMod[] mods, ShaderMod.Replacement replacement, string incoming, ref string outgoing)
	{
		if (mods != null)
		{
			int length = (int)mods.Length;
			for (int i = 0; i < length; i++)
			{
				if (mods[i] && mods[i].Replace(replacement, incoming, ref outgoing))
				{
					return i;
				}
			}
		}
		return -1;
	}

	public static int ReplaceReverse(this ShaderMod[] mods, ShaderMod.Replacement replacement, string incoming, ref string outgoing)
	{
		if (mods != null)
		{
			for (int i = (int)mods.Length - 1; i >= 0; i--)
			{
				if (mods[i] && mods[i].Replace(replacement, incoming, ref outgoing))
				{
					return i;
				}
			}
		}
		return -1;
	}
}