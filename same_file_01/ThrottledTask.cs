using Facepunch.Clocks.Counters;
using Facepunch.Progress;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[AddComponentMenu("")]
public abstract class ThrottledTask : MonoBehaviour
{
	private const int kTargetMSPerFrame = 400;

	[NonSerialized]
	private bool working;

	[NonSerialized]
	private bool added;

	private static int numWorking;

	private static List<ThrottledTask> AllTasks;

	public static ThrottledTask[] AllWorkingTasks
	{
		get
		{
			ThrottledTask[] throttledTaskArray = new ThrottledTask[ThrottledTask.numWorking];
			int num = 0;
			foreach (ThrottledTask allTask in ThrottledTask.AllTasks)
			{
				if (!allTask.working)
				{
					continue;
				}
				int num1 = num;
				num = num1 + 1;
				throttledTaskArray[num1] = allTask;
				if (num != ThrottledTask.numWorking)
				{
					continue;
				}
				break;
			}
			return throttledTaskArray;
		}
	}

	public static IEnumerable<IProgress> AllWorkingTasksProgress
	{
		get
		{
			ThrottledTask.<>c__IteratorC variable = null;
			return variable;
		}
	}

	protected ThrottledTask.Timer Begin
	{
		get
		{
			return ThrottledTask.Timer.Start;
		}
	}

	public static bool Operational
	{
		get
		{
			return ThrottledTask.numWorking > 0;
		}
	}

	public bool Working
	{
		get
		{
			return this.working;
		}
		protected set
		{
			this.SetWorking(value);
		}
	}

	static ThrottledTask()
	{
		ThrottledTask.AllTasks = new List<ThrottledTask>();
	}

	protected ThrottledTask()
	{
	}

	protected void Awake()
	{
		if (!this.added)
		{
			this.added = true;
			ThrottledTask.AllTasks.Add(this);
		}
	}

	protected void OnDestroy()
	{
		if (!this.added)
		{
			this.added = true;
		}
		else
		{
			ThrottledTask.AllTasks.Remove(this);
		}
		this.SetWorking(false);
	}

	private void SetWorking(bool on)
	{
		if (on != this.working)
		{
			this.working = on;
			if (!on)
			{
				ThrottledTask.numWorking = ThrottledTask.numWorking - 1;
			}
			else
			{
				ThrottledTask.numWorking = ThrottledTask.numWorking + 1;
			}
		}
	}

	protected struct Timer
	{
		private readonly SystemTimestamp clock;

		public bool Continue
		{
			get
			{
				bool totalMilliseconds;
				if (ThrottledTask.numWorking == 0)
				{
					totalMilliseconds = true;
				}
				else
				{
					TimeSpan elapsed = this.clock.Elapsed;
					totalMilliseconds = elapsed.TotalMilliseconds < 400 / (double)ThrottledTask.numWorking;
				}
				return totalMilliseconds;
			}
		}

		internal static ThrottledTask.Timer Start
		{
			get
			{
				return new ThrottledTask.Timer(SystemTimestamp.Restart);
			}
		}

		private Timer(SystemTimestamp clock)
		{
			this.clock = clock;
		}
	}
}