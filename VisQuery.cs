using System;
using System.Collections.Generic;
using UnityEngine;

public class VisQuery : ScriptableObject
{
	[SerializeField]
	protected VisEval evaluation;

	[SerializeField]
	protected VisAction[] actions;

	[SerializeField]
	protected bool nonInstance;

	public VisQuery()
	{
	}

	private void Enter(VisNode a, VisNode b)
	{
		IDMain dMain;
		IDMain dMain1 = a.idMain;
		if (!this.nonInstance)
		{
			dMain = b.idMain;
		}
		else
		{
			dMain = null;
		}
		IDMain dMain2 = dMain;
		for (int i = 0; i < (int)this.actions.Length; i++)
		{
			if (this.actions[i])
			{
				this.actions[i].Accomplish(dMain1, dMain2);
			}
		}
	}

	private void Exit(VisNode a, VisNode b)
	{
		IDMain dMain;
		IDMain dMain1 = a.idMain;
		if (!this.nonInstance)
		{
			dMain = b.idMain;
		}
		else
		{
			dMain = null;
		}
		IDMain dMain2 = dMain;
		for (int i = 0; i < (int)this.actions.Length; i++)
		{
			if (this.actions[i])
			{
				this.actions[i].UnAcomplish(dMain1, dMain2);
			}
		}
	}

	private bool Try(VisNode self, VisNode instigator)
	{
		Vis.Mask mask = self.traitMask;
		Vis.Mask mask1 = instigator.traitMask;
		return this.evaluation.Pass(mask, mask1);
	}

	public class Instance
	{
		public readonly VisQuery outer;

		private readonly HSet<VisNode> applicable;

		private readonly long bit;

		private readonly byte bitNumber;

		private int num;

		private int execNum;

		public int count
		{
			get
			{
				return this.num;
			}
		}

		internal Instance(VisQuery outer, ref int bit)
		{
			this.outer = outer;
			this.applicable = new HSet<VisNode>();
			this.bit = (long)(1 << (bit & 31));
			this.bitNumber = (byte)bit;
			bit = bit + 1;
		}

		public void Clear(VisNode self)
		{
			while (true)
			{
				VisQuery.Instance instance = this;
				int num = instance.num - 1;
				int num1 = num;
				instance.num = num;
				if (num1 < 0)
				{
					break;
				}
				HSetIter<VisNode> enumerator = this.applicable.GetEnumerator();
				enumerator.MoveNext();
				VisNode current = enumerator.Current;
				enumerator.Dispose();
				this.TryRemove(self, current);
			}
		}

		public void Execute(VisQuery.TryResult res, VisNode self, VisNode other)
		{
			switch (res)
			{
				case VisQuery.TryResult.Enter:
				{
					this.ExecuteEnter(self, other);
					break;
				}
				case VisQuery.TryResult.Stay:
				{
					break;
				}
				case VisQuery.TryResult.Exit:
				{
					this.ExecuteExit(self, other);
					break;
				}
				default:
				{
					goto case VisQuery.TryResult.Stay;
				}
			}
		}

		public void ExecuteEnter(VisNode self, VisNode other)
		{
			VisQuery.Instance instance = this;
			int num = instance.execNum;
			int num1 = num;
			instance.execNum = num + 1;
			if (num1 == 0 || !this.outer.nonInstance)
			{
				this.outer.Enter(self, other);
			}
		}

		public void ExecuteExit(VisNode self, VisNode other)
		{
			VisQuery.Instance instance = this;
			int num = instance.execNum - 1;
			int num1 = num;
			instance.execNum = num;
			if (num1 == 0 || !this.outer.nonInstance)
			{
				this.outer.Exit(self, other);
			}
		}

		public bool Fits(VisNode other)
		{
			return this.applicable.Contains(other);
		}

		public bool IsActive(long mask)
		{
			return (mask & this.bit) == this.bit;
		}

		public VisQuery.TryResult TryAdd(VisNode self, VisNode other)
		{
			if (!this.outer.Try(self, other))
			{
				return this.TryRemove(self, other);
			}
			if (!this.applicable.Add(other))
			{
				return VisQuery.TryResult.Stay;
			}
			VisQuery.Instance instance = this;
			instance.num = instance.num + 1;
			return VisQuery.TryResult.Enter;
		}

		public VisQuery.TryResult TryRemove(VisNode self, VisNode other)
		{
			if (!this.applicable.Remove(other))
			{
				return VisQuery.TryResult.Outside;
			}
			VisQuery.Instance instance = this;
			instance.num = instance.num - 1;
			return VisQuery.TryResult.Exit;
		}
	}

	public enum TryResult
	{
		Outside,
		Enter,
		Stay,
		Exit
	}
}