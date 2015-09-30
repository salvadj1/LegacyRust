using System;
using UnityEngine;

public sealed class TransformInterpolatorEarly : StateInterpolator<PosRot>, IStateInterpolatorWithLinearVelocity, IStateInterpolator<PosRot>, IStateInterpolatorSampler<PosRot>
{
	public Transform target;

	public bool exterpolate;

	public float allowDifference = 0.1f;

	public TransformInterpolatorEarly()
	{
	}

	// privatescope
	internal void IStateInterpolator[[PosRot, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]].SetGoals(ref TimeStamped<PosRot> sample)
	{
		base.SetGoals(ref sample);
	}

	public bool Sample(ref double time, out PosRot result)
	{
		int num;
		double num1;
		int num2;
		int num3 = this.len;
		if (num3 == 0)
		{
			result = new PosRot();
			return false;
		}
		if (num3 == 1)
		{
			num = this.tbuffer[0].index;
			num1 = this.tbuffer[num].timeStamp;
			result = this.tbuffer[num].@value;
			return true;
		}
		int num4 = 0;
		int num5 = -1;
		do
		{
			num = this.tbuffer[num4].index;
			num1 = this.tbuffer[num].timeStamp;
			if (num1 <= time)
			{
				if (num1 == time)
				{
					result = this.tbuffer[num].@value;
					return true;
				}
				if (num1 < time)
				{
					if (num5 != -1)
					{
						double num6 = this.tbuffer[num5].timeStamp;
						double num7 = (double)this.allowDifference + NetCull.sendInterval;
						double num8 = num6 - num1;
						if (num8 > num7)
						{
							double num9 = num7;
							num8 = num9;
							num1 = num6 - num9;
							if (num1 >= time)
							{
								result = this.tbuffer[num].@value;
								return true;
							}
						}
						double num10 = (time - num1) / num8;
						PosRot.Lerp(ref this.tbuffer[num].@value, ref this.tbuffer[num5].@value, num10, out result);
					}
					else if (!this.exterpolate || num4 >= this.len - 1)
					{
						result = this.tbuffer[num].@value;
					}
					else
					{
						num5 = num;
						num = this.tbuffer[num4 + 1].index;
						double num11 = (time - this.tbuffer[num].timeStamp) / (this.tbuffer[num5].timeStamp - this.tbuffer[num].timeStamp);
						PosRot.Lerp(ref this.tbuffer[num].@value, ref this.tbuffer[num5].@value, num11, out result);
					}
					return true;
				}
			}
			else
			{
				num5 = num;
			}
			num2 = num4 + 1;
			num4 = num2;
		}
		while (num2 < this.len);
		result = this.tbuffer[this.tbuffer[this.len - 1].index].@value;
		return true;
	}

	public bool SampleWorldVelocity(double time, out Vector3 worldLinearVelocity)
	{
		int num;
		int num1 = this.len;
		if (num1 == 0 || num1 == 1)
		{
			worldLinearVelocity = new Vector3();
			return false;
		}
		int num2 = 0;
		int num3 = -1;
		do
		{
			int num4 = this.tbuffer[num2].index;
			double num5 = this.tbuffer[num4].timeStamp;
			if (num5 <= time)
			{
				if (num3 == -1)
				{
					worldLinearVelocity = new Vector3();
					return false;
				}
				double num6 = this.tbuffer[num3].timeStamp;
				double num7 = (double)this.allowDifference + NetCull.sendInterval;
				double num8 = num6 - num5;
				if (num8 >= num7)
				{
					num8 = num7;
					num5 = num6 - num8;
					if (time <= num5)
					{
						worldLinearVelocity = new Vector3();
						return false;
					}
				}
				worldLinearVelocity = this.tbuffer[num3].@value.position - this.tbuffer[num4].@value.position;
				worldLinearVelocity.x = (float)((double)worldLinearVelocity.x / num8);
				worldLinearVelocity.y = (float)((double)worldLinearVelocity.y / num8);
				worldLinearVelocity.z = (float)((double)worldLinearVelocity.z / num8);
				return true;
			}
			num3 = num4;
			num = num2 + 1;
			num2 = num;
		}
		while (num < this.len);
		worldLinearVelocity = new Vector3();
		return false;
	}

	public bool SampleWorldVelocity(out Vector3 worldLinearVelocity)
	{
		return this.SampleWorldVelocity(Interpolation.time, out worldLinearVelocity);
	}

	public sealed override void SetGoals(Vector3 pos, Quaternion rot, double timestamp)
	{
		PosRot posRot = new PosRot();
		posRot.position = pos;
		posRot.rotation = rot;
		this.SetGoals(ref posRot, ref timestamp);
	}

	public void SetGoals(PosRot frame, double timestamp)
	{
		this.SetGoals(ref frame, ref timestamp);
	}

	protected override void Syncronize()
	{
	}

	public void Update()
	{
		PosRot posRot;
		if (!base.running)
		{
			return;
		}
		double num = Interpolation.time;
		if (this.Sample(ref num, out posRot))
		{
			this.target.position = posRot.position;
			this.target.rotation = posRot.rotation;
		}
	}
}