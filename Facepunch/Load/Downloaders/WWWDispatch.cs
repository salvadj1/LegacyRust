using Facepunch.Load;
using System;
using UnityEngine;

namespace Facepunch.Load.Downloaders
{
	public sealed class WWWDispatch : IDownloaderDispatch
	{
		private readonly Facepunch.Load.Utility.ReferenceCountedCoroutine.Runner coroutineRunner = new Facepunch.Load.Utility.ReferenceCountedCoroutine.Runner("WWWDispatch");

		public WWWDispatch()
		{
		}

		public void BindLoader(Loader loader)
		{
			this.coroutineRunner.Retain();
		}

		public IDownloader CreateDownloaderForJob(Job job)
		{
			return new WWWDispatch.Downloader(this);
		}

		public void DeleteDownloader(Job job, IDownloader idownloader)
		{
			if (idownloader is WWWDispatch.Downloader)
			{
				WWWDispatch.Downloader downloader = (WWWDispatch.Downloader)idownloader;
				downloader.Job = null;
				downloader.Download = null;
			}
		}

		private void DownloadBegin(WWWDispatch.Downloader downloader, Job job)
		{
			downloader.Job = job;
			downloader.Download = new WWW(job.Path);
			job.OnDownloadingBegin(downloader);
			if (!downloader.Download.isDone)
			{
				this.coroutineRunner.Install(WWWDispatch.Downloader.DownloaderRoutineCallback, downloader, downloader.Download, true);
			}
			else
			{
				this.DownloadFinished(downloader);
			}
		}

		private void DownloadFinished(WWWDispatch.Downloader downloader)
		{
			downloader.Job.OnDownloadingComplete();
		}

		public void UnbindLoader(Loader loader)
		{
			this.coroutineRunner.Release();
		}

		private class Downloader : IDownloader
		{
			public readonly WWWDispatch Dispatch;

			public WWW Download;

			public Job Job;

			public readonly static Facepunch.Load.Utility.ReferenceCountedCoroutine.Callback DownloaderRoutineCallback;

			static Downloader()
			{
				WWWDispatch.Downloader.DownloaderRoutineCallback = new Facepunch.Load.Utility.ReferenceCountedCoroutine.Callback(WWWDispatch.Downloader.DownloaderRoutine);
			}

			public Downloader(WWWDispatch dispatch)
			{
				this.Dispatch = dispatch;
			}

			public void BeginJob(Job job)
			{
				this.Dispatch.DownloadBegin(this, job);
			}

			private static bool DownloaderRoutine(ref object yieldInstruction, ref object tag)
			{
				WWWDispatch.Downloader downloader = (WWWDispatch.Downloader)tag;
				yieldInstruction = downloader.Download;
				if (!downloader.Download.isDone)
				{
					return true;
				}
				downloader.Dispatch.DownloadFinished(downloader);
				return false;
			}

			public float GetDownloadProgress(Job job)
			{
				return (this.Download != null ? this.Download.progress : 0f);
			}

			public AssetBundle GetLoadedAssetBundle(Job job)
			{
				return this.Download.assetBundle;
			}

			public void OnJobCompleted(Job job)
			{
				if (this.Job == job)
				{
					this.Download.Dispose();
					this.Download = null;
					this.Job = null;
				}
			}
		}
	}
}