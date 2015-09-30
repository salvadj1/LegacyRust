using System;
using System.Runtime.CompilerServices;

public static class AnchorStyleExtensions
{
	public static bool IsAnyFlagSet(this dfAnchorStyle value, dfAnchorStyle flag)
	{
		return dfAnchorStyle.None != (value & flag);
	}

	public static bool IsFlagSet(this dfAnchorStyle value, dfAnchorStyle flag)
	{
		return flag == (value & flag);
	}

	public static dfAnchorStyle SetFlag(this dfAnchorStyle value, dfAnchorStyle flag)
	{
		return value | flag;
	}

	public static dfAnchorStyle SetFlag(this dfAnchorStyle value, dfAnchorStyle flag, bool on)
	{
		if (on)
		{
			return value | flag;
		}
		return value & ~flag;
	}
}