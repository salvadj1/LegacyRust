using Facepunch.Precision;
using System;
using UnityEngine;

public abstract class BobEffect : ScriptableObject
{
	[NonSerialized]
	private bool loaded;

	protected BobEffect()
	{
	}

	protected abstract void CloseData(BobEffect.Data data);

	public bool Create(out BobEffect.Data data)
	{
		if (!this.loaded)
		{
			this.InitializeNonSerializedData();
			this.loaded = true;
		}
		return this.OpenData(out data);
	}

	public void Destroy(ref BobEffect.Data data)
	{
		if (this.loaded && data != null)
		{
			this.CloseData(data);
			data = null;
		}
	}

	protected abstract void InitializeNonSerializedData();

	protected abstract bool OpenData(out BobEffect.Data data);

	public BOBRES Simulate(ref BobEffect.Context ctx)
	{
		if (!this.loaded)
		{
			return BOBRES.ERROR;
		}
		return this.SimulateData(ref ctx);
	}

	protected abstract BOBRES SimulateData(ref BobEffect.Context ctx);

	public struct Context
	{
		public double dt;

		public BobEffect.Data data;
	}

	public class Data
	{
		public Vector3G force;

		public Vector3G torque;

		public BobEffect effect;

		public Data()
		{
		}

		public virtual BobEffect.Data Clone()
		{
			return (BobEffect.Data)this.MemberwiseClone();
		}

		public virtual void CopyDataTo(BobEffect.Data target)
		{
			target.force = this.force;
			target.torque = this.torque;
		}
	}
}