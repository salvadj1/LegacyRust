using System;
using System.Reflection;

namespace Facepunch.Load
{
	public struct TaskStatusCount
	{
		public int Pending;

		public int Downloading;

		public int Complete;

		public readonly static TaskStatusCount OnePending;

		public readonly static TaskStatusCount OneDownloading;

		public readonly static TaskStatusCount OneComplete;

		public bool CompletelyComplete
		{
			get
			{
				bool flag;
				if (this.Complete <= 0)
				{
					flag = false;
				}
				else
				{
					flag = (this.Downloading != 0 ? false : this.Pending == 0);
				}
				return flag;
			}
		}

		public bool CompletelyDownloading
		{
			get
			{
				bool flag;
				if (this.Downloading <= 0)
				{
					flag = false;
				}
				else
				{
					flag = (this.Pending != 0 ? false : this.Complete == 0);
				}
				return flag;
			}
		}

		public bool CompletelyPending
		{
			get
			{
				bool flag;
				if (this.Pending <= 0)
				{
					flag = false;
				}
				else
				{
					flag = (this.Downloading != 0 ? false : this.Complete == 0);
				}
				return flag;
			}
		}

		public int this[Facepunch.Load.TaskStatus status]
		{
			get
			{
				switch (status)
				{
					case Facepunch.Load.TaskStatus.Pending:
					{
						return this.Pending;
					}
					case Facepunch.Load.TaskStatus.Downloading:
					{
						return this.Downloading;
					}
					case Facepunch.Load.TaskStatus.Pending | Facepunch.Load.TaskStatus.Downloading:
					{
						return this.Pending + this.Downloading;
					}
					case Facepunch.Load.TaskStatus.Complete:
					{
						return this.Complete;
					}
					case Facepunch.Load.TaskStatus.Pending | Facepunch.Load.TaskStatus.Complete:
					{
						return this.Pending + this.Complete;
					}
					case Facepunch.Load.TaskStatus.Downloading | Facepunch.Load.TaskStatus.Complete:
					{
						return this.Pending + this.Downloading;
					}
					case Facepunch.Load.TaskStatus.Pending | Facepunch.Load.TaskStatus.Downloading | Facepunch.Load.TaskStatus.Complete:
					{
						return this.Pending + this.Downloading + this.Complete;
					}
				}
				return 0;
			}
			set
			{
				int num;
				switch (status)
				{
					case Facepunch.Load.TaskStatus.Pending:
					{
						this.Pending = value;
						break;
					}
					case Facepunch.Load.TaskStatus.Downloading:
					{
						this.Downloading = value;
						break;
					}
					case Facepunch.Load.TaskStatus.Pending | Facepunch.Load.TaskStatus.Downloading:
					{
						int num1 = value;
						num = num1;
						this.Pending = num1;
						this.Downloading = num;
						break;
					}
					case Facepunch.Load.TaskStatus.Complete:
					{
						this.Complete = value;
						break;
					}
					case Facepunch.Load.TaskStatus.Pending | Facepunch.Load.TaskStatus.Complete:
					{
						int num2 = value;
						num = num2;
						this.Pending = num2;
						this.Complete = num;
						break;
					}
					case Facepunch.Load.TaskStatus.Downloading | Facepunch.Load.TaskStatus.Complete:
					{
						int num3 = value;
						num = num3;
						this.Complete = num3;
						this.Downloading = num;
						break;
					}
					case Facepunch.Load.TaskStatus.Pending | Facepunch.Load.TaskStatus.Downloading | Facepunch.Load.TaskStatus.Complete:
					{
						int num4 = value;
						num = num4;
						this.Downloading = num4;
						int num5 = num;
						num = num5;
						this.Pending = num5;
						this.Complete = num;
						break;
					}
				}
			}
		}

		public float PercentComplete
		{
			get
			{
				return (this.Complete != 0 ? (float)((double)this.Remaining / (double)this.Total) : 0f);
			}
		}

		public float PercentDownloading
		{
			get
			{
				return ((float)this.Downloading != 0f ? (float)((double)this.Downloading / (double)this.Total) : 0f);
			}
		}

		public float PercentPending
		{
			get
			{
				return (this.Pending != 0 ? (float)((double)this.Pending / (double)this.Total) : 0f);
			}
		}

		public int Remaining
		{
			get
			{
				return this.Pending + this.Downloading;
			}
		}

		public Facepunch.Load.TaskStatus TaskStatus
		{
			get
			{
				if (this.Pending > 0)
				{
					if (this.Downloading > 0)
					{
						if (this.Complete > 0)
						{
							return Facepunch.Load.TaskStatus.Pending | Facepunch.Load.TaskStatus.Downloading | Facepunch.Load.TaskStatus.Complete;
						}
						return Facepunch.Load.TaskStatus.Pending | Facepunch.Load.TaskStatus.Downloading;
					}
					if (this.Complete > 0)
					{
						return Facepunch.Load.TaskStatus.Pending | Facepunch.Load.TaskStatus.Complete;
					}
					return Facepunch.Load.TaskStatus.Pending;
				}
				if (this.Downloading > 0)
				{
					if (this.Complete > 0)
					{
						return Facepunch.Load.TaskStatus.Downloading | Facepunch.Load.TaskStatus.Complete;
					}
					return Facepunch.Load.TaskStatus.Downloading;
				}
				if (this.Complete > 0)
				{
					return Facepunch.Load.TaskStatus.Complete;
				}
				return 0;
			}
		}

		public int Total
		{
			get
			{
				return this.Pending + this.Downloading + this.Complete;
			}
		}

		static TaskStatusCount()
		{
			TaskStatusCount.OnePending = new TaskStatusCount()
			{
				Pending = 1
			};
			TaskStatusCount.OneDownloading = new TaskStatusCount()
			{
				Downloading = 1
			};
			TaskStatusCount.OneComplete = new TaskStatusCount()
			{
				Complete = 1
			};
		}
	}
}