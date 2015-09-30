using System;

namespace Facepunch.Load
{
	public interface IDownloaderDescriptive : IDownloader
	{
		bool DescribeProgress(Job job, ref string lastString);
	}
}