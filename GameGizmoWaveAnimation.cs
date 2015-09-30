using System;
using UnityEngine;

public class GameGizmoWaveAnimation : GameGizmo
{
	[SerializeField]
	protected float frequency = 1.2566371f;

	[SerializeField]
	protected float amplitudePositive = 0.15f;

	[SerializeField]
	protected float amplitudeNegative = -0.1f;

	[SerializeField]
	protected float phase;

	[SerializeField]
	protected Vector3 axis = Vector3.up;

	public GameGizmoWaveAnimation()
	{
	}

	protected override GameGizmo.Instance ConstructInstance()
	{
		return new GameGizmoWaveAnimation.Instance(this);
	}

	public class Instance : GameGizmo.Instance
	{
		public double Phase;

		public double Frequency;

		public double AmplitudePositive;

		public double AmplitudeNegative;

		public double @value;

		public double arcSine;

		public double sine;

		public Vector3 Axis;

		public bool Started;

		private ulong lastRenderTime;

		protected internal Instance(GameGizmoWaveAnimation gameGizmo) : base(gameGizmo)
		{
			this.Frequency = (double)gameGizmo.frequency;
			this.Phase = (double)gameGizmo.phase;
			this.AmplitudePositive = (double)gameGizmo.amplitudePositive;
			this.AmplitudeNegative = (double)gameGizmo.amplitudeNegative;
			this.Axis = gameGizmo.axis;
		}

		protected override void Render(bool useCamera, Camera camera)
		{
			double num;
			double num1;
			double amplitudeNegative;
			Vector3 axis = new Vector3();
			Matrix4x4 value;
			ulong num2 = NetCull.localTimeInMillis;
			if (!this.Started || this.lastRenderTime >= num2)
			{
				this.Started = true;
				double num3 = 0;
				num1 = num3;
				num = num3;
			}
			else
			{
				ulong num4 = num2 - this.lastRenderTime;
				num = (double)((float)num4) * 0.001;
				num1 = 1000 / (double)((float)num4);
				GameGizmoWaveAnimation.Instance phase = this;
				phase.Phase = phase.Phase + this.Frequency * num;
			}
			this.lastRenderTime = num2;
			if (this.Phase > 1)
			{
				if (!double.IsPositiveInfinity(this.Phase))
				{
					GameGizmoWaveAnimation.Instance instance = this;
					instance.Phase = instance.Phase % 1;
				}
				else
				{
					this.Phase = 0;
				}
			}
			else if (this.Phase == 1)
			{
				this.Phase = 0;
			}
			else if (this.Phase < 0)
			{
				if (!double.IsNegativeInfinity(this.Phase))
				{
					this.Phase = -this.Phase % 1;
					if (this.Phase > 0)
					{
						this.Phase = 1 - this.Phase;
					}
				}
				else
				{
					this.Phase = 0;
				}
			}
			if (this.Phase >= 0.5)
			{
				this.arcSine = (this.Phase * 2 - 2) * 3.14159265358979;
				amplitudeNegative = this.AmplitudeNegative;
			}
			else
			{
				this.arcSine = this.Phase * 6.28318530717959;
				amplitudeNegative = this.AmplitudePositive;
			}
			this.sine = Math.Sin(this.arcSine);
			this.@value = this.sine * amplitudeNegative;
			axis.x = (float)((double)this.Axis.x * this.@value);
			axis.y = (float)((double)this.Axis.y * this.@value);
			axis.z = (float)((double)this.Axis.z * this.@value);
			Matrix4x4? nullable = this.ultimateMatrix;
			if (!nullable.HasValue)
			{
				Matrix4x4? nullable1 = this.overrideMatrix;
				value = (!nullable1.HasValue ? base.DefaultMatrix() : nullable1.Value);
			}
			else
			{
				value = nullable.Value;
			}
			this.ultimateMatrix = new Matrix4x4?(value * Matrix4x4.TRS(axis, Quaternion.identity, Vector3.one));
			base.Render(useCamera, camera);
			this.ultimateMatrix = nullable;
		}
	}
}