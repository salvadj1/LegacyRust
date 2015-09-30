using System;
using UnityEngine;

namespace Facepunch.Load
{
	public class Group : IDownloadTask
	{
		[NonSerialized]
		public Operation _op;

		[NonSerialized]
		public Job[] Jobs;

		[NonSerialized]
		public int ByteLength;

		[NonSerialized]
		private string jobDesc;

		[NonSerialized]
		private string lastDescriptiveText;

		[NonSerialized]
		private Facepunch.Load.TaskStatusCount? lastStatusCount;

		public int ByteLengthDownloaded
		{
			get
			{
				int byteLengthDownloaded = 0;
				Job[] jobs = this.Jobs;
				for (int i = 0; i < (int)jobs.Length; i++)
				{
					byteLengthDownloaded = byteLengthDownloaded + jobs[i].ByteLengthDownloaded;
				}
				return byteLengthDownloaded;
			}
		}

		public string ContextualDescription
		{
			get
			{
				Facepunch.Load.TaskStatusCount taskStatusCount = this.TaskStatusCount;
				Facepunch.Load.TaskStatusCount? nullable = this.lastStatusCount;
				Facepunch.Load.TaskStatusCount taskStatusCount1 = (!nullable.HasValue ? taskStatusCount : nullable.Value);
				if (!this.lastStatusCount.HasValue || taskStatusCount1.Pending != taskStatusCount.Pending || taskStatusCount1.Complete != taskStatusCount.Complete || taskStatusCount1.Downloading != taskStatusCount.Downloading)
				{
					this.lastStatusCount = new Facepunch.Load.TaskStatusCount?(taskStatusCount);
					switch ((byte)(taskStatusCount.TaskStatus & (Facepunch.Load.TaskStatus.Pending | Facepunch.Load.TaskStatus.Downloading | Facepunch.Load.TaskStatus.Complete)))
					{
						case 1:
						{
							this.lastDescriptiveText = string.Format("{0} pending", taskStatusCount.Pending);
							return this.lastDescriptiveText;
						}
						case 2:
						{
							this.lastDescriptiveText = string.Format("{0} downloading", taskStatusCount.Downloading);
							return this.lastDescriptiveText;
						}
						case 3:
						{
							this.lastDescriptiveText = string.Format("{0} pending, {1} downloading", taskStatusCount.Pending, taskStatusCount.Downloading);
							return this.lastDescriptiveText;
						}
						case 4:
						{
							this.lastDescriptiveText = string.Format("{0} complete", taskStatusCount.Complete);
							return this.lastDescriptiveText;
						}
						case 5:
						{
							this.lastDescriptiveText = string.Format("{0} pending, {1} complete", taskStatusCount.Pending, taskStatusCount.Downloading);
							return this.lastDescriptiveText;
						}
						case 6:
						{
							this.lastDescriptiveText = string.Format("{0} downloading, {1} complete", taskStatusCount.Downloading, taskStatusCount.Complete);
							return this.lastDescriptiveText;
						}
						case 7:
						{
							this.lastDescriptiveText = string.Format("{0} pending, {1} downloading, {2} complete", taskStatusCount.Pending, taskStatusCount.Downloading, taskStatusCount.Complete);
							return this.lastDescriptiveText;
						}
					}
					throw new ArgumentException("TaskStatus");
				}
				return this.lastDescriptiveText;
			}
		}

		public int Count
		{
			get
			{
				return (int)this.Jobs.Length;
			}
		}

		public int Done
		{
			get
			{
				int num = 0;
				Job[] jobs = this.Jobs;
				for (int i = 0; i < (int)jobs.Length; i++)
				{
					if (jobs[i].TaskStatus == Facepunch.Load.TaskStatus.Complete)
					{
						num++;
					}
				}
				return num;
			}
		}

		int Facepunch.Load.IDownloadTask.ByteLength
		{
			get
			{
				return this.ByteLength;
			}
		}

		string Facepunch.Load.IDownloadTask.Name
		{
			get
			{
				return this.jobDesc;
			}
		}

		public Facepunch.Load.Loader Loader
		{
			get
			{
				return this._op.Loader;
			}
		}

		public float PercentDone
		{
			get
			{
				return (float)((double)this.ByteLengthDownloaded / (double)this.ByteLength);
			}
		}

		public Facepunch.Load.TaskStatus TaskStatus
		{
			get
			{
				Facepunch.Load.TaskStatus taskStatu = Facepunch.Load.TaskStatus.Complete;
				Job[] jobs = this.Jobs;
				for (int i = 0; i < (int)jobs.Length; i++)
				{
					Job job = jobs[i];
					if (job.TaskStatus == Facepunch.Load.TaskStatus.Downloading)
					{
						return Facepunch.Load.TaskStatus.Downloading;
					}
					if (job.TaskStatus == Facepunch.Load.TaskStatus.Pending)
					{
						taskStatu = Facepunch.Load.TaskStatus.Pending;
					}
				}
				return taskStatu;
			}
		}

		public Facepunch.Load.TaskStatusCount TaskStatusCount
		{
			get
			{
				Facepunch.Load.TaskStatusCount taskStatusCount = new Facepunch.Load.TaskStatusCount();
				Job[] jobs = this.Jobs;
				for (int i = 0; i < (int)jobs.Length; i++)
				{
					Facepunch.Load.TaskStatus taskStatus = jobs[i].TaskStatus;
					int item = (*stackVariable12)[taskStatus];
					taskStatusCount[taskStatus] = item + 1;
				}
				return taskStatusCount;
			}
		}

		public Group()
		{
		}

		internal void GetArrays(out AssetBundle[] assetBundles, out Item[] items)
		{
			items = new Item[(int)this.Jobs.Length];
			assetBundles = new AssetBundle[(int)this.Jobs.Length];
			for (int i = 0; i < (int)this.Jobs.Length; i++)
			{
				assetBundles[i] = this.Jobs[i].AssetBundle;
				items[i] = this.Jobs[i].Item;
			}
		}

		public void Initialize()
		{
			this.ByteLength = 0;
			Job[] jobs = this.Jobs;
			for (int i = 0; i < (int)jobs.Length; i++)
			{
				Job job = jobs[i];
				Group byteLength = this;
				byteLength.ByteLength = byteLength.ByteLength + job.Item.ByteLength;
			}
			int length = (int)this.Jobs.Length;
			if (length == 0)
			{
				this.jobDesc = "No bundles";
			}
			else if (length == 1)
			{
				this.jobDesc = "1 bundle";
			}
			else
			{
				this.jobDesc = string.Format("{0} bundles", (int)this.Jobs.Length);
			}
		}
	}
}