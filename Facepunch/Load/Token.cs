using System;

namespace Facepunch.Load
{
	public enum Token
	{
		Uninitialized,
		DownloadQueueBegin,
		RandomLoadOrderAreaBegin,
		BundleListing,
		RandomLoadOrderAreaEnd,
		DownloadQueueEnd,
		End
	}
}