using System;
using UnityEngine;

public sealed class CharacterStateInterpolator : CharacterInterpolatorBase<CharacterStateInterpolatorData>, IStateInterpolator<CharacterStateInterpolatorData>, IStateInterpolatorWithLinearVelocity, IStateInterpolatorWithAngularVelocity, IStateInterpolatorWithVelocity, IStateInterpolatorSampler<CharacterStateInterpolatorData>, IStateInterpolatorSampler<CharacterTransformInterpolatorData>
{
	private bool once;

	public CharacterStateInterpolator()
	{
	}

	// privatescope
	internal void IStateInterpolator[[CharacterStateInterpolatorData, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]].SetGoals(ref CharacterStateInterpolatorData sample, ref double timeStamp)
	{
		base.SetGoals(ref sample, ref timeStamp);
	}

	// privatescope
	internal void IStateInterpolator[[CharacterStateInterpolatorData, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]].SetGoals(ref TimeStamped<CharacterStateInterpolatorData> sample)
	{
		base.SetGoals(ref sample);
	}

	public bool Sample(ref double time, out CharacterTransformInterpolatorData result)
	{
		result = new CharacterTransformInterpolatorData();
		CharacterStateInterpolatorData characterStateInterpolatorDatum;
		bool flag = this.Sample(ref time, out characterStateInterpolatorDatum);
		result.eyesAngles = characterStateInterpolatorDatum.eyesAngles;
		result.origin = characterStateInterpolatorDatum.origin;
		return flag;
	}

