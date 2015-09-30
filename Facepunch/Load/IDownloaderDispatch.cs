using System;

namespace Facepunch.Load
{
	public interface IDownloaderDispatch
	{
		void BindLoader(Loader loader);

		IDownloader CreateDownloaderForJob(Job job);

		void DeleteDownloader(Job job, IDownloader downloader);

		void UnbindLoader(Loader loader);
	}
}