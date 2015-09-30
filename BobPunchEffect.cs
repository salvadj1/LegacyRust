using Facepunch.Precision;
using System;
using UnityEngine;

public class BobPunchEffect : BobEffect
{
	[SerializeField]
	private AnimationCurve _x;

	[SerializeField]
	private AnimationCurve _y;

	[SerializeField]
	private AnimationCurve _z;

	[SerializeField]
	private AnimationCurve _yaw;

	[SerializeField]
	private AnimationCurve _pitch;

	[SerializeField]
	private AnimationCurve _roll;

	private BobPunchEffect.CurveInfo x;

	private BobPunchEffect.CurveInfo y;

	private BobPunchEffect.CurveInfo z;

	private BobPunchEffect.CurveInfo yaw;

	private BobPunchEffect.CurveInfo pitch;

	private BobPunchEffect.CurveInfo roll;

	private BobPunchEffect.CurveInfo glob;

	public BobPunchEffect()
	{
	}

	protected override void CloseData(BobEffect.Data data)
	{
	}

	protected override void InitializeNonSerializedData()
	{
		bool flag;
		bool flag1;
		int num;
		this.x = new BobPunchEffect.CurveInfo(this._x);
		this.y = new BobPunchEffect.CurveInfo(this._y);
		this.z = new BobPunchEffect.CurveInfo(this._z);
		this.yaw = new BobPunchEffect.CurveInfo(this._yaw);
		this.pitch = new BobPunchEffect.CurveInfo(this._pitch);
		this.roll = new BobPunchEffect.CurveInfo(this._roll);
		flag = (this.x.valid || this.y.valid || this.z.valid || this.yaw.valid || this.pitch.valid ? true : this.roll.valid);
		this.glob.valid = flag;
		if (!this.glob.valid)
		{
			flag1 = false;
		}
		else
		{
			if ((!this.x.valid || this.x.constant) && (!this.y.valid || this.y.constant) && (!this.z.valid || this.z.constant) && (!this.yaw.valid || this.yaw.constant) && (!this.pitch.valid || this.pitch.constant))
			{
				num = (!this.roll.valid ? 0 : (int)(!this.roll.constant));
			}
			else
			{
				num = 1;
			}
			flag1 = num == 0;
		}
		this.glob.constant = flag1;
		if (!this.glob.constant)
		{
			this.glob.startTime = Single.PositiveInfinity;
			this.glob.endTime = Single.NegativeInfinity;
			if (this.x.valid && !this.x.constant)
			{
				if (this.x.startTime < this.glob.startTime)
				{
					this.glob.startTime = this.x.startTime;
				}
				if (this.x.endTime > this.glob.endTime)
				{
					this.glob.endTime = this.x.endTime;
				}
			}
			if (this.z.valid && !this.z.constant)
			{
				if (this.z.startTime < this.glob.startTime)
				{
					this.glob.startTime = this.z.startTime;
				}
				if (this.z.endTime > this.glob.endTime)
				{
					this.glob.endTime = this.z.endTime;
				}
			}
			if (this.yaw.valid && !this.yaw.constant)
			{
				if (this.yaw.startTime < this.glob.startTime)
				{
					this.glob.startTime = this.yaw.startTime;
				}
				if (this.yaw.endTime > this.glob.endTime)
				{
					this.glob.endTime = this.yaw.endTime;
				}
			}
			if (this.pitch.valid && !this.pitch.constant)
			{
				if (this.pitch.startTime < this.glob.startTime)
				{
					this.glob.startTime = this.pitch.startTime;
				}
				if (this.pitch.endTime > this.glob.endTime)
				{
					this.glob.endTime = this.pitch.endTime;
				}
			}
			if (this.roll.valid && !this.roll.constant)
			{
				if (this.roll.startTime < this.glob.startTime)
				{
					this.glob.startTime = this.roll.startTime;
				}
				if (this.roll.endTime > this.glob.endTime)
				{
					this.glob.endTime = this.roll.endTime;
				}
			}
			if (this.roll.valid && !this.roll.constant)
			{
				if (this.roll.startTime < this.glob.startTime)
				{
					this.glob.startTime = this.roll.startTime;
				}
				if (this.roll.endTime > this.glob.endTime)
				{
					this.glob.endTime = this.roll.endTime;
				}
			}
			if (this.glob.startTime != Single.PositiveInfinity)
			{
				this.glob.duration = this.glob.endTime - this.glob.startTime;
			}
			else
			{
				this.glob.startTime = 0f;
				this.glob.endTime = 0f;
				this.glob.duration = 0f;
				this.glob.valid = false;
			}
		}
		else
		{
			this.glob.valid = false;
			this.glob.startTime = 0f;
			this.glob.endTime = 0f;
			this.glob.duration = 0f;
		}
	}

	protected override bool OpenData(out BobEffect.Data data)
	{
		if (!this.glob.valid)
		{
			data = null;
			return false;
		}
		data = new BobPunchEffect.PunchData()
		{
			effect = this
		};
		return true;
	}

