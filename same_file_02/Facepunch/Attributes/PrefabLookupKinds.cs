using System;

namespace Facepunch.Attributes
{
	[Flags]
	public enum PrefabLookupKinds
	{
		Controllable = 4,
		Character = 6,
		NetMain = 7,
		NGC = 8,
		Net = 15,
		Bundled = 16,
		All = 31
	}
}