	public bool Sample(ref double time, out CharacterStateInterpolatorData result)
	{
		result = new CharacterStateInterpolatorData();
		int num;
		double num1;
		int num2;
		int num3 = this.len;
		if (num3 == 0)
		{
			result = new CharacterStateInterpolatorData();
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
						double num7 = (double)this.allowableTimeSpan + NetCull.sendInterval;
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
						if (num10 == 0)
						{
							result = this.tbuffer[num].@value;
						}
						else if (num10 != 1)
						{
							double num11 = 1 - num10;
							result.origin.x = (float)((double)this.tbuffer[num].@value.origin.x * num11 + (double)this.tbuffer[num5].@value.origin.x * num10);
							result.origin.y = (float)((double)this.tbuffer[num].@value.origin.y * num11 + (double)this.tbuffer[num5].@value.origin.y * num10);
							result.origin.z = (float)((double)this.tbuffer[num].@value.origin.z * num11 + (double)this.tbuffer[num5].@value.origin.z * num10);
							result.eyesAngles = new Angle2()
							{
								yaw = (float)((double)this.tbuffer[num].@value.eyesAngles.yaw + (double)Mathf.DeltaAngle(this.tbuffer[num].@value.eyesAngles.yaw, this.tbuffer[num5].@value.eyesAngles.yaw) * num10),
								pitch = Mathf.DeltaAngle(0f, (float)((double)this.tbuffer[num].@value.eyesAngles.pitch + (double)Mathf.DeltaAngle(this.tbuffer[num].@value.eyesAngles.pitch, this.tbuffer[num5].@value.eyesAngles.pitch) * num10))
							};
							if (num10 > 1)
							{
								result.state = this.tbuffer[num5].@value.state;
							}
							else if (num10 >= 0)
							{
								result.state = this.tbuffer[num].@value.state;
								result.state.flags = (ushort)(result.state.flags | (byte)(this.tbuffer[num5].@value.state.flags & 67));
								if (result.state.grounded != this.tbuffer[num5].@value.state.grounded)
								{
									result.state.grounded = false;
								}
							}
							else
							{
								result.state = this.tbuffer[num].@value.state;
							}
						}
						else
						{
							result = this.tbuffer[num5].@value;
						}
					}
					else if (!this.extrapolate || num4 >= this.len - 1)
					{
						result = this.tbuffer[num].@value;
					}
					else
					{
						num5 = num;
						num = this.tbuffer[num4 + 1].index;
						double num12 = (num1 - this.tbuffer[num].timeStamp) / (num1 - this.tbuffer[num].timeStamp);
						if (num12 == 0)
						{
							result = this.tbuffer[num].@value;
						}
						else if (num12 != 1)
						{
							double num13 = 1 - num12;
							result.origin.x = (float)((double)this.tbuffer[num].@value.origin.x * num13 + (double)this.tbuffer[num5].@value.origin.x * num12);
							result.origin.y = (float)((double)this.tbuffer[num].@value.origin.y * num13 + (double)this.tbuffer[num5].@value.origin.y * num12);
							result.origin.z = (float)((double)this.tbuffer[num].@value.origin.z * num13 + (double)this.tbuffer[num5].@value.origin.z * num12);
							result.eyesAngles = new Angle2()
							{
								yaw = (float)((double)this.tbuffer[num].@value.eyesAngles.yaw + (double)Mathf.DeltaAngle(this.tbuffer[num].@value.eyesAngles.yaw, this.tbuffer[num5].@value.eyesAngles.yaw) * num12),
								pitch = Mathf.DeltaAngle(0f, (float)((double)this.tbuffer[num].@value.eyesAngles.pitch + (double)Mathf.DeltaAngle(this.tbuffer[num].@value.eyesAngles.pitch, this.tbuffer[num5].@value.eyesAngles.pitch) * num12))
							};
							if (num12 > 1)
							{
								result.state = this.tbuffer[num5].@value.state;
							}
							else if (num12 >= 0)
							{
								result.state = this.tbuffer[num].@value.state;
								result.state.flags = (ushort)(result.state.flags | (byte)(this.tbuffer[num5].@value.state.flags & 67));
								if (result.state.grounded != this.tbuffer[num5].@value.state.grounded)
								{
									result.state.grounded = false;
								}
							}
							else
							{
								result.state = this.tbuffer[num].@value.state;
							}
						}
						else
						{
							result = this.tbuffer[num5].@value;
						}
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

	public bool SampleWorldVelocity(double time, out Vector3 worldLinearVelocity, out Angle2 worldAngularVelocity)
	{
		int num;
		int num1 = this.len;
		if (num1 == 0 || num1 == 1)
		{
			worldLinearVelocity = new Vector3();
			worldAngularVelocity = new Angle2();
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
					worldAngularVelocity = new Angle2();
					return false;
				}
				double num6 = this.tbuffer[num3].timeStamp;
				double num7 = (double)this.allowableTimeSpan + NetCull.sendInterval;
				double num8 = num6 - num5;
				if (num8 >= num7)
				{
					num8 = num7;
					num5 = num6 - num8;
					if (time <= num5)
					{
						worldLinearVelocity = new Vector3();
						worldAngularVelocity = new Angle2();
						return false;
					}
				}
				worldLinearVelocity = this.tbuffer[num3].@value.origin - this.tbuffer[num4].@value.origin;
				worldAngularVelocity = Angle2.Delta(this.tbuffer[num4].@value.eyesAngles, this.tbuffer[num3].@value.eyesAngles);
				worldLinearVelocity.x = (float)((double)worldLinearVelocity.x / num8);
				worldLinearVelocity.y = (float)((double)worldLinearVelocity.y / num8);
				worldLinearVelocity.z = (float)((double)worldLinearVelocity.z / num8);
				worldAngularVelocity.x = (float)((double)worldAngularVelocity.x / num8);
				worldAngularVelocity.y = (float)((double)worldAngularVelocity.y / num8);
				return true;
			}
			num3 = num4;
			num = num2 + 1;
			num2 = num;
		}
		while (num < this.len);
		worldLinearVelocity = new Vector3();
		worldAngularVelocity = new Angle2();
		return false;
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
				double num7 = (double)this.allowableTimeSpan + NetCull.sendInterval;
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
				worldLinearVelocity = this.tbuffer[num3].@value.origin - this.tbuffer[num4].@value.origin;
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

	public bool SampleWorldVelocity(double time, out Angle2 worldAngularVelocity)
	{
		int num;
		int num1 = this.len;
		if (num1 == 0 || num1 == 1)
		{
			worldAngularVelocity = new Angle2();
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
					worldAngularVelocity = new Angle2();
					return false;
				}
				double num6 = this.tbuffer[num3].timeStamp;
				double num7 = (double)this.allowableTimeSpan + NetCull.sendInterval;
				double num8 = num6 - num5;
				if (num8 >= num7)
				{
					num8 = num7;
					num5 = num6 - num8;
					if (time <= num5)
					{
						worldAngularVelocity = new Angle2();
						return false;
					}
				}
				worldAngularVelocity = Angle2.Delta(this.tbuffer[num4].@value.eyesAngles, this.tbuffer[num3].@value.eyesAngles);
				worldAngularVelocity.x = (float)((double)worldAngularVelocity.x / num8);
				worldAngularVelocity.y = (float)((double)worldAngularVelocity.y / num8);
				return true;
			}
			num3 = num4;
			num = num2 + 1;
			num2 = num;
		}
		while (num < this.len);
		worldAngularVelocity = new Angle2();
		return false;
	}

	public bool SampleWorldVelocity(out Angle2 worldAngularVelocity)
	{
		return this.SampleWorldVelocity(Interpolation.time, out worldAngularVelocity);
	}

	public bool SampleWorldVelocity(out Vector3 worldLinearVelocity, out Angle2 worldAngularVelocity)
	{
		return this.SampleWorldVelocity(Interpolation.time, out worldLinearVelocity, out worldAngularVelocity);
	}

	public sealed override void SetGoals(Vector3 pos, Quaternion rot, double timestamp)
	{
		this.SetGoals(pos, (Angle2)rot, base.idMain.stateFlags, timestamp);
	}

	public void SetGoals(Vector3 pos, Angle2 rot, CharacterStateFlags state, double timestamp)
	{
		CharacterStateInterpolatorData characterStateInterpolatorDatum = new CharacterStateInterpolatorData();
		characterStateInterpolatorDatum.origin = pos;
		characterStateInterpolatorDatum.eyesAngles = rot;
		characterStateInterpolatorDatum.state = state;
		base.SetGoals(ref characterStateInterpolatorDatum, ref timestamp);
	}

	protected override void Syncronize()
	{
		CharacterStateInterpolatorData characterStateInterpolatorDatum;
		double num = Interpolation.time;
		if (this.Sample(ref num, out characterStateInterpolatorDatum))
		{
			Character character = base.idMain;
			if (character)
			{
				character.origin = characterStateInterpolatorDatum.origin;
				character.eyesAngles = characterStateInterpolatorDatum.eyesAngles;
				CharacterStateFlags characterStateFlag = character.stateFlags;
				character.stateFlags = characterStateInterpolatorDatum.state;
				if (!characterStateFlag.Equals(characterStateInterpolatorDatum.state))
				{
					if (this.once)
					{
						character.Signal_State_FlagsChanged(false);
					}
					else
					{
						character.Signal_State_FlagsChanged(true);
						this.once = true;
					}
				}
			}
		}
	}
}