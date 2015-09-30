using System;
using System.Collections.Generic;

public class dfMarkupTokenAttribute
{
	public int Index;

	public dfMarkupToken Key;

	public dfMarkupToken Value;

	private static List<dfMarkupTokenAttribute> pool;

	private static int poolIndex;

	static dfMarkupTokenAttribute()
	{
		dfMarkupTokenAttribute.pool = new List<dfMarkupTokenAttribute>();
		dfMarkupTokenAttribute.poolIndex = 0;
	}

	private dfMarkupTokenAttribute()
	{
	}

	internal static dfMarkupTokenAttribute GetAttribute(int index)
	{
		return dfMarkupTokenAttribute.pool[index];
	}

	public static dfMarkupTokenAttribute Obtain(dfMarkupToken key, dfMarkupToken value)
	{
		if (dfMarkupTokenAttribute.poolIndex >= dfMarkupTokenAttribute.pool.Count - 1)
		{
			dfMarkupTokenAttribute.pool.Add(new dfMarkupTokenAttribute());
		}
		dfMarkupTokenAttribute item = dfMarkupTokenAttribute.pool[dfMarkupTokenAttribute.poolIndex];
		item.Index = dfMarkupTokenAttribute.poolIndex;
		item.Key = key;
		item.Value = value;
		dfMarkupTokenAttribute.poolIndex = dfMarkupTokenAttribute.poolIndex + 1;
		return item;
	}

	public static void Reset()
	{
		dfMarkupTokenAttribute.poolIndex = 0;
	}
}