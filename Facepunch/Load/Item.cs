using System;

namespace Facepunch.Load
{
	public struct Item
	{
		public string Name;

		public string Path;

		public int ByteLength;

		public Type TypeOfAssets;

		public Facepunch.Load.ContentType ContentType;
	}
}