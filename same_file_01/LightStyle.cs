using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

public abstract class LightStyle : ScriptableObject
{
	private static Dictionary<string, WeakReference> loadedByString;

	private static bool madeLoadedByString;

	public static double time
	{
		get
		{
			if (NetCull.isRunning)
			{
				return NetCull.time;
			}
			return (double)Time.time;
		}
	}

	static LightStyle()
	{
	}

	protected LightStyle()
	{
	}

	protected abstract LightStyle.Simulation ConstructSimulation(LightStylist stylist);

	public LightStyle.Simulation CreateSimulation(LightStylist stylist)
	{
		return this.CreateSimulation(LightStyle.time, stylist);
	}

	public LightStyle.Simulation CreateSimulation(double startTime, LightStylist stylist)
	{
		LightStyle.Simulation simulation = this.ConstructSimulation(stylist);
		if (simulation != null)
		{
			simulation.ResetTime(startTime);
		}
		return simulation;
	}

	protected abstract bool DeconstructSimulation(LightStyle.Simulation simulation);

	private static LightStyle MissingLightStyle(string name)
	{
		return LightStyleDefault.Singleton;
	}

	public static implicit operator LightStyle(string name)
	{
		WeakReference weakReference;
		if (!LightStyle.madeLoadedByString)
		{
			LightStyle lightStyle = (LightStyle)UnityEngine.Resources.Load(name, typeof(LightStyle));
			if (!lightStyle)
			{
				lightStyle = LightStyle.MissingLightStyle(name);
			}
			else
			{
				LightStyle.loadedByString = new Dictionary<string, WeakReference>(StringComparer.InvariantCultureIgnoreCase);
				LightStyle.loadedByString[name] = new WeakReference(lightStyle);
			}
			return lightStyle;
		}
		if (!LightStyle.loadedByString.TryGetValue(name, out weakReference))
		{
			LightStyle lightStyle1 = (LightStyle)UnityEngine.Resources.Load(name, typeof(LightStyle));
			if (!lightStyle1)
			{
				lightStyle1 = LightStyle.MissingLightStyle(name);
			}
			else
			{
				weakReference = new WeakReference(lightStyle1);
				LightStyle.loadedByString[name] = weakReference;
			}
			return lightStyle1;
		}
		object target = weakReference.Target;
		if (weakReference.IsAlive && (UnityEngine.Object)target)
		{
			return (LightStyle)target;
		}
		LightStyle lightStyle2 = (LightStyle)UnityEngine.Resources.Load(name, typeof(LightStyle));
		if (!lightStyle2)
		{
			lightStyle2 = LightStyle.MissingLightStyle(name);
		}
		else
		{
			weakReference.Target = lightStyle2;
		}
		return lightStyle2;
	}

