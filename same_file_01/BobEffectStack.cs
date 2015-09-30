using Facepunch.Precision;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BobEffectStack : IDisposable
{
	private List<BobEffect.Data> data = new List<BobEffect.Data>();

	private List<BobEffectStack> forks = new List<BobEffectStack>();

	private BobEffectStack owner;

	private int dataCount;

	private bool isFork;

	private BobEffect.Context ctx;

	public BobEffectStack()
	{
	}

	public bool CreateInstance(BobEffect effect)
	{
		BobEffect.Data datum;
		if (!effect || !effect.Create(out datum))
		{
			return false;
		}
		this.data.Add(datum);
		foreach (BobEffectStack fork in this.forks)
		{
			fork.data.Add(datum.Clone());
		}
		return true;
	}

	private void DestroyAllEffects()
	{
		foreach (BobEffect.Data datum in this.data)
		{
			this.ctx.data = datum;
			if (!this.ctx.data.effect)
			{
				continue;
			}
			this.ctx.data.effect.Destroy(ref this.ctx.data);
		}
		this.ctx.data = null;
		this.data.Clear();
	}

	public void Dispose()
	{
		if (this.isFork)
		{
			this.DestroyAllEffects();
			this.owner.forks.Remove(this);
			this.owner = null;
			this.isFork = false;
		}
		else
		{
			foreach (BobEffectStack fork in this.forks)
			{
				fork.DestroyAllEffects();
			}
		}
	}

	public BobEffectStack Fork()
	{
		BobEffectStack bobEffectStack = new BobEffectStack()
		{
			isFork = true,
			owner = (!this.isFork ? this : this.owner)
		};
		bobEffectStack.owner.forks.Add(bobEffectStack);
		foreach (BobEffect.Data datum in bobEffectStack.owner.data)
		{
			bobEffectStack.data.Add(datum.Clone());
		}
		return bobEffectStack;
	}

	public bool IsForkOf(BobEffectStack stack)
	{
		return (this.owner == null ? false : this.owner == stack);
	}

	public void Join()
	{
		if (this.isFork)
		{
			this.dataCount = this.data.Count;
			for (int i = 0; i < this.dataCount; i++)
			{
				this.owner.data[i].CopyDataTo(this.data[i]);
			}
		}
	}

	private void RunSim(ref int i, ref Vector3G force, ref Vector3G torque)
	{
		while (i < this.dataCount)
		{
			this.ctx.data = this.data[i];
			switch (this.ctx.data.effect.Simulate(ref this.ctx))
			{
				case BOBRES.CONTINUE:
				{
					force.x = force.x + this.ctx.data.force.x;
					force.y = force.y + this.ctx.data.force.y;
					force.z = force.z + this.ctx.data.force.z;
					torque.x = torque.x + this.ctx.data.torque.x;
					torque.y = torque.y + this.ctx.data.torque.y;
					torque.z = torque.z + this.ctx.data.torque.z;
					break;
				}
				case BOBRES.EXIT:
				{
					if (!this.isFork)
					{
						int num = i;
						int num1 = num;
						i = num + 1;
						int num2 = num1;
						this.RunSim(ref i, ref force, ref torque);
						if (this.ctx.data == null)
						{
							this.data.RemoveAt(num2);
						}
						else
						{
							if (this.ctx.data.effect != null)
							{
								this.ctx.data.effect.Destroy(ref this.ctx.data);
							}
							this.data.RemoveAt(num2);
							foreach (BobEffectStack fork in this.forks)
							{
								fork.data.RemoveAt(num2);
							}
						}
					}
					return;
				}
				case BOBRES.ERROR:
				{
					Debug.LogError("Error with effect", this.ctx.data.effect);
					break;
				}
			}
			i = i + 1;
		}
	}

	public void Simulate(ref double dt, ref Vector3G force, ref Vector3G torque)
	{
		this.dataCount = this.data.Count;
		if (this.dataCount > 0)
		{
			int num = 0;
			this.ctx.dt = dt;
			this.RunSim(ref num, ref force, ref torque);
		}
	}
}