using System;
using UnityEngine;

public struct CacheRef<T>
where T : UnityEngine.Object
{
	[NonSerialized]
	public T @value;

	[NonSerialized]
	public readonly bool cached;

	[NonSerialized]
	private bool existed;

	public bool alive
	{
		get
		{
			bool flag;
			if (!this.existed)
			{
				flag = false;
			}
			else
			{
				bool flag1 = this.@value;
				bool flag2 = flag1;
				this.existed = flag1;
				flag = flag2;
			}
			return flag;
		}
	}

	private CacheRef(T value)
	{
		this.@value = value;
		this.existed = value;
		this.cached = true;
	}

	public bool Get(out T value)
	{
		bool flag;
		value = this.@value;
		if (!this.cached || !this.existed)
		{
			flag = false;
		}
		else
		{
			bool flag1 = value;
			bool flag2 = flag1;
			this.existed = flag1;
			flag = flag2;
		}
		return flag;
	}

	public static implicit operator CacheRef<T>(T value)
	{
		return new CacheRef<T>(value);
	}
}