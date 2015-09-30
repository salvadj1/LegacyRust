using System;
using UnityEngine;

public class RecoilSimulation : IDLocalCharacter
{
	[NonSerialized]
	private GrabBag<RecoilSimulation.Recoil> recoilImpulses;

	public RecoilSimulation()
	{
	}

	public void AddRecoil(float duration, float pitch, float yaw)
	{
		Angle2 angle2 = new Angle2()
		{
			pitch = pitch,
			yaw = yaw
		};
		this.AddRecoil(duration, ref angle2);
	}

	public void AddRecoil(float duration, float pitch)
	{
		Angle2 angle2 = new Angle2()
		{
			pitch = pitch
		};
		this.AddRecoil(duration, ref angle2);
	}

	public void AddRecoil(float duration, Angle2 angle)
	{
		this.AddRecoil(duration, ref angle);
	}

	public void AddRecoil(float duration, ref Angle2 angle2)
	{
		if (duration > 0f && (angle2.pitch != 0f || angle2.yaw != 0f))
		{
			if (this.recoilImpulses == null)
			{
				this.recoilImpulses = new GrabBag<RecoilSimulation.Recoil>(4);
				Debug.Log("Created GrabBag<Recoil>", this);
			}
			if (this.recoilImpulses.Add(new RecoilSimulation.Recoil(ref angle2, duration)) == 0)
			{
				base.enabled = true;
			}
		}
	}

	private bool ExtractRecoil(out Angle2 offset)
	{
		offset = new Angle2();
		if (this.recoilImpulses != null)
		{
			int count = this.recoilImpulses.Count;
			if (count > 0)
			{
				float single = Time.deltaTime;
				RecoilSimulation.Recoil[] buffer = this.recoilImpulses.Buffer;
				for (int i = count - 1; i >= 0; i--)
				{
					if (buffer[i].Extract(ref offset, single))
					{
						this.recoilImpulses.RemoveAt(i);
						while (true)
						{
							int num = i - 1;
							i = num;
							if (num < 0)
							{
								break;
							}
							if (buffer[i].Extract(ref offset, single))
							{
								this.recoilImpulses.RemoveAt(i);
							}
						}
						if (this.recoilImpulses.Count == 0)
						{
							base.enabled = false;
						}
					}
				}
				return (offset.pitch != 0f ? true : offset.yaw != 0f);
			}
		}
		return false;
	}

	private void LateUpdate()
	{
		Angle2 angle2;
		if (this.ExtractRecoil(out angle2))
		{
			base.ApplyAdditiveEyeAngles(angle2);
		}
	}

	private struct Recoil
	{
		public Angle2 angle;

		public float fraction;

		public float timeScale;

		public Recoil(ref Angle2 angle, float duration)
		{
			this.angle = angle;
			this.timeScale = 1f / duration;
			this.fraction = 0f;
		}

		public bool Extract(ref Angle2 sum, float deltaTime)
		{
			float single = this.fraction + (this.fraction - this.fraction * this.fraction);
			RecoilSimulation.Recoil recoil = this;
			recoil.fraction = recoil.fraction + deltaTime * this.timeScale;
			if (this.fraction >= 1f)
			{
				single = 1f - single;
				sum.pitch = sum.pitch + this.angle.pitch * single;
				sum.yaw = sum.yaw + this.angle.yaw * single;
				return true;
			}
			single = this.fraction + (this.fraction - this.fraction * this.fraction) - single;
			sum.pitch = sum.pitch + this.angle.pitch * single;
			sum.yaw = sum.yaw + this.angle.yaw * single;
			return false;
		}
	}
}