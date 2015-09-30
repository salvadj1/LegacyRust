using System;
using System.Collections.Generic;

public sealed class TempList<T> : List<T>, IDisposable
{
	private static TempList<T> dump;

	private static int dumpCount;

	private static TempList<T> lastActive;

	private static TempList<T> firstActive;

	private static int activeCount;

	private TempList<T> prev;

	private TempList<T> next;

	private bool inDump;

	private bool active;

	private bool p;

	private bool n;

	private TempList()
	{
	}

	private TempList(IEnumerable<T> enumerable) : base(enumerable)
	{
	}

	private void Activate()
	{
		if (!this.active)
		{
			if (this.inDump)
			{
				throw new InvalidOperationException();
			}
			if (TempList<T>.activeCount == 0)
			{
				TempList<T> tempList = this;
				TempList<T>.firstActive = tempList;
				TempList<T>.lastActive = tempList;
				int num = 0;
				bool flag = (bool)num;
				this.n = (bool)num;
				this.p = flag;
				object obj = null;
				TempList<T> tempList1 = (TempList<T>)obj;
				this.next = (TempList<T>)obj;
				this.prev = tempList1;
			}
			else if (TempList<T>.activeCount != 1)
			{
				this.p = true;
				this.n = false;
				this.prev = TempList<T>.lastActive;
				TempList<T>.lastActive.n = true;
				TempList<T>.lastActive.next = this;
				TempList<T>.lastActive = this;
				this.next = null;
			}
			else
			{
				TempList<T>.lastActive = this;
				this.p = true;
				this.n = false;
				this.prev = TempList<T>.firstActive;
				this.next = null;
				TempList<T>.firstActive.n = true;
				TempList<T>.firstActive.next = this;
			}
			TempList<T>.activeCount = TempList<T>.activeCount + 1;
			this.active = true;
		}
	}

	private void Bin()
	{
		if (!this.inDump)
		{
			if (this.active)
			{
				throw new InvalidOperationException();
			}
			this.next = TempList<T>.dump;
			int num = TempList<T>.dumpCount;
			TempList<T>.dumpCount = num + 1;
			if (num != 0)
			{
				TempList<T>.dump.prev = this;
			}
			TempList<T>.dump = this;
			this.inDump = true;
			this.Clear();
		}
	}

	private void Deactivate()
	{
		if (this.active)
		{
			if (this.inDump)
			{
				throw new InvalidOperationException();
			}
			if (TempList<T>.lastActive == this)
			{
				if (TempList<T>.firstActive == this)
				{
					TempList<T>.lastActive = null;
					TempList<T>.firstActive = null;
				}
				else
				{
					TempList<T>.lastActive = this.prev;
					this.prev.n = false;
					this.prev.next = null;
				}
			}
			else if (TempList<T>.firstActive != this)
			{
				this.prev.next = this.next;
				this.next.prev = this.prev;
			}
			else
			{
				this.next.p = false;
				this.next.prev = null;
				TempList<T>.firstActive = this.next;
			}
			this.prev = null;
			this.next = null;
			this.p = false;
			this.n = false;
			this.active = false;
			TempList<T>.activeCount = TempList<T>.activeCount - 1;
		}
	}

	public void Dispose()
	{
		this.Deactivate();
		this.Bin();
	}

	public static TempList<T> New()
	{
		TempList<T> tempList;
		if (TempList<T>.Resurrect(out tempList))
		{
			return tempList;
		}
		return new TempList<T>();
	}

	public static TempList<T> New(IEnumerable<T> windows)
	{
		TempList<T> tempList;
		if (!TempList<T>.Resurrect(out tempList))
		{
			return new TempList<T>(windows);
		}
		tempList.AddRange(windows);
		return tempList;
	}

	private static bool Resurrect(out TempList<T> twl)
	{
		TempList<T> tempList;
		if (TempList<T>.dumpCount == 0)
		{
			twl = null;
			return false;
		}
		twl = TempList<T>.dump;
		int num = TempList<T>.dumpCount - 1;
		TempList<T>.dumpCount = num;
		if (num != 0)
		{
			tempList = twl.prev;
		}
		else
		{
			tempList = null;
		}
		TempList<T>.dump = tempList;
		twl.inDump = false;
		twl.prev = null;
		return true;
	}
}