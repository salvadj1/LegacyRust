using Facepunch.Precision;
using System;
using UnityEngine;

[Serializable]
public class BobForceCurve
{
	public Vector3 positionScale = Vector3.one;

	public AnimationCurve forceX;

	public AnimationCurve forceY;

	public AnimationCurve forceZ;

	public Vector3 outputScale = Vector3.one;

	public AnimationCurve sourceMask;

	public AnimationCurve sourceScale;

	private float duration;

	private float offset;

	private BobForceCurve.CurveInfo infoX;

	private BobForceCurve.CurveInfo infoY;

	private BobForceCurve.CurveInfo infoZ;

	private bool once;

	private bool calc;

	private bool mask;

	private bool scale;

	private bool scaleFixed;

	public BobForceCurveTarget target;

	public BobForceCurveSource source;

	public BobForceCurve()
	{
	}

	public void Calculate(ref Vector3G v, ref double pow, ref double dt, ref Vector3G sum)
	{
		Vector3G vector3G = new Vector3G();
		float single;
		bool flag;
		if (!this.once)
		{
			this.Gasp();
		}
		if (!this.calc)
		{
			return;
		}
		float single1 = (!this.mask ? 1f : this.sourceMask.Evaluate((float)pow));
		bool flag1 = (single1 == 0f ? true : single1 == 0f);
		if (!this.scaleFixed)
		{
			single = (!this.scale ? 1f : this.sourceScale.Evaluate((float)pow));
		}
		else
		{
			single = 0f;
		}
		float single2 = single;
		if (this.scaleFixed)
		{
			flag = false;
		}
		else
		{
			flag = (single2 == 0f ? false : single2 != 0f);
		}
		bool flag2 = flag;
		if (!this.infoX.calc)
		{
			vector3G.x = 0;
		}
		else
		{
			if (flag2 && !this.infoX.constant)
			{
				v.x = v.x + pow * dt * (double)single2 * (double)this.positionScale.x;
				if (v.x > (double)this.infoX.duration)
				{
					v.x = v.x - (double)this.infoX.duration;
				}
				else if (v.x < (double)(-this.infoX.duration))
				{
					v.x = v.x + (double)this.infoX.duration;
				}
			}
			vector3G.x = (double)((!flag1 ? this.forceX.Evaluate((float)v.x) * this.outputScale.x : 0f));
		}
		if (!this.infoY.calc)
		{
			vector3G.y = 0;
		}
		else
		{
			if (flag2 && !this.infoY.constant)
			{
				v.y = v.y + pow * dt * (double)single2 * (double)this.positionScale.y;
				if (v.y > (double)this.infoY.duration)
				{
					v.y = v.y - (double)this.infoY.duration;
				}
				else if (v.y < (double)(-this.infoY.duration))
				{
					v.y = v.y + (double)this.infoY.duration;
				}
			}
			vector3G.y = (double)((!flag1 ? this.forceY.Evaluate((float)v.y) * this.outputScale.y : 0f));
		}
		if (!this.infoZ.calc)
		{
			vector3G.z = 0;
		}
		else
		{
			if (flag2 && !this.infoZ.constant)
			{
				v.z = v.z + pow * dt * (double)single2 * (double)this.positionScale.z;
				if (v.z > (double)this.infoZ.duration)
				{
					v.z = v.z - (double)this.infoZ.duration;
				}
				else if (v.z < (double)(-this.infoZ.duration))
				{
					v.z = v.z + (double)this.infoZ.duration;
				}
			}
			vector3G.z = (double)((!flag1 ? this.forceZ.Evaluate((float)v.z) * this.outputScale.z : 0f));
		}
		if (!flag1)
		{
			sum.x = sum.x + vector3G.x * (double)single1;
			sum.y = sum.y + vector3G.y * (double)single1;
			sum.z = sum.z + vector3G.z * (double)single1;
		}
	}

	private void Gasp()
	{
		bool flag;
		bool flag1;
		bool flag2;
		this.infoX = new BobForceCurve.CurveInfo(this.forceX);
		this.infoY = new BobForceCurve.CurveInfo(this.forceY);
		this.infoZ = new BobForceCurve.CurveInfo(this.forceZ);
		this.calc = (this.infoX.calc || this.infoY.calc ? true : this.infoZ.calc);
		int num = this.sourceMask.length;
		if (num != 1)
		{
			flag = (num != 0 ? true : false);
		}
		else if (this.sourceMask[0].@value == 1f)
		{
			flag = false;
		}
		else if (this.sourceMask[0].@value != 0f)
		{
			flag = true;
		}
		else
		{
			this.calc = false;
			flag = false;
		}
		num = this.sourceScale.length;
		if (num == 1)
		{
			flag1 = (this.sourceScale[0].@value != 1f ? true : false);
			flag2 = (this.sourceScale[0].@value != 0f ? false : true);
		}
		else if (num != 0)
		{
			flag1 = true;
			flag2 = false;
		}
		else
		{
			flag1 = false;
			flag2 = false;
		}
		this.mask = flag;
		this.scale = flag1;
		this.scaleFixed = flag2;
		this.once = true;
	}

	private struct CurveInfo
	{
		public float duration;

		public float offset;

		public bool calc;

		public bool constant;

		public CurveInfo(AnimationCurve curve)
		{
			int num = (curve != null ? curve.length : 0);
			if (num == 0)
			{
				this.calc = false;
				this.constant = true;
				this.duration = 0f;
				this.offset = 0f;
			}
			else if (num != 1)
			{
				Keyframe item = curve[0];
				Keyframe keyframe = curve[num - 1];
				this.calc = true;
				this.constant = false;
				this.duration = keyframe.time - item.time;
				this.offset = curve[0].time;
				BobForceCurve.CurveInfo curveInfo = this;
				curveInfo.duration = curveInfo.duration * 8f;
			}
			else
			{
				this.calc = curve[0].@value != 0f;
				this.constant = true;
				this.duration = 0f;
				this.offset = 0f;
			}
		}
	}
}