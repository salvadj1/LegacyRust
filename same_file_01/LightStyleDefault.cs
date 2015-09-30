using System;
using UnityEngine;

public class LightStyleDefault : LightStyle
{
	private static LightStyleDefault singleton;

	private LightStyleDefault.DefaultSimulation singletonSimulation;

	public static LightStyleDefault Singleton
	{
		get
		{
			if (LightStyleDefault.singleton)
			{
				return LightStyleDefault.singleton;
			}
			return ScriptableObject.CreateInstance<LightStyleDefault>();
		}
	}

	public LightStyleDefault()
	{
	}

	protected override LightStyle.Simulation ConstructSimulation(LightStylist stylist)
	{
		LightStyleDefault.DefaultSimulation defaultSimulation = this.singletonSimulation;
		if (defaultSimulation == null)
		{
			LightStyleDefault.DefaultSimulation defaultSimulation1 = new LightStyleDefault.DefaultSimulation(this);
			LightStyleDefault.DefaultSimulation defaultSimulation2 = defaultSimulation1;
			this.singletonSimulation = defaultSimulation1;
			defaultSimulation = defaultSimulation2;
		}
		return defaultSimulation;
	}

	protected override bool DeconstructSimulation(LightStyle.Simulation simulation)
	{
		return false;
	}

	private void OnDisable()
	{
		if (LightStyleDefault.singleton == this)
		{
			LightStyleDefault.singleton = null;
		}
	}

	private void OnEnable()
	{
		LightStyleDefault.singleton = this;
	}

	private class DefaultSimulation : LightStyle.Simulation
	{
		public DefaultSimulation(LightStyleDefault def) : base(def)
		{
			float? nullable = new float?(1f);
			for (LightStyle.Mod.Element i = LightStyle.Mod.Element.Red; i < (LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle); i = (LightStyle.Mod.Element)((int)i + (int)LightStyle.Mod.Element.Green))
			{
				this.mod[i] = nullable;
			}
		}

		protected override void Simulate(double currentTime)
		{
		}
	}
}