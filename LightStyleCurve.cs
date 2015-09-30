using System;
using UnityEngine;

public class LightStyleCurve : LightStyle
{
	[SerializeField]
	private AnimationCurve curve;

	public LightStyleCurve()
	{
	}

	protected override LightStyle.Simulation ConstructSimulation(LightStylist stylist)
	{
		return new LightStyleCurve.Simulation(this);
	}

	protected override bool DeconstructSimulation(LightStyle.Simulation simulation)
	{
		return true;
	}

	private float GetCurveValue(double relativeStartTime)
	{
		return this.curve.Evaluate((float)relativeStartTime);
	}

	protected class Simulation : LightStyle.Simulation<LightStyleCurve>
	{
		private float? lastValue;

		public Simulation(LightStyleCurve creator) : base(creator)
		{
			this.lastValue = null;
		}

		protected override void Simulate(double currentTime)
		{
			float curveValue = base.creator.GetCurveValue(currentTime - this.startTime);
			if (!this.lastValue.HasValue || this.lastValue.Value != curveValue)
			{
				this.lastValue = new float?(curveValue);
				for (LightStyle.Mod.Element i = LightStyle.Mod.Element.Red; i < (LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle); i = (LightStyle.Mod.Element)((int)i + (int)LightStyle.Mod.Element.Green))
				{
					this.mod[i] = this.lastValue;
				}
			}
		}
	}
}