	protected override BOBRES SimulateData(ref BobEffect.Context ctx)
	{
		if (ctx.dt == 0)
		{
			return BOBRES.CONTINUE;
		}
		BobPunchEffect.PunchData punchDatum = (BobPunchEffect.PunchData)ctx.data;
		if (punchDatum.time >= this.glob.endTime)
		{
			return BOBRES.EXIT;
		}
		if (punchDatum.time >= this.glob.endTime)
		{
			return BOBRES.EXIT;
		}
		if (this.x.valid)
		{
			if (this.x.constant || punchDatum.time <= this.x.startTime)
			{
				punchDatum.force.x = (double)this.x.startValue;
			}
			else if (punchDatum.time < this.x.endValue)
			{
				punchDatum.force.x = (double)this.x.curve.Evaluate(punchDatum.time);
			}
			else
			{
				punchDatum.force.x = (double)this.x.endValue;
			}
		}
		if (this.y.valid)
		{
			if (this.y.constant || punchDatum.time <= this.y.startTime)
			{
				punchDatum.force.y = (double)this.y.startValue;
			}
			else if (punchDatum.time < this.y.endValue)
			{
				punchDatum.force.y = (double)this.y.curve.Evaluate(punchDatum.time);
			}
			else
			{
				punchDatum.force.y = (double)this.y.endValue;
			}
		}
		if (this.z.valid)
		{
			if (this.z.constant || punchDatum.time <= this.z.startTime)
			{
				punchDatum.force.z = (double)this.z.startValue;
			}
			else if (punchDatum.time < this.z.endValue)
			{
				punchDatum.force.z = (double)this.z.curve.Evaluate(punchDatum.time);
			}
			else
			{
				punchDatum.force.z = (double)this.z.endValue;
			}
		}
		if (this.pitch.valid)
		{
			if (this.pitch.constant || punchDatum.time <= this.pitch.startTime)
			{
				punchDatum.torque.x = (double)this.pitch.startValue;
			}
			else if (punchDatum.time < this.pitch.endValue)
			{
				punchDatum.torque.x = (double)this.pitch.curve.Evaluate(punchDatum.time);
			}
			else
			{
				punchDatum.torque.x = (double)this.pitch.endValue;
			}
		}
		if (this.yaw.valid)
		{
			if (this.yaw.constant || punchDatum.time <= this.yaw.startTime)
			{
				punchDatum.torque.y = (double)this.yaw.startValue;
			}
			else if (punchDatum.time < this.yaw.endValue)
			{
				punchDatum.torque.y = (double)this.yaw.curve.Evaluate(punchDatum.time);
			}
			else
			{
				punchDatum.torque.y = (double)this.yaw.endValue;
			}
		}
		if (this.roll.valid)
		{
			if (this.roll.constant || punchDatum.time <= this.roll.startTime)
			{
				punchDatum.torque.z = (double)this.roll.startValue;
			}
			else if (punchDatum.time < this.roll.endValue)
			{
				punchDatum.torque.z = (double)this.roll.curve.Evaluate(punchDatum.time);
			}
			else
			{
				punchDatum.torque.z = (double)this.roll.endValue;
			}
		}
		BobPunchEffect.PunchData punchDatum1 = punchDatum;
		punchDatum1.time = punchDatum1.time + (float)ctx.dt;
		return BOBRES.CONTINUE;
	}

	private struct CurveInfo
	{
		public AnimationCurve curve;

		public float endTime;

		public float startTime;

		public float startValue;

		public float endValue;

		public float duration;

		public float min;

		public float max;

		public float range;

		public int length;

		public bool valid;

		public bool constant;

		public CurveInfo(AnimationCurve curve)
		{
			if (curve != null)
			{
				this.curve = curve;
			}
			else
			{
				this = new BobPunchEffect.CurveInfo();
			}
			this.length = curve.length;
			switch (this.length)
			{
				case 0:
				{
					this.endTime = 0f;
					this.startTime = 0f;
					this.duration = 0f;
					this.min = 0f;
					this.max = 0f;
					this.range = 0f;
					this.startValue = 0f;
					this.endValue = 0f;
					this.valid = false;
					this.constant = false;
					break;
				}
				case 1:
				{
					this.startTime = curve[0].time;
					this.endTime = this.startTime;
					this.duration = 0f;
					this.min = curve[0].@value;
					this.max = this.min;
					this.startValue = this.min;
					this.endValue = this.min;
					this.range = 0f;
					this.valid = true;
					this.constant = true;
					break;
				}
				case 2:
				{
					this.startTime = curve[0].time;
					this.endTime = curve[1].time;
					this.duration = this.endTime - this.startTime;
					this.startValue = curve[0].@value;
					this.endValue = curve[1].@value;
					if (this.endValue >= this.startValue)
					{
						this.range = this.endValue - this.startValue;
						this.min = this.startValue;
						this.max = this.endValue;
					}
					else
					{
						this.range = this.startValue - this.endValue;
						this.min = this.endValue;
						this.max = this.startValue;
					}
					this.valid = true;
					this.constant = this.range == 0f;
					break;
				}
				default:
				{
					this.startTime = curve[0].time;
					Keyframe item = curve[this.length - 1];
					this.endTime = item.time;
					this.duration = this.endTime - this.startTime;
					float single = curve[0].@value;
					float single1 = single;
					this.startValue = single;
					this.min = single1;
					this.max = this.min;
					this.endValue = this.startValue;
					for (int i = 1; i < this.length; i++)
					{
						this.endValue = curve[i].@value;
						if (this.endValue > this.max)
						{
							this.max = this.endValue;
						}
						if (this.endValue < this.min)
						{
							this.min = this.endValue;
						}
					}
					this.range = this.max - this.min;
					this.valid = true;
					this.constant = this.range == 0f;
					break;
				}
			}
		}

		public override string ToString()
		{
			return string.Format("[CurveInfo startTime={0}, duration={1}, min={2}, max={3}, length={4}, valid={5}, constant={6}]", new object[] { this.startTime, this.duration, this.min, this.max, this.length, this.valid, this.constant });
		}
	}

	private class PunchData : BobEffect.Data
	{
		public float time;

		public PunchData()
		{
		}

		public override void CopyDataTo(BobEffect.Data data)
		{
			base.CopyDataTo(data);
			((BobPunchEffect.PunchData)data).time = this.time;
		}
	}
}