using System;

namespace Facepunch.Load
{
	public abstract class Stream : IDisposable
	{
		protected Stream()
		{
		}

		public abstract void Dispose();

		protected static class Property
		{
			public const string Path = "filename";

			public const string TypeOfAssets = "type";

			public const string ContentType = "content";

			public const string ByteLength = "size";

			public const string RelativeOrAbsoluteURL = "url";
		}
	}
}