	public static implicit operator String(LightStyle lightStyle)
	{
		string str;
		if (!lightStyle)
		{
			str = null;
		}
		else
		{
			str = lightStyle.name;
		}
		return str;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct Mod
	{
		[FieldOffset(-1)]
		public const LightStyle.Mod.Element kElementFirst = LightStyle.Mod.Element.Red;

		[FieldOffset(-1)]
		public const LightStyle.Mod.Element kElementLast = LightStyle.Mod.Element.SpotAngle;

		[FieldOffset(-1)]
		public const LightStyle.Mod.Element kElementBegin = LightStyle.Mod.Element.Red;

		[FieldOffset(-1)]
		public const LightStyle.Mod.Element kElementEnd = LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle;

		[FieldOffset(-1)]
		public const LightStyle.Mod.Element kElementEnumeratorBegin = LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle;

		[FieldOffset(-1)]
		public const LightStyle.Mod.Mask kMaskNone = 0;

		[FieldOffset(-1)]
		public const LightStyle.Mod.Mask kMaskRGB = LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue;

		[FieldOffset(-1)]
		public const LightStyle.Mod.Mask kMaskRGBA = LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha;

		[FieldOffset(-1)]
		public const LightStyle.Mod.Mask kMaskDirectionalLight = LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha | LightStyle.Mod.Mask.Intensity;

		[FieldOffset(-1)]
		public const LightStyle.Mod.Mask kMaskPointLight = LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha | LightStyle.Mod.Mask.Intensity | LightStyle.Mod.Mask.Range;

		[FieldOffset(-1)]
		public const LightStyle.Mod.Mask kMaskSpotLight = LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha | LightStyle.Mod.Mask.Intensity | LightStyle.Mod.Mask.Range | LightStyle.Mod.Mask.SpotAngle;

		[FieldOffset(-1)]
		public const LightStyle.Mod.Mask kMaskAll = LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha | LightStyle.Mod.Mask.Intensity | LightStyle.Mod.Mask.Range | LightStyle.Mod.Mask.SpotAngle;

		[FieldOffset(0)]
		public Color color;

		[FieldOffset(0)]
		public float r;

		[FieldOffset(4)]
		public float g;

		[FieldOffset(8)]
		public float b;

		[FieldOffset(12)]
		public float a;

		[FieldOffset(16)]
		public float intensity;

		[FieldOffset(20)]
		public float range;

		[FieldOffset(24)]
		public float spotAngle;

		[FieldOffset(28)]
		public LightStyle.Mod.Mask mask;

		public float? this[LightStyle.Mod.Element element]
		{
			get
			{
				if (!this.Contains(element))
				{
					return null;
				}
				return new float?(this.GetFaceValue(element));
			}
			set
			{
				if (!value.HasValue)
				{
					this.ClearModify(element);
				}
				else
				{
					this.SetFaceValue(element, value.Value);
					this.SetModify(element);
				}
			}
		}

		public bool AllOf(LightStyle.Mod.Mask mask)
		{
			return (this.mask & mask) == mask;
		}

		public bool AnyOf(LightStyle.Mod.Mask mask)
		{
			return (int)(this.mask & mask) != 0;
		}

		public void ApplyTo(Light light)
		{
			switch (light.type)
			{
				case LightType.Spot:
				{
					this.ApplyTo(light, LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha | LightStyle.Mod.Mask.Intensity | LightStyle.Mod.Mask.Range | LightStyle.Mod.Mask.SpotAngle);
					break;
				}
				case LightType.Directional:
				{
					this.ApplyTo(light, LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha | LightStyle.Mod.Mask.Intensity);
					break;
				}
				case LightType.Point:
				{
					this.ApplyTo(light, LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha | LightStyle.Mod.Mask.Intensity | LightStyle.Mod.Mask.Range);
					break;
				}
			}
		}

		public void ApplyTo(Light light, LightStyle.Mod.Mask applyMask)
		{
			LightStyle.Mod.Mask mask = this.mask & applyMask;
			if ((int)(mask & (LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha)) != 0)
			{
				if ((mask & (LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha)) != (LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha))
				{
					Color color = light.color;
					if ((mask & LightStyle.Mod.Mask.Red) == LightStyle.Mod.Mask.Red)
					{
						color.r = this.r;
					}
					if ((mask & LightStyle.Mod.Mask.Green) == LightStyle.Mod.Mask.Green)
					{
						color.g = this.g;
					}
					if ((mask & LightStyle.Mod.Mask.Blue) == LightStyle.Mod.Mask.Blue)
					{
						color.b = this.b;
					}
					if ((mask & LightStyle.Mod.Mask.Alpha) == LightStyle.Mod.Mask.Alpha)
					{
						color.a = this.a;
					}
					light.color = color;
				}
				else
				{
					light.color = this.color;
				}
			}
			if ((mask & LightStyle.Mod.Mask.Intensity) == LightStyle.Mod.Mask.Intensity)
			{
				light.intensity = this.intensity;
			}
			if ((mask & LightStyle.Mod.Mask.Range) == LightStyle.Mod.Mask.Range)
			{
				light.range = this.range;
			}
			if ((mask & LightStyle.Mod.Mask.SpotAngle) == LightStyle.Mod.Mask.SpotAngle)
			{
				light.spotAngle = this.spotAngle;
			}
		}

		public void ClearModify(LightStyle.Mod.Element element)
		{
			LightStyle.Mod maskNot = this;
			maskNot.mask = maskNot.mask & LightStyle.Mod.ElementToMaskNot(element);
		}

		public bool Contains(LightStyle.Mod.Element element)
		{
			return this.AllOf(LightStyle.Mod.ElementToMask(element));
		}

		public static LightStyle.Mod.Mask ElementToMask(LightStyle.Mod.Element element)
		{
			return (LightStyle.Mod.Mask)((int)LightStyle.Mod.Element.Green << (int)(element & (LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle)) & (int)(LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle));
		}

		public static LightStyle.Mod.Mask ElementToMaskNot(LightStyle.Mod.Element element)
		{
			return (LightStyle.Mod.Mask)(~((int)LightStyle.Mod.Element.Green << (int)(element & (LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle))) & (LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle));
		}

		public float GetFaceValue(LightStyle.Mod.Element element)
		{
			switch (element)
			{
				case LightStyle.Mod.Element.Red:
				{
					return this.r;
				}
				case LightStyle.Mod.Element.Green:
				{
					return this.g;
				}
				case LightStyle.Mod.Element.Blue:
				{
					return this.b;
				}
				case LightStyle.Mod.Element.Alpha:
				{
					return this.a;
				}
				case LightStyle.Mod.Element.Intensity:
				{
					return this.intensity;
				}
				case LightStyle.Mod.Element.Range:
				{
					return this.range;
				}
				case LightStyle.Mod.Element.SpotAngle:
				{
					return this.spotAngle;
				}
			}
			throw new ArgumentOutOfRangeException("element");
		}

		public static LightStyle.Mod Lerp(LightStyle.Mod a, LightStyle.Mod b, float t, LightStyle.Mod.Mask mask)
		{
			b.mask = b.mask & mask;
			if ((int)b.mask == 0)
			{
				return a;
			}
			a.mask = a.mask & mask;
			if ((int)a.mask == 0)
			{
				return b;
			}
			LightStyle.Mod.Mask mask1 = a.mask & b.mask;
			if ((int)mask1 != 0)
			{
				float single = 1f - t;
				if ((int)mask != 0)
				{
					for (LightStyle.Mod.Element i = LightStyle.Mod.Element.Red; i < (LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle); i = (LightStyle.Mod.Element)((int)i + (int)LightStyle.Mod.Element.Green))
					{
						if ((mask1 & LightStyle.Mod.ElementToMask(i)) == LightStyle.Mod.ElementToMask(i))
						{
							float faceValue = a.GetFaceValue(i);
							float faceValue1 = b.GetFaceValue(i);
							float single1 = faceValue * single + faceValue1 * t;
							a.SetFaceValue(i, single1);
						}
					}
				}
			}
			if (mask1 != a.mask)
			{
				a = a | b;
			}
			return a;
		}

		public static LightStyle.Mod operator +(LightStyle.Mod a, LightStyle.Mod b)
		{
			LightStyle.Mod mod = a;
			LightStyle.Mod.Mask mask = a.mask & b.mask;
			for (LightStyle.Mod.Element i = LightStyle.Mod.Element.Red; i < (LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle); i = (LightStyle.Mod.Element)((int)i + (int)LightStyle.Mod.Element.Green))
			{
				if ((mask & LightStyle.Mod.ElementToMask(i)) == LightStyle.Mod.ElementToMask(i))
				{
					mod.SetFaceValue(i, a.GetFaceValue(i) + b.GetFaceValue(i));
				}
			}
			return mod;
		}

		public static LightStyle.Mod operator +(LightStyle.Mod a, LightStyle.Mod.Element b)
		{
			a.mask = a.mask | LightStyle.Mod.ElementToMask(b);
			return a;
		}

		public static LightStyle.Mod operator +(LightStyle.Mod a, float b)
		{
			for (LightStyle.Mod.Element i = LightStyle.Mod.Element.Red; i < (LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle); i = (LightStyle.Mod.Element)((int)i + (int)LightStyle.Mod.Element.Green))
			{
				if ((a.mask & LightStyle.Mod.ElementToMask(i)) == LightStyle.Mod.ElementToMask(i))
				{
					a.SetFaceValue(i, a.GetFaceValue(i) + b);
				}
			}
			return a;
		}

		public static LightStyle.Mod operator +(LightStyle.Mod a, Color b)
		{
			for (int i = 0; i < 4; i++)
			{
				if ((a.mask & LightStyle.Mod.ElementToMask((LightStyle.Mod.Element)(0 + i))) == LightStyle.Mod.ElementToMask((LightStyle.Mod.Element)(0 + i)))
				{
					a.SetFaceValue((LightStyle.Mod.Element)(0 + i), a.GetFaceValue((LightStyle.Mod.Element)(0 + i)) + b[i]);
				}
			}
			return a;
		}

		public static Color operator +(Color b, LightStyle.Mod a)
		{
			for (int i = 0; i < 4; i++)
			{
				if ((a.mask & LightStyle.Mod.ElementToMask((LightStyle.Mod.Element)(0 + i))) == LightStyle.Mod.ElementToMask((LightStyle.Mod.Element)(0 + i)))
				{
					int faceValue = i;
					float item = (*stackVariable17)[faceValue];
					b[faceValue] = item + a.GetFaceValue((LightStyle.Mod.Element)(0 + i));
				}
			}
			return b;
		}

		public static LightStyle.Mod operator &(LightStyle.Mod a, LightStyle.Mod b)
		{
			for (LightStyle.Mod.Element i = LightStyle.Mod.Element.Red; i < (LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle); i = (LightStyle.Mod.Element)((int)i + (int)LightStyle.Mod.Element.Green))
			{
				if ((a.mask & LightStyle.Mod.ElementToMask(i)) == LightStyle.Mod.ElementToMask(i) && (b.mask & LightStyle.Mod.ElementToMask(i)) == LightStyle.Mod.ElementToMask(i))
				{
					a.SetFaceValue(i, b.GetFaceValue(i));
				}
			}
			return a;
		}

		public static LightStyle.Mod operator &(LightStyle.Mod a, LightStyle.Mod.Mask b)
		{
			a.mask = a.mask & b;
			return a;
		}

		public static bool operator &(LightStyle.Mod a, LightStyle.Mod.Element b)
		{
			return a.Contains(b);
		}

		public static LightStyle.Mod operator |(LightStyle.Mod a, LightStyle.Mod b)
		{
			for (LightStyle.Mod.Element i = LightStyle.Mod.Element.Red; i < (LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle); i = (LightStyle.Mod.Element)((int)i + (int)LightStyle.Mod.Element.Green))
			{
				if ((a.mask & LightStyle.Mod.ElementToMask(i)) != LightStyle.Mod.ElementToMask(i) && (b.mask & LightStyle.Mod.ElementToMask(i)) == LightStyle.Mod.ElementToMask(i))
				{
					a.SetModify(i, b.GetFaceValue(i));
				}
			}
			return a;
		}

		public static LightStyle.Mod operator |(LightStyle.Mod a, LightStyle.Mod.Mask b)
		{
			a.mask = a.mask | b;
			return a;
		}

		public static float? operator |(LightStyle.Mod a, LightStyle.Mod.Element b)
		{
			return a[b];
		}

		public static LightStyle.Mod operator /(LightStyle.Mod a, LightStyle.Mod b)
		{
			LightStyle.Mod.Mask mask = a.mask & b.mask;
			for (LightStyle.Mod.Element i = LightStyle.Mod.Element.Red; i < (LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle); i = (LightStyle.Mod.Element)((int)i + (int)LightStyle.Mod.Element.Green))
			{
				if ((mask & LightStyle.Mod.ElementToMask(i)) == LightStyle.Mod.ElementToMask(i))
				{
					a.SetFaceValue(i, a.GetFaceValue(i) / b.GetFaceValue(i));
				}
			}
			return a;
		}

		public static LightStyle.Mod operator /(LightStyle.Mod a, float b)
		{
			for (LightStyle.Mod.Element i = LightStyle.Mod.Element.Red; i < (LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle); i = (LightStyle.Mod.Element)((int)i + (int)LightStyle.Mod.Element.Green))
			{
				if ((a.mask & LightStyle.Mod.ElementToMask(i)) == LightStyle.Mod.ElementToMask(i))
				{
					a.SetFaceValue(i, a.GetFaceValue(i) / b);
				}
			}
			return a;
		}

		public static LightStyle.Mod operator /(LightStyle.Mod a, Color b)
		{
			for (int i = 0; i < 4; i++)
			{
				if ((a.mask & LightStyle.Mod.ElementToMask((LightStyle.Mod.Element)(0 + i))) == LightStyle.Mod.ElementToMask((LightStyle.Mod.Element)(0 + i)))
				{
					a.SetFaceValue((LightStyle.Mod.Element)(0 + i), a.GetFaceValue((LightStyle.Mod.Element)(0 + i)) / b[i]);
				}
			}
			return a;
		}

		public static Color operator /(Color b, LightStyle.Mod a)
		{
			for (int i = 0; i < 4; i++)
			{
				if ((a.mask & LightStyle.Mod.ElementToMask((LightStyle.Mod.Element)(0 + i))) == LightStyle.Mod.ElementToMask((LightStyle.Mod.Element)(0 + i)))
				{
					int faceValue = i;
					float item = (*stackVariable17)[faceValue];
					b[faceValue] = item / a.GetFaceValue((LightStyle.Mod.Element)(0 + i));
				}
			}
			return b;
		}

		public static LightStyle.Mod operator ^(LightStyle.Mod a, LightStyle.Mod b)
		{
			for (LightStyle.Mod.Element i = LightStyle.Mod.Element.Red; i < (LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle); i = (LightStyle.Mod.Element)((int)i + (int)LightStyle.Mod.Element.Green))
			{
				if ((a.mask & LightStyle.Mod.ElementToMask(i)) == LightStyle.Mod.ElementToMask(i))
				{
					if ((b.mask & LightStyle.Mod.ElementToMask(i)) == LightStyle.Mod.ElementToMask(i))
					{
						a.SetFaceValue(i, b.GetFaceValue(i));
					}
				}
				else if ((b.mask & LightStyle.Mod.ElementToMask(i)) == LightStyle.Mod.ElementToMask(i))
				{
					a.SetModify(i, b.GetFaceValue(i));
				}
			}
			return a;
		}

		public static LightStyle.Mod operator ^(LightStyle.Mod a, LightStyle.Mod.Mask b)
		{
			a.mask = a.mask ^ b;
			return a;
		}

		public static float operator ^(LightStyle.Mod a, LightStyle.Mod.Element b)
		{
			return a.GetFaceValue(b);
		}

		public static explicit operator Mod(Light light)
		{
			LightStyle.Mod mod = new LightStyle.Mod();
			if (light)
			{
				mod.color = light.color;
				mod.intensity = light.intensity;
				mod.range = light.range;
				mod.spotAngle = light.spotAngle;
				switch (light.type)
				{
					case LightType.Spot:
					{
						mod.mask = LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha | LightStyle.Mod.Mask.Intensity | LightStyle.Mod.Mask.Range | LightStyle.Mod.Mask.SpotAngle;
						break;
					}
					case LightType.Directional:
					{
						mod.mask = LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha | LightStyle.Mod.Mask.Intensity;
						break;
					}
					case LightType.Point:
					{
						mod.mask = LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha | LightStyle.Mod.Mask.Intensity | LightStyle.Mod.Mask.Range;
						break;
					}
				}
			}
			return mod;
		}

		public static explicit operator Mod(Color color)
		{
			LightStyle.Mod mod = new LightStyle.Mod()
			{
				color = color,
				mask = LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha
			};
			return mod;
		}

		public static explicit operator Mod(float intensity)
		{
			return new LightStyle.Mod()
			{
				intensity = intensity,
				mask = LightStyle.Mod.Mask.Intensity
			};
		}

		public static bool operator @false(LightStyle.Mod b)
		{
			return (int)(b.mask & (LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha | LightStyle.Mod.Mask.Intensity | LightStyle.Mod.Mask.Range | LightStyle.Mod.Mask.SpotAngle)) == 0;
		}

		public static LightStyle.Mod operator *(LightStyle.Mod a, LightStyle.Mod b)
		{
			LightStyle.Mod.Mask mask = a.mask & b.mask;
			for (LightStyle.Mod.Element i = LightStyle.Mod.Element.Red; i < (LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle); i = (LightStyle.Mod.Element)((int)i + (int)LightStyle.Mod.Element.Green))
			{
				if ((mask & LightStyle.Mod.ElementToMask(i)) == LightStyle.Mod.ElementToMask(i))
				{
					a.SetFaceValue(i, a.GetFaceValue(i) * b.GetFaceValue(i));
				}
			}
			return a;
		}

		public static LightStyle.Mod operator *(LightStyle.Mod a, float b)
		{
			LightStyle.Mod mod = a;
			for (LightStyle.Mod.Element i = LightStyle.Mod.Element.Red; i < (LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle); i = (LightStyle.Mod.Element)((int)i + (int)LightStyle.Mod.Element.Green))
			{
				if ((a.mask & LightStyle.Mod.ElementToMask(i)) == LightStyle.Mod.ElementToMask(i))
				{
					mod.SetFaceValue(i, a.GetFaceValue(i) * b);
				}
			}
			return mod;
		}

		public static LightStyle.Mod operator *(LightStyle.Mod a, Color b)
		{
			for (int i = 0; i < 4; i++)
			{
				if ((a.mask & LightStyle.Mod.ElementToMask((LightStyle.Mod.Element)(0 + i))) == LightStyle.Mod.ElementToMask((LightStyle.Mod.Element)(0 + i)))
				{
					a.SetFaceValue((LightStyle.Mod.Element)(0 + i), a.GetFaceValue((LightStyle.Mod.Element)(0 + i)) * b[i]);
				}
			}
			return a;
		}

		public static Color operator *(Color b, LightStyle.Mod a)
		{
			for (int i = 0; i < 4; i++)
			{
				if ((a.mask & LightStyle.Mod.ElementToMask((LightStyle.Mod.Element)(0 + i))) == LightStyle.Mod.ElementToMask((LightStyle.Mod.Element)(0 + i)))
				{
					int faceValue = i;
					float item = (*stackVariable17)[faceValue];
					b[faceValue] = item * a.GetFaceValue((LightStyle.Mod.Element)(0 + i));
				}
			}
			return b;
		}

		public static LightStyle.Mod operator ~(LightStyle.Mod a)
		{
			a.mask = ~a.mask & (LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha | LightStyle.Mod.Mask.Intensity | LightStyle.Mod.Mask.Range | LightStyle.Mod.Mask.SpotAngle);
			return a;
		}

		public static LightStyle.Mod operator -(LightStyle.Mod a, LightStyle.Mod b)
		{
			LightStyle.Mod.Mask mask = a.mask & b.mask;
			for (LightStyle.Mod.Element i = LightStyle.Mod.Element.Red; i < (LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle); i = (LightStyle.Mod.Element)((int)i + (int)LightStyle.Mod.Element.Green))
			{
				if ((mask & LightStyle.Mod.ElementToMask(i)) == LightStyle.Mod.ElementToMask(i))
				{
					a.SetFaceValue(i, a.GetFaceValue(i) - b.GetFaceValue(i));
				}
			}
			return a;
		}

		public static LightStyle.Mod operator -(LightStyle.Mod a, LightStyle.Mod.Element b)
		{
			a.mask = a.mask & LightStyle.Mod.ElementToMaskNot(b);
			return a;
		}

		public static LightStyle.Mod operator -(LightStyle.Mod a, float b)
		{
			for (LightStyle.Mod.Element i = LightStyle.Mod.Element.Red; i < (LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle); i = (LightStyle.Mod.Element)((int)i + (int)LightStyle.Mod.Element.Green))
			{
				if ((a.mask & LightStyle.Mod.ElementToMask(i)) == LightStyle.Mod.ElementToMask(i))
				{
					a.SetFaceValue(i, a.GetFaceValue(i) - b);
				}
			}
			return a;
		}

		public static LightStyle.Mod operator -(LightStyle.Mod a, Color b)
		{
			for (int i = 0; i < 4; i++)
			{
				if ((a.mask & LightStyle.Mod.ElementToMask((LightStyle.Mod.Element)(0 + i))) == LightStyle.Mod.ElementToMask((LightStyle.Mod.Element)(0 + i)))
				{
					a.SetFaceValue((LightStyle.Mod.Element)(0 + i), a.GetFaceValue((LightStyle.Mod.Element)(0 + i)) - b[i]);
				}
			}
			return a;
		}

		public static Color operator -(Color b, LightStyle.Mod a)
		{
			for (int i = 0; i < 4; i++)
			{
				if ((a.mask & LightStyle.Mod.ElementToMask((LightStyle.Mod.Element)(0 + i))) == LightStyle.Mod.ElementToMask((LightStyle.Mod.Element)(0 + i)))
				{
					int faceValue = i;
					float item = (*stackVariable17)[faceValue];
					b[faceValue] = item - a.GetFaceValue((LightStyle.Mod.Element)(0 + i));
				}
			}
			return b;
		}

		public static bool operator @true(LightStyle.Mod a)
		{
			return (int)(a.mask & (LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha | LightStyle.Mod.Mask.Intensity | LightStyle.Mod.Mask.Range | LightStyle.Mod.Mask.SpotAngle)) != 0;
		}

		public static LightStyle.Mod operator -(LightStyle.Mod a)
		{
			for (LightStyle.Mod.Element i = LightStyle.Mod.Element.Red; i < (LightStyle.Mod.Element.Green | LightStyle.Mod.Element.Blue | LightStyle.Mod.Element.Alpha | LightStyle.Mod.Element.Intensity | LightStyle.Mod.Element.Range | LightStyle.Mod.Element.SpotAngle); i = (LightStyle.Mod.Element)((int)i + (int)LightStyle.Mod.Element.Green))
			{
				a.SetFaceValue(i, -a.GetFaceValue(i));
			}
			return a;
		}

		public void SetFaceValue(LightStyle.Mod.Element element, float value)
		{
			switch (element)
			{
				case LightStyle.Mod.Element.Red:
				{
					this.r = value;
					break;
				}
				case LightStyle.Mod.Element.Green:
				{
					this.g = value;
					break;
				}
				case LightStyle.Mod.Element.Blue:
				{
					this.b = value;
					break;
				}
				case LightStyle.Mod.Element.Alpha:
				{
					this.a = value;
					break;
				}
				case LightStyle.Mod.Element.Intensity:
				{
					this.intensity = value;
					break;
				}
				case LightStyle.Mod.Element.Range:
				{
					this.range = value;
					break;
				}
				case LightStyle.Mod.Element.SpotAngle:
				{
					this.spotAngle = value;
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException("element");
				}
			}
		}

		public void SetModify(LightStyle.Mod.Element element)
		{
			LightStyle.Mod mask = this;
			mask.mask = mask.mask | LightStyle.Mod.ElementToMask(element);
		}

		public void SetModify(LightStyle.Mod.Element element, float assignValue)
		{
			this.SetFaceValue(element, assignValue);
			LightStyle.Mod mask = this;
			mask.mask = mask.mask | LightStyle.Mod.ElementToMask(element);
		}

		public void ToggleModify(LightStyle.Mod.Element element)
		{
			LightStyle.Mod mask = this;
			mask.mask = mask.mask ^ LightStyle.Mod.ElementToMask(element);
		}

		public enum Element
		{
			Red,
			Green,
			Blue,
			Alpha,
			Intensity,
			Range,
			SpotAngle
		}

		[Flags]
		public enum Mask
		{
			Red = 1,
			Green = 2,
			Blue = 4,
			Alpha = 8,
			Intensity = 16,
			Range = 32,
			SpotAngle = 64
		}
	}

	public abstract class Simulation : IDisposable
	{
		protected LightStyle creator;

		protected LightStyle.Mod mod;

		protected double startTime;

		protected double nextBindTime;

		protected double lastSimulateTime;

		private bool isDisposing;

		private bool destroyed;

		public bool alive
		{
			get
			{
				return !this.destroyed;
			}
		}

		public bool disposed
		{
			get
			{
				return this.destroyed;
			}
		}

		protected Simulation(LightStyle creator)
		{
			this.creator = creator;
		}

		public LightStyle.Mod BindMod(LightStyle.Mod.Mask mask)
		{
			if (!this.destroyed)
			{
				this.UpdateBinding();
			}
			LightStyle.Mod mod = this.mod;
			mod.mask = mod.mask & mask;
			return mod;
		}

		public void BindToLight(Light light)
		{
			if (this.destroyed)
			{
				return;
			}
			this.UpdateBinding();
			this.mod.ApplyTo(light);
		}

		public void BindToLight(Light light, LightStyle.Mod.Mask mask)
		{
			if (mask != (LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha | LightStyle.Mod.Mask.Intensity | LightStyle.Mod.Mask.Range | LightStyle.Mod.Mask.SpotAngle))
			{
				if (this.destroyed)
				{
					return;
				}
				this.UpdateBinding();
				if ((this.mod.mask & mask) == this.mod.mask)
				{
					this.mod.ApplyTo(light);
				}
				else
				{
					LightStyle.Mod mod = this.mod;
					mod.mask = mod.mask & mask;
					mod.ApplyTo(light);
				}
			}
			else
			{
				this.BindToLight(light);
			}
		}

		public void Dispose()
		{
			if (this.isDisposing || this.destroyed)
			{
				return;
			}
			this.isDisposing = true;
			bool flag = true;
			try
			{
				flag = this.creator.DeconstructSimulation(this);
			}
			finally
			{
				this.isDisposing = false;
				this.destroyed = flag;
			}
		}

		protected virtual void OnTimeReset()
		{
		}

		public void ResetTime(double time)
		{
			if (this.startTime != time)
			{
				this.startTime = time;
				this.OnTimeReset();
			}
		}

		protected abstract void Simulate(double currentTime);

		private void UpdateBinding()
		{
			double num = LightStyle.time;
			if (num >= this.nextBindTime)
			{
				this.Simulate(num);
				this.lastSimulateTime = num;
			}
		}
	}

	public abstract class Simulation<Style> : LightStyle.Simulation
	where Style : LightStyle
	{
		protected Style creator
		{
			get
			{
				return (Style)this.creator;
			}
			set
			{
				this.creator = value;
			}
		}

		protected Simulation(Style creator) : base(creator)
		{
		}
	}
}