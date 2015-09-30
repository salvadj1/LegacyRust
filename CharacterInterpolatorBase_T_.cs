using System;
using UnityEngine;

public abstract class CharacterInterpolatorBase<T> : CharacterInterpolatorBase
{
	private const IDLocalCharacterAddon.AddonFlags kRequiredAddonFlags = IDLocalCharacterAddon.AddonFlags.FireOnAddonAwake | IDLocalCharacterAddon.AddonFlags.FireOnAddonPostAwake;

	[NonSerialized]
	protected TimeStamped<T>[] tbuffer;

	private readonly bool customPusher;

	protected sealed override double __newestTimeStamp
	{
		get
		{
			return this.newestTimeStamp;
		}
	}

	protected sealed override double __oldestTimeStamp
	{
		get
		{
			return this.oldestTimeStamp;
		}
	}

	protected sealed override double __storedDuration
	{
		get
		{
			return this.storedDuration;
		}
	}

	public int bufferCapacity
	{
		get
		{
			return (int)this.tbuffer.Length;
		}
	}

	public new CharacterInterpolatorBase interpolator
	{
		get
		{
			return this;
		}
	}

	public new double newestTimeStamp
	{
		get
		{
			return (this.len != 0 ? this.tbuffer[this.tbuffer[this.len - 1].index].timeStamp : Double.PositiveInfinity);
		}
	}

	public new double oldestTimeStamp
	{
		get
		{
			return (this.len != 0 ? this.tbuffer[this.tbuffer[0].index].timeStamp : Double.NegativeInfinity);
		}
	}

	public new double storedDuration
	{
		get
		{
			return (this.len >= 2 ? this.tbuffer[this.tbuffer[0].index].timeStamp - this.tbuffer[this.tbuffer[this.len - 1].index].timeStamp : 0);
		}
	}

	protected CharacterInterpolatorBase() : this(false, IDLocalCharacterAddon.AddonFlags.FireOnAddonAwake | IDLocalCharacterAddon.AddonFlags.FireOnAddonPostAwake)
	{
	}

	protected CharacterInterpolatorBase(bool customPusher) : this(customPusher, IDLocalCharacterAddon.AddonFlags.FireOnAddonAwake | IDLocalCharacterAddon.AddonFlags.FireOnAddonPostAwake)
	{
	}

	protected CharacterInterpolatorBase(IDLocalCharacterAddon.AddonFlags addonFlags) : this(false, addonFlags)
	{
	}

	protected CharacterInterpolatorBase(bool customPusher, IDLocalCharacterAddon.AddonFlags addonFlags) : base((IDLocalCharacterAddon.AddonFlags)((byte)(addonFlags | IDLocalCharacterAddon.AddonFlags.FireOnAddonAwake | IDLocalCharacterAddon.AddonFlags.FireOnAddonPostAwake)))
	{
		this.customPusher = customPusher;
	}

	protected sealed override void __Clear()
	{
		this.Clear();
	}

	public new void Clear()
	{
		if (this.len > 0)
		{
			if (ReferenceTypeHelper<T>.TreatAsReferenceHolder)
			{
				for (int i = 0; i < this.len; i++)
				{
					T t = default(T);
					this.tbuffer[this.tbuffer[i].index].@value = t;
				}
			}
			this.len = 0;
		}
	}

	protected virtual bool CustomPusher(ref T state, ref double timeStamp)
	{
		throw new NotImplementedException();
	}

	protected override void OnAddonPostAwake()
	{
		this.tbuffer = new TimeStamped<T>[(this._bufferCapacity > 0 ? this._bufferCapacity : 32)];
		this._bufferCapacity = (int)this.tbuffer.Length;
		for (int i = 0; i < this._bufferCapacity; i++)
		{
			this.tbuffer[i].index = i;
		}
	}

	protected void Push(ref T state, ref double timeStamp)
	{
		int num;
		int length = (int)this.tbuffer.Length;
		if (this.len >= length)
		{
			for (int i = 0; i < length; i++)
			{
				int num1 = this.tbuffer[i].index;
				if (this.tbuffer[num1].timeStamp < timeStamp)
				{
					int num2 = this.tbuffer[length - 1].index;
					for (int j = length - 1; j > i; j--)
					{
						this.tbuffer[j].index = this.tbuffer[j - 1].index;
					}
					this.tbuffer[i].index = num2;
					this.tbuffer[num2].Set(ref state, ref timeStamp);
					return;
				}
				if (this.tbuffer[num1].timeStamp == timeStamp)
				{
					this.tbuffer[num1].Set(ref state, ref timeStamp);
					return;
				}
			}
		}
		else
		{
			for (int k = 0; k < this.len; k++)
			{
				int num3 = this.tbuffer[k].index;
				if (this.tbuffer[num3].timeStamp < timeStamp)
				{
					for (int l = this.len; l > k; l--)
					{
						this.tbuffer[l].index = this.tbuffer[l - 1].index;
					}
					this.tbuffer[k].index = this.len;
					TimeStamped<T>[] timeStampedArray = this.tbuffer;
					CharacterInterpolatorBase<T> characterInterpolatorBase = this;
					int num4 = characterInterpolatorBase.len;
					num = num4;
					characterInterpolatorBase.len = num4 + 1;
					timeStampedArray[num].Set(ref state, ref timeStamp);
					return;
				}
				if (this.tbuffer[num3].timeStamp == timeStamp)
				{
					this.tbuffer[num3].Set(ref state, ref timeStamp);
					return;
				}
			}
			this.tbuffer[this.len].index = this.len;
			TimeStamped<T>[] timeStampedArray1 = this.tbuffer;
			CharacterInterpolatorBase<T> characterInterpolatorBase1 = this;
			int num5 = characterInterpolatorBase1.len;
			num = num5;
			characterInterpolatorBase1.len = num5 + 1;
			timeStampedArray1[num].Set(ref state, ref timeStamp);
		}
	}

	public override void SetGoals(Vector3 pos, Quaternion rot, double timestamp)
	{
		throw new NotImplementedException(string.Concat("The thing using this has not implemented a way to take pos, rot to ", typeof(T)));
	}

	public void SetGoals(ref T state, ref double timeStamp)
	{
		if (!this.customPusher)
		{
			this.Push(ref state, ref timeStamp);
		}
		else
		{
			double num = timeStamp;
			T t = state;
			if (this.CustomPusher(ref t, ref num))
			{
				this.Push(ref t, ref num);
			}
		}
	}

	public void SetGoals(ref TimeStamped<T> state)
	{
		double num = state.timeStamp;
		T t = state.@value;
		this.SetGoals(ref t, ref num);
	}
}