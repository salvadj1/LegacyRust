using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Facepunch.Load
{
	public sealed class Job : IDownloadTask
	{
		[NonSerialized]
		public Operation _op;

		[NonSerialized]
		public Facepunch.Load.Item Item;

		private IDownloaderDescriptive descriptiveDownloader;

		private bool hasDescriptiveDownloader;

		private string lastDescriptiveString;

		public object Tag;

		private IDownloader downloader;

		internal UnityEngine.AssetBundle AssetBundle
		{
			get;
			set;
		}

		public int ByteLength
		{
			get
			{
				return this.Item.ByteLength;
			}
		}

		public int ByteLengthDownloaded
		{
			get
			{
				return Mathf.FloorToInt(this.PercentDone * (float)this.ByteLength);
			}
		}

		public Facepunch.Load.ContentType ContentType
		{
			get
			{
				return this.Item.ContentType;
			}
		}

		public string ContextualDescription
		{
			get
			{
				switch (this.TaskStatus)
				{
					case Facepunch.Load.TaskStatus.Pending:
					{
						return "Pending";
					}
					case Facepunch.Load.TaskStatus.Downloading:
					{
						return (!this.hasDescriptiveDownloader || !this.descriptiveDownloader.DescribeProgress(this, ref this.lastDescriptiveString) ? "Downloading" : this.lastDescriptiveString ?? string.Empty);
					}
					case Facepunch.Load.TaskStatus.Pending | Facepunch.Load.TaskStatus.Downloading:
					{
						throw new ArgumentOutOfRangeException("TaskStatus");
					}
					case Facepunch.Load.TaskStatus.Complete:
					{
						return "Complete";
					}
					default:
					{
						throw new ArgumentOutOfRangeException("TaskStatus");
					}
				}
			}
		}

		int Facepunch.Load.IDownloadTask.Count
		{
			get
			{
				return 1;
			}
		}

		int Facepunch.Load.IDownloadTask.Done
		{
			get
			{
				return (this.TaskStatus != Facepunch.Load.TaskStatus.Complete ? 0 : 1);
			}
		}

		Facepunch.Load.TaskStatusCount Facepunch.Load.IDownloadTask.TaskStatusCount
		{
			get
			{
				switch (this.TaskStatus)
				{
					case Facepunch.Load.TaskStatus.Pending:
					{
						return Facepunch.Load.TaskStatusCount.OnePending;
					}
					case Facepunch.Load.TaskStatus.Downloading:
					{
						return Facepunch.Load.TaskStatusCount.OneDownloading;
					}
					case Facepunch.Load.TaskStatus.Pending | Facepunch.Load.TaskStatus.Downloading:
					{
						throw new ArgumentOutOfRangeException("TaskStatus");
					}
					case Facepunch.Load.TaskStatus.Complete:
					{
						return Facepunch.Load.TaskStatusCount.OneComplete;
					}
					default:
					{
						throw new ArgumentOutOfRangeException("TaskStatus");
					}
				}
			}
		}

		public Facepunch.Load.Group Group
		{
			get;
			internal set;
		}

		public Facepunch.Load.Loader Loader
		{
			get
			{
				return this._op.Loader;
			}
		}

		public string Name
		{
			get
			{
				return this.Item.Name;
			}
		}

		public string Path
		{
			get
			{
				return this.Item.Path;
			}
		}

		public float PercentDone
		{
			get
			{
				switch (this.TaskStatus)
				{
					case Facepunch.Load.TaskStatus.Pending:
					{
						return 0f;
					}
					case Facepunch.Load.TaskStatus.Downloading:
					{
						return this.downloader.GetDownloadProgress(this);
					}
					case Facepunch.Load.TaskStatus.Pending | Facepunch.Load.TaskStatus.Downloading:
					{
						throw new ArgumentOutOfRangeException("TaskStatus");
					}
					case Facepunch.Load.TaskStatus.Complete:
					{
						return 1f;
					}
					default:
					{
						throw new ArgumentOutOfRangeException("TaskStatus");
					}
				}
			}
		}

		public Facepunch.Load.TaskStatus TaskStatus
		{
			get
			{
				return JustDecompileGenerated_get_TaskStatus();
			}
			set
			{
				JustDecompileGenerated_set_TaskStatus(value);
			}
		}

		private Facepunch.Load.TaskStatus JustDecompileGenerated_TaskStatus_k__BackingField;

		public Facepunch.Load.TaskStatus JustDecompileGenerated_get_TaskStatus()
		{
			return this.JustDecompileGenerated_TaskStatus_k__BackingField;
		}

		private void JustDecompileGenerated_set_TaskStatus(Facepunch.Load.TaskStatus value)
		{
			this.JustDecompileGenerated_TaskStatus_k__BackingField = value;
		}

		public Type TypeOfAssets
		{
			get
			{
				return this.Item.TypeOfAssets;
			}
		}

		public Job()
		{
			this.TaskStatus = Facepunch.Load.TaskStatus.Pending;
		}

		public void OnDownloadingBegin(IDownloader downloader)
		{
			this.downloader = downloader;
			this.lastDescriptiveString = null;
			IDownloaderDescriptive downloaderDescriptive = downloader as IDownloaderDescriptive;
			IDownloaderDescriptive downloaderDescriptive1 = downloaderDescriptive;
			this.descriptiveDownloader = downloaderDescriptive;
			this.hasDescriptiveDownloader = downloaderDescriptive1 != null;
			this.TaskStatus = Facepunch.Load.TaskStatus.Downloading;
		}

		public void OnDownloadingComplete()
		{
			this.TaskStatus = Facepunch.Load.TaskStatus.Complete;
			IDownloader downloader = this.downloader;
			this.downloader = null;
			this.descriptiveDownloader = null;
			this.hasDescriptiveDownloader = false;
			this.Loader.OnJobCompleted(this, downloader);
			if (!object.ReferenceEquals(this.Tag, null))
			{
				Debug.LogWarning("Clearing tag manually ( should have been done by the IDownloader during the OnJobComplete callback )");
			}
			this.Tag = null;
		}
	}
}