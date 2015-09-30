using System;
using UnityEngine;

public class Crouchable : IDLocalCharacter
{
	private const double kSmoothInterval = 0.00322397606520169;

	private const double kSmoothDamp = 0.5;

	private const double kSmoothDampInput = 0;

	private const double kSmoothStiffness = 5;

	[NonSerialized]
	private CharacterCrouchTrait _crouchTrait;

	[NonSerialized]
	private bool didCrouchTraitTest;

	[NonSerialized]
	private float crouchUnits;

	[NonSerialized]
	private float crouchTime;

	public new Crouchable crouchable
	{
		get
		{
			return this;
		}
	}

	protected AnimationCurve crouchCurve
	{
		get
		{
			return this.crouchTrait.crouchCurve;
		}
	}

	public new bool crouched
	{
		get
		{
			return this.crouchUnits < 0f;
		}
	}

	protected float crouchToSpeedFraction
	{
		get
		{
			return this.crouchTrait.crouchToSpeedFraction;
		}
	}

	protected CharacterCrouchTrait crouchTrait
	{
		get
		{
			if (!this.didCrouchTraitTest)
			{
				this._crouchTrait = base.GetTrait<CharacterCrouchTrait>();
				this.didCrouchTraitTest = true;
			}
			return this._crouchTrait;
		}
	}

	public Crouchable()
	{
	}

	protected internal void ApplyCrouch(ref Vector3 localPosition)
	{
		localPosition.y = localPosition.y + this.crouchUnits;
	}

	public void ApplyCrouchOffset(ref CCTotem.PositionPlacement placement)
	{
		float single = placement.bottom.y + base.initialEyesOffsetY;
		float single1 = placement.originalTop.y - single;
		float single2 = placement.top.y - single1;
		float single3 = single2 - single;
		this.crouchUnits = (!Mathf.Approximately(single3, 0f) ? single3 : 0f);
		base.idMain.InvalidateEyesOffset();
	}

	public void LocalPlayerUpdateCrouchState(ref Crouchable.CrouchState incoming, ref bool crouchFlag, ref bool crouchBlockFlag, ref Crouchable.Smoothing smoothing)
	{
		double num = (double)base.initialEyesOffsetY;
		double bottomY = (double)incoming.BottomY + num;
		double bottomY1 = (double)(incoming.BottomY + incoming.InitialStandingHeight);
		double topY = (double)incoming.TopY - (bottomY1 - bottomY);
		double num1 = topY - (double)incoming.BottomY;
		this.crouchUnits = smoothing.CatchUp(num1 - num);
		base.idMain.InvalidateEyesOffset();
		if (!incoming.CrouchBlocked)
		{
			crouchBlockFlag = false;
		}
		else
		{
			crouchBlockFlag = true;
			crouchFlag = true;
		}
	}

	public void LocalPlayerUpdateCrouchState(CCMotor ccmotor, ref bool crouchFlag, ref bool crouchBlockFlag, ref Crouchable.Smoothing smoothing)
	{
		Crouchable.CrouchState crouchState = new Crouchable.CrouchState();
		crouchState.CrouchBlocked = ccmotor.isCrouchBlocked;
		CCTotem.PositionPlacement? lastPositionPlacement = ccmotor.LastPositionPlacement;
		CCTotem.PositionPlacement positionPlacement = (!lastPositionPlacement.HasValue ? new CCTotem.PositionPlacement(base.origin, base.origin, base.origin, ccmotor.ccTotemPole.MaximumHeight) : lastPositionPlacement.Value);
		crouchState.BottomY = positionPlacement.bottom.y;
		crouchState.TopY = positionPlacement.top.y;
		crouchState.InitialStandingHeight = positionPlacement.originalHeight;
		this.LocalPlayerUpdateCrouchState(ref crouchState, ref crouchFlag, ref crouchBlockFlag, ref smoothing);
	}

	public struct CrouchState
	{
		public bool CrouchBlocked;

		public float BottomY;

		public float TopY;

		public float InitialStandingHeight;
	}

	public struct Smoothing
	{
		private bool I;

		private double T;

		private double A;

		private double V;

		private double Y;

		private double Z;

		public void AddSeconds(double elapsedSeconds)
		{
			if (elapsedSeconds > 0)
			{
				Crouchable.Smoothing z = this;
				z.Z = z.Z + elapsedSeconds;
				Crouchable.Smoothing y = this;
				y.Y = y.Y + elapsedSeconds;
			}
		}

		public float CatchUp(double target)
		{
			double num;
			double num1;
			double num2;
			if (this.I)
			{
				if (this.Z > 0)
				{
					Crouchable.Smoothing v = this;
					v.V = v.V + (target - this.T) / this.Z * 0;
					this.Z = 0;
				}
				double num3 = 0.00322397606520169;
				double y = this.Y;
				if (y >= num3)
				{
					double num4 = target;
					num1 = num4;
					this.T = num4;
					double a = this.A - num1;
					double v1 = this.V;
					double num5 = 0.5;
					double num6 = 5;
					do
					{
						double num7 = a;
						a = a + v1 * num3;
						double num8 = -a * num6 - num5 * v1;
						a = a + num8 * num3;
						v1 = (a - num7) / num3;
						num2 = y - num3;
						y = num2;
					}
					while (num2 >= num3);
					this.A = target + a;
					this.V = v1;
					this.Y = y;
				}
				num = (y >= 1.40129846432482E-45 ? this.A + this.V * y : this.A);
			}
			else
			{
				double num9 = 0;
				num1 = num9;
				this.Z = num9;
				double num10 = num1;
				num1 = num10;
				this.V = num10;
				this.Y = num1;
				this.I = true;
				double num11 = target;
				num1 = num11;
				this.A = num11;
				double num12 = num1;
				num1 = num12;
				this.T = num12;
				num = num1;
			}
			return (num >= 1.40129846432482E-45 || num <= -1.40129846432482E-45 ? (float)num : 0f);
		}

		public void Reset()
		{
			this = new Crouchable.Smoothing();
		}

		public void Solve()
		{
			this.A = this.T;
			double num = 0;
			double num1 = num;
			this.Y = num;
			double num2 = num1;
			num1 = num2;
			this.Z = num2;
			this.V = num1;
			this.I = true;
		}
	}
}