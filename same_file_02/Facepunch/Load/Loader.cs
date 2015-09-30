using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Facepunch.Load
{
	public sealed class Loader : IDisposable, IDownloadTask
	{
		[NonSerialized]
		private readonly Group MasterGroup;

		[NonSerialized]
		public readonly Group[] Groups;

		[NonSerialized]
		public readonly Job[] Jobs;

		[NonSerialized]
		private int jobsCompleted;

		[NonSerialized]
		private int currentGroup = -1;

		[NonSerialized]
		private int jobsCompletedInGroup;

		[NonSerialized]
		private bool disposed;

		[NonSerialized]
		private IDownloaderDispatch Dispatch;

		private AssetBundleLoadedEventHandler OnAssetBundleLoaded;

		private MultipleAssetBundlesLoadedEventHandler OnAllAssetBundlesLoaded;

		private MultipleAssetBundlesLoadedEventHandler OnGroupedAssetBundlesLoaded;

		public int ByteLength
		{
			get
			{
				return this.MasterGroup.ByteLength;
			}
		}

		public int ByteLengthDownloaded
		{
			get
			{
				return this.MasterGroup.ByteLengthDownloaded;
			}
		}

		public int Count
		{
			get
			{
				return this.MasterGroup.Count;
			}
		}

		public Group CurrentGroup
		{
			get
			{
				Group groups;
				if (this.currentGroup < 0 || this.currentGroup >= (int)this.Groups.Length)
				{
					groups = null;
				}
				else
				{
					groups = this.Groups[this.currentGroup];
				}
				return groups;
			}
		}

		public int Done
		{
			get
			{
				return this.MasterGroup.Done;
			}
		}

		string Facepunch.Load.IDownloadTask.ContextualDescription
		{
			get
			{
				return this.MasterGroup.ContextualDescription;
			}
		}

		string Facepunch.Load.IDownloadTask.Name
		{
			get
			{
				return "Loading all bundles";
			}
		}

		public float PercentDone
		{
			get
			{
				return this.MasterGroup.PercentDone;
			}
		}

		public Facepunch.Load.TaskStatus TaskStatus
		{
			get
			{
				return this.MasterGroup.TaskStatus;
			}
		}

		public Facepunch.Load.TaskStatusCount TaskStatusCount
		{
			get
			{
				return this.MasterGroup.TaskStatusCount;
			}
		}

		public IEnumerator WaitEnumerator
		{
			get
			{
				Loader.<>c__Iterator25 variable = null;
				return variable;
			}
		}

		private Loader(Group masterGroup, Job[] allJobs, Group[] allGroups, IDownloaderDispatch dispatch)
		{
			this.MasterGroup = masterGroup;
			this.Jobs = allJobs;
			this.Groups = allGroups;
			this.Dispatch = dispatch;
		}

		public static Loader Create(Reader reader, IDownloaderDispatch dispatch)
		{
			return Loader.Deserialize(reader, dispatch);
		}

		public static Loader CreateFromFile(string downloadListPath, string bundlePath, IDownloaderDispatch dispatch)
		{
			Loader loader;
			using (Reader reader = Reader.CreateFromFile(downloadListPath, bundlePath))
			{
				loader = Loader.Deserialize(reader, dispatch);
			}
			return loader;
		}

		public static Loader CreateFromFile(string downloadListPath, IDownloaderDispatch dispatch)
		{
			Loader loader;
			using (Reader reader = Reader.CreateFromFile(downloadListPath))
			{
				loader = Loader.Deserialize(reader, dispatch);
			}
			return loader;
		}

		public static Loader CreateFromReader(TextReader textReader, string bundlePath, IDownloaderDispatch dispatch)
		{
			Loader loader;
			using (Reader reader = Reader.CreateFromReader(textReader, bundlePath))
			{
				loader = Loader.Deserialize(reader, dispatch);
			}
			return loader;
		}

		public static Loader CreateFromReader(JsonReader jsonReader, string bundlePath, IDownloaderDispatch dispatch)
		{
			Loader loader;
			using (Reader reader = Reader.CreateFromReader(jsonReader, bundlePath))
			{
				loader = Loader.Deserialize(reader, dispatch);
			}
			return loader;
		}

		public static Loader CreateFromText(string downloadListJson, string bundlePath, IDownloaderDispatch dispatch)
		{
			Loader loader;
			using (Reader reader = Reader.CreateFromText(downloadListJson, bundlePath))
			{
				loader = Loader.Deserialize(reader, dispatch);
			}
			return loader;
		}

		private static Loader Deserialize(Reader reader, IDownloaderDispatch dispatch)
		{
			List<Item[]> itemArrays = new List<Item[]>();
			List<Item> items = new List<Item>();
			while (reader.Read())
			{
				switch (reader.Token)
				{
					case Token.RandomLoadOrderAreaBegin:
					{
						items.Clear();
						continue;
					}
					case Token.BundleListing:
					{
						items.Add(reader.Item);
						continue;
					}
					case Token.RandomLoadOrderAreaEnd:
					{
						itemArrays.Add(items.ToArray());
						continue;
					}
					case Token.DownloadQueueEnd:
					{
						Operation operation = new Operation();
						int length = 0;
						foreach (Item[] itemArray in itemArrays)
						{
							length = length + (int)itemArray.Length;
						}
						Job[] jobArray = new Job[length];
						int num = 0;
						foreach (Item[] itemArray1 in itemArrays)
						{
							Item[] itemArray2 = itemArray1;
							for (int i = 0; i < (int)itemArray2.Length; i++)
							{
								Item item = itemArray2[i];
								int num1 = num;
								num = num1 + 1;
								Job job = new Job()
								{
									_op = operation,
									Item = item
								};
								jobArray[num1] = job;
							}
						}
						Group group = new Group()
						{
							_op = operation,
							Jobs = jobArray
						};
						group.Initialize();
						Group[] groupArray = new Group[itemArrays.Count];
						int num2 = 0;
						int num3 = 0;
						foreach (Item[] itemArray3 in itemArrays)
						{
							int length1 = (int)itemArray3.Length;
							Job[] jobArray1 = new Job[length1];
							for (int j = 0; j < length1; j++)
							{
								int num4 = num2;
								num2 = num4 + 1;
								jobArray1[j] = jobArray[num4];
							}
							groupArray[num3] = new Group();
							groupArray[num3]._op = operation;
							groupArray[num3].Jobs = jobArray1;
							groupArray[num3].Initialize();
							for (int k = 0; k < length1; k++)
							{
								jobArray1[k].Group = groupArray[num3];
							}
							num3++;
						}
						Loader loader = new Loader(group, jobArray, groupArray, dispatch);
						Loader loader1 = loader;
						operation.Loader = loader;
						return loader1;
					}
					default:
					{
						continue;
					}
				}
			}
			throw new InvalidProgramException();
		}

		public void Dispose()
		{
			if (!this.disposed)
			{
				this.disposed = true;
				this.DisposeDispatch();
			}
		}

		private void DisposeDispatch()
		{
			if (this.Dispatch != null)
			{
				IDownloaderDispatch dispatch = this.Dispatch;
				this.Dispatch = null;
				dispatch.UnbindLoader(this);
			}
		}

		internal void OnJobCompleted(Job job, IDownloader downloader)
		{
			Item[] itemArray;
			AssetBundle[] assetBundleArray;
			Item[] itemArray1;
			AssetBundle[] assetBundleArray1;
			job.AssetBundle = downloader.GetLoadedAssetBundle(job);
			if (this.OnAssetBundleLoaded != null)
			{
				try
				{
					this.OnAssetBundleLoaded(job.AssetBundle, job.Item);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
			}
			downloader.OnJobCompleted(job);
			this.Dispatch.DeleteDownloader(job, downloader);
			Loader loader = this;
			int num = loader.jobsCompleted + 1;
			int num1 = num;
			loader.jobsCompleted = num;
			if (num1 != this.MasterGroup.Count)
			{
				Loader loader1 = this;
				int num2 = loader1.jobsCompletedInGroup + 1;
				num1 = num2;
				loader1.jobsCompletedInGroup = num2;
				if (num1 == (int)this.Groups[this.currentGroup].Jobs.Length)
				{
					if (this.OnGroupedAssetBundlesLoaded != null)
					{
						this.Groups[this.currentGroup].GetArrays(out assetBundleArray1, out itemArray1);
						try
						{
							this.OnGroupedAssetBundlesLoaded(assetBundleArray1, itemArray1);
						}
						catch (Exception exception1)
						{
							UnityEngine.Debug.LogException(exception1);
						}
					}
					this.StartNextGroup();
				}
			}
			else
			{
				if (this.OnAllAssetBundlesLoaded != null)
				{
					this.MasterGroup.GetArrays(out assetBundleArray, out itemArray);
					try
					{
						this.OnAllAssetBundlesLoaded(assetBundleArray, itemArray);
					}
					catch (Exception exception2)
					{
						UnityEngine.Debug.LogException(exception2);
					}
				}
				this.DisposeDispatch();
			}
		}

		private void StartJob(Job job)
		{
			this.Dispatch.CreateDownloaderForJob(job).BeginJob(job);
		}

		public void StartLoading()
		{
			if (this.currentGroup == -1)
			{
				this.Dispatch.BindLoader(this);
				if ((int)this.Groups.Length > 0)
				{
					this.StartNextGroup();
				}
			}
		}

		private void StartNextGroup()
		{
			this.jobsCompletedInGroup = 0;
			Loader loader = this;
			int num = loader.currentGroup + 1;
			int num1 = num;
			loader.currentGroup = num;
			Job[] jobs = this.Groups[num1].Jobs;
			for (int i = 0; i < (int)jobs.Length; i++)
			{
				this.StartJob(jobs[i]);
			}
		}

		public event MultipleAssetBundlesLoadedEventHandler OnAllAssetBundlesLoaded
		{
			add
			{
				this.OnAllAssetBundlesLoaded += value;
			}
			remove
			{
				this.OnAllAssetBundlesLoaded -= value;
			}
		}

		public event AssetBundleLoadedEventHandler OnAssetBundleLoaded
		{
			add
			{
				this.OnAssetBundleLoaded += value;
			}
			remove
			{
				this.OnAssetBundleLoaded -= value;
			}
		}

		public event MultipleAssetBundlesLoadedEventHandler OnGroupedAssetBundlesLoaded
		{
			add
			{
				this.OnGroupedAssetBundlesLoaded += value;
			}
			remove
			{
				this.OnGroupedAssetBundlesLoaded -= value;
			}
		}
	}
}