using System;
using System.Collections.Generic;
using UnityEngine;

public struct DisposeCallbackList<TOwner, TCallback> : IDisposable
where TOwner : UnityEngine.Object
where TCallback : class
{
	private readonly DisposeCallbackList<TOwner, TCallback>.Function function;

	private TOwner owner;

	private List<TCallback> list;

	private int destroyIndex;

	private int count;

	public static DisposeCallbackList<TOwner, TCallback> invalid
	{
		get
		{
			return new DisposeCallbackList<TOwner, TCallback>();
		}
	}

	public DisposeCallbackList(TOwner owner, DisposeCallbackList<TOwner, TCallback>.Function invoke)
	{
		if (invoke == null)
		{
			throw new ArgumentNullException("invoke");
		}
		this.function = invoke;
		this.list = null;
		this.destroyIndex = -1;
		this.count = 0;
		this.owner = owner;
	}

	public bool Add(TCallback value)
	{
		if (this.list != null)
		{
			int num = this.list.IndexOf(value);
			if (num != -1)
			{
				if (this.destroyIndex < num && this.count - 1 != num)
				{
					this.list.RemoveAt(num);
					this.list.Add(value);
				}
				return false;
			}
		}
		else
		{
			this.list = new List<TCallback>();
		}
		this.list.Add(value);
		int num1 = this.destroyIndex;
		DisposeCallbackList<TOwner, TCallback> disposeCallbackList = this;
		int num2 = disposeCallbackList.count;
		int num3 = num2;
		disposeCallbackList.count = num2 + 1;
		if (num1 == num3)
		{
			this.Invoke(value);
			DisposeCallbackList<TOwner, TCallback> disposeCallbackList1 = this;
			disposeCallbackList1.destroyIndex = disposeCallbackList1.destroyIndex + 1;
		}
		return true;
	}

	public void Dispose()
	{
		if (this.destroyIndex == -1)
		{
			while (true)
			{
				DisposeCallbackList<TOwner, TCallback> disposeCallbackList = this;
				int num = disposeCallbackList.destroyIndex + 1;
				int num1 = num;
				disposeCallbackList.destroyIndex = num;
				if (num1 >= this.count)
				{
					break;
				}
				this.Invoke(this.list[this.destroyIndex]);
			}
		}
	}

	private void Invoke(TCallback value)
	{
		try
		{
			this.function(this.owner, this.list[this.destroyIndex]);
		}
		catch (Exception exception1)
		{
			Exception exception = exception1;
			Debug.LogError(string.Format("There was a exception thrown while attempting to invoke '{0}' thru '{1}' via owner '{2}'. exception is below\r\n{3}", new object[] { value, this.function, this.owner, exception }), this.owner);
		}
	}

	public bool Remove(TCallback value)
	{
		if (this.destroyIndex != -1)
		{
			return false;
		}
		return (this.list == null ? false : this.list.Remove(value));
	}

	public delegate void Function(TOwner owner, TCallback callback);
}