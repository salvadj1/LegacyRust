using System;

namespace Facepunch.Load
{
	[Flags]
	public enum TaskStatus : byte
	{
		Pending = 1,
		Downloading = 2,
		Complete = 4
	}
}