using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facepunch.Progress
{
	public sealed class ProgressBar
	{
		private readonly List<IProgress> List = new List<IProgress>();

		private float bonus;

		private float denom;

		private int count;

		public ProgressBar()
		{
		}

		public void Add(Facepunch.Progress.IProgress IProgress)
		{
			if (object.ReferenceEquals(IProgress, null))
			{
				return;
			}
			this.List.Add(IProgress);
			ProgressBar progressBar = this;
			progressBar.count = progressBar.count + 1;
			ProgressBar progressBar1 = this;
			progressBar1.denom = progressBar1.denom + 1f;
		}

		public void Add(AsyncOperation Progress)
		{
			if (!object.ReferenceEquals(Progress, null))
			{
				this.Add(new ProgressBar.AsyncOperationProgress(Progress));
			}
		}

		public void AddMultiple<T>(IEnumerable<T> collection)
		where T : IProgress
		{
			IEnumerator<T> enumerator = collection.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					this.Add(enumerator.Current);
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
		}

		public void Clean()
		{
			float single;
			this.Update(out single);
		}

		public void Clear()
		{
			float single = 0f;
			float single1 = single;
			this.denom = single;
			this.bonus = single1;
			this.List.Clear();
			this.count = 0;
		}

		public bool Update(out float progress)
		{
			float single;
			if (this.count == 0)
			{
				progress = 0f;
				return false;
			}
			float single1 = 0f;
			int num = 0;
			int num1 = this.count;
			int num2 = num1 - 1;
			while (num < num1)
			{
				if (!this.List[num2].Poll(out single) || single >= 1f)
				{
					ProgressBar progressBar = this;
					int num3 = progressBar.count - 1;
					int num4 = num3;
					progressBar.count = num3;
					if (num4 <= 0)
					{
						this.Clear();
						progress = 1f;
						return true;
					}
					ProgressBar progressBar1 = this;
					progressBar1.bonus = progressBar1.bonus + 1f;
					this.List.RemoveAt(num2);
				}
				else
				{
					single1 = single1 + single;
				}
				num++;
				num2--;
			}
			float single2 = (single1 + this.bonus) / this.denom;
			float single3 = single2;
			progress = single2;
			if (single3 > 1f)
			{
				progress = 1f;
			}
			return true;
		}

		private struct AsyncOperationProgress : IProgress
		{
			public readonly AsyncOperation aop;

			public float progress
			{
				get
				{
					return (this.aop == null || this.aop.isDone ? 1f : this.aop.progress * 0.999f);
				}
			}

			public AsyncOperationProgress(AsyncOperation aop)
			{
				this.aop = aop;
			}
		}
	}
}