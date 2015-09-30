using System;
using System.Runtime.CompilerServices;
using uLink;

namespace InventoryExtensions
{
	public static class inventory_helper
	{
		public static int ReadInvInt(this BitStream stream)
		{
			return stream.ReadByte();
		}

		public static void WriteInvInt(this BitStream stream, int i)
		{
			stream.WriteByte((byte)i);
		}

		public static void WriteInvInt(this BitStream stream, byte i)
		{
			stream.WriteByte(i);
		}
	}
}