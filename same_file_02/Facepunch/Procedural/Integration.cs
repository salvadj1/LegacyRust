using System;

namespace Facepunch.Procedural
{
	[Flags]
	public enum Integration : byte
	{
		Stationary = 1,
		Moved = 2,
		MovedDestination = 3,
		Ahead = 4
	}
}