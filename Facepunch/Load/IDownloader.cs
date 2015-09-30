using System;
using UnityEngine;

namespace Facepunch.Load
{
	public interface IDownloader
	{
		void BeginJob(Job job);

		float GetDownloadProgress(Job job);

		AssetBundle GetLoadedAssetBundle(Job job);

		void OnJobCompleted(Job job);
	}
}