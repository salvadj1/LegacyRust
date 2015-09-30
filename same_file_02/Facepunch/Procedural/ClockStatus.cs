using System;

namespace Facepunch.Procedural
{
	[Flags]
	public enum ClockStatus : byte
	{
		Unset = 0,
		Elapsed = 1,
		WillElapse = 2,
		DidElapse = 3,
		Negative = 4
	}
}