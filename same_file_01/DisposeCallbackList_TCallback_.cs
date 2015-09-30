using System;
using UnityEngine;

public struct DisposeCallbackList<TCallback> : IDisposable
where TCallback : class
{
	private DisposeCallbackList<UnityEngine.Object, TCallback> def;

	public static DisposeCallbackList<TCallback> invalid
	{
		get
		{
			return new DisposeCallbackList<TCallback>();
		}
	}

	public DisposeCallbackList(DisposeCallbackList<UnityEngine.Object, TCallback>.Function invoke)
	{
		this.def = new DisposeCallbackList<UnityEngine.Object, TCallback>(null, invoke);
	}

	public bool Add(TCallback callback)
	{
		return this.def.Add(callback);
	}

	public void Dispose()
	{
		this.def.Dispose();
	}

	public bool Remove(TCallback callback)
	{
		return this.def.Remove(callback);
	}
}