using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class LightStylist : MonoBehaviour
{
	private const float kDefaultFadeLength = 0.3f;

	[SerializeField]
	protected Light[] lights;

	[SerializeField]
	protected LightStyle _lightStyle;

	private LightStyle crossfadeThisFrame;

	private LightStyle crossfadeNextFrame;

	private float crossfadeLength;

	protected LightStyle.Simulation simulationIdle;

	protected LightStyle.Simulation simulationPlaying;

	[HideInInspector]
	[SerializeField]
	protected LightStyle.Mod.Mask _mask = LightStyle.Mod.Mask.Red | LightStyle.Mod.Mask.Green | LightStyle.Mod.Mask.Blue | LightStyle.Mod.Mask.Alpha | LightStyle.Mod.Mask.Intensity | LightStyle.Mod.Mask.Range | LightStyle.Mod.Mask.SpotAngle;

	private Dictionary<LightStyle, LightStylist.Clip> clips;

	private LightStylist.Clip[] sortingArray;

	private int clipsInSortingArray;

	private bool awoke;

	public LightStylist ensuredAwake
	{
		get
		{
			this.Awake();
			return this;
		}
	}

	public LightStyle style
	{
		get
		{
			return this._lightStyle;
		}
		set
		{
			if (this._lightStyle != value)
			{
				if (this._lightStyle)
				{
					this.simulationIdle.Dispose();
					this.simulationIdle = null;
				}
				else if (this.simulationIdle != null)
				{
					this.simulationIdle.Dispose();
					this.simulationIdle = null;
				}
				this._lightStyle = value;
				if (this._lightStyle)
				{
					this.simulationIdle = this._lightStyle.CreateSimulation(LightStyle.time, this);
				}
			}
			else if (value && (this.simulationIdle == null || this.simulationIdle.disposed))
			{
				this.simulationIdle = this._lightStyle.CreateSimulation(LightStyle.time, this);
			}
		}
	}

	public IEnumerable<float> Weights
	{
		get
		{
			LightStylist.<>c__Iterator3E variable = null;
			return variable;
		}
	}

	public LightStylist()
	{
	}

	private void Awake()
	{
		if (!this.awoke)
		{
			this.clips = new Dictionary<LightStyle, LightStylist.Clip>();
			this.awoke = true;
		}
	}

	public bool Blend(LightStyle style, float targetWeight, float fadeLength)
	{
		float single;
		float single1;
		if (fadeLength <= 0f)
		{
			this.Play(style);
			return true;
		}
		targetWeight = Mathf.Clamp01(targetWeight);
		if (style != this._lightStyle)
		{
			LightStylist.Clip orMakeClip = this.GetOrMakeClip(style);
			if (Mathf.Approximately(orMakeClip.weight, targetWeight))
			{
				return true;
			}
			orMakeClip.weight = Mathf.MoveTowards(orMakeClip.weight, targetWeight, Time.deltaTime / fadeLength);
			float single2 = this.CalculateSumWeight(false, out single1);
			if (single2 != orMakeClip.weight && single2 > 1f)
			{
				float single3 = single2 - orMakeClip.weight;
				foreach (LightStylist.Clip value in this.clips.Values)
				{
					if (value == orMakeClip)
					{
						continue;
					}
					LightStylist.Clip clip = value;
					clip.weight = clip.weight / single3;
					LightStylist.Clip clip1 = value;
					clip1.weight = clip1.weight * (1f - orMakeClip.weight);
				}
			}
		}
		else
		{
			float single4 = this.CalculateSumWeight(true, out single);
			if (Mathf.Approximately(1f - single4, targetWeight))
			{
				return true;
			}
			float single5 = Mathf.MoveTowards(single4, 1f - targetWeight, Time.deltaTime / fadeLength);
			if (single5 > 0f)
			{
				float single6 = single5 / single4;
				foreach (LightStylist.Clip value1 in this.clips.Values)
				{
					LightStylist.Clip clip2 = value1;
					clip2.weight = clip2.weight * single6;
				}
			}
			else
			{
				foreach (LightStylist.Clip value2 in this.clips.Values)
				{
					value2.weight = 0f;
				}
			}
		}
		return false;
	}

	public bool Blend(LightStyle style, float targetWeight)
	{
		return this.Blend(style, targetWeight, 0.3f);
	}

	public bool Blend(LightStyle style)
	{
		return this.Blend(style, 1f, 0.3f);
	}

	private float CalculateSumWeight(bool normalize, out float maxWeight)
	{
		float single = 0f;
		maxWeight = 0f;
		foreach (LightStylist.Clip value in this.clips.Values)
		{
			if (value.weight > maxWeight)
			{
				maxWeight = value.weight;
			}
			else if (value.weight < 0f)
			{
				value.weight = 0f;
			}
			single = single + value.weight;
		}
		if (normalize && single > 1f)
		{
			float single1 = single;
			maxWeight = maxWeight / single1;
			foreach (LightStylist.Clip clip in this.clips.Values)
			{
				LightStylist.Clip clip1 = clip;
				clip1.weight = clip1.weight / single1;
			}
			single = 1f;
		}
		return single;
	}

	public bool CrossFade(LightStyle style, float fadeLength)
	{
		if (this.crossfadeThisFrame != style)
		{
			this.crossfadeThisFrame = style;
			this.crossfadeNextFrame = null;
			this.crossfadeLength = fadeLength;
			if (this.Blend(style, 1f, fadeLength))
			{
				this.CrossFadeDone();
				return true;
			}
		}
		return false;
	}

	public bool CrossFade(LightStyle style)
	{
		return this.CrossFade(style, 0.3f);
	}

	private void CrossFadeDone()
	{
		LightStylist.Clip clip;
		if (this.clips.TryGetValue(this.crossfadeThisFrame, out clip))
		{
			this.clips.Remove(this.style);
			this.GetOrMakeClip(this._lightStyle).weight = 0f;
			this._lightStyle = this.style;
			this.simulationIdle = clip.simulation;
		}
		this.crossfadeThisFrame = null;
		this.crossfadeNextFrame = null;
	}

	public void EnsureAwake()
	{
		this.Awake();
	}

	private LightStylist.Clip GetOrMakeClip(LightStyle style)
	{
		LightStylist.Clip clip;
		if (this.clips.TryGetValue(style, out clip))
		{
			return clip;
		}
		clip = new LightStylist.Clip()
		{
			simulation = style.CreateSimulation(LightStyle.time, this)
		};
		this.clips[style] = clip;
		return clip;
	}

	protected void LateUpdate()
	{
		float single;
		LightStyle.Mod mod;
		int num;
		if (this.crossfadeThisFrame)
		{
			this.crossfadeNextFrame = this.crossfadeThisFrame;
			this.crossfadeThisFrame = null;
		}
		else if (this.crossfadeNextFrame && !this.CrossFade(this.crossfadeNextFrame, this.crossfadeLength))
		{
			this.crossfadeNextFrame = this.crossfadeThisFrame;
			this.crossfadeThisFrame = null;
		}
		float single1 = this.CalculateSumWeight(true, out single);
		if (single1 != 0f)
		{
			int count = this.clips.Count;
			if (this.clipsInSortingArray != count)
			{
				if (this.clipsInSortingArray > count)
				{
					while (this.clipsInSortingArray > count)
					{
						LightStylist lightStylist = this;
						int num1 = lightStylist.clipsInSortingArray - 1;
						num = num1;
						lightStylist.clipsInSortingArray = num1;
						this.sortingArray[num] = null;
					}
				}
				else if (this.sortingArray == null || (int)this.sortingArray.Length < count)
				{
					int num2 = count / 4;
					Array.Resize<LightStylist.Clip>(ref this.sortingArray, (num2 + (count % 4 != 0 ? 1 : 2)) * 4);
				}
			}
			int num3 = 0;
			foreach (LightStylist.Clip value in this.clips.Values)
			{
				if (value.weight <= 0f)
				{
					continue;
				}
				int num4 = num3;
				num3 = num4 + 1;
				this.sortingArray[num4] = value;
			}
			if (this.clipsInSortingArray >= num3)
			{
				while (this.clipsInSortingArray > num3)
				{
					LightStylist lightStylist1 = this;
					int num5 = lightStylist1.clipsInSortingArray - 1;
					num = num5;
					lightStylist1.clipsInSortingArray = num5;
					this.sortingArray[num] = null;
				}
			}
			else
			{
				this.clipsInSortingArray = num3;
			}
			Array.Sort<LightStylist.Clip>(this.sortingArray, 0, this.clipsInSortingArray);
			float single2 = this.sortingArray[0].weight;
			mod = this.sortingArray[0].simulation.BindMod(this._mask);
			for (int i = 1; i < this.clipsInSortingArray; i++)
			{
				LightStylist.Clip clip = this.sortingArray[i];
				single2 = single2 + clip.weight;
				mod = LightStyle.Mod.Lerp(mod, clip.simulation.BindMod(this._mask), clip.weight / single2, this._mask);
			}
			if (this._lightStyle)
			{
				LightStyle.Mod mod1 = this.simulationIdle.BindMod(this._mask);
				mod = (single1 >= 1f ? mod | mod1 : LightStyle.Mod.Lerp(mod, mod1, 1f - single1, this._mask));
			}
		}
		else
		{
			while (this.clipsInSortingArray > 0)
			{
				LightStylist lightStylist2 = this;
				int num6 = lightStylist2.clipsInSortingArray - 1;
				num = num6;
				lightStylist2.clipsInSortingArray = num6;
				this.sortingArray[num] = null;
			}
			if (!this._lightStyle)
			{
				return;
			}
			mod = this.simulationIdle.BindMod(this._mask);
		}
		Light[] lightArray = this.lights;
		for (int j = 0; j < (int)lightArray.Length; j++)
		{
			Light light = lightArray[j];
			if (light)
			{
				mod.ApplyTo(light, this._mask);
			}
		}
	}

	public void Play(LightStyle style, double time)
	{
		if (style != this._lightStyle)
		{
			LightStylist.Clip orMakeClip = this.GetOrMakeClip(style);
			this.clips.Clear();
			this.clips[style] = orMakeClip;
			orMakeClip.weight = 1f;
			orMakeClip.simulation.ResetTime(time);
		}
		else
		{
			this.clips.Clear();
		}
	}

	public void Play(LightStyle style)
	{
		if (style != this._lightStyle)
		{
			LightStylist.Clip orMakeClip = this.GetOrMakeClip(style);
			this.clips.Clear();
			this.clips[style] = orMakeClip;
			orMakeClip.weight = 1f;
			orMakeClip.simulation.ResetTime(LightStyle.time);
		}
		else
		{
			this.clips.Clear();
		}
	}

	protected void Reset()
	{
		this.lights = base.GetComponents<Light>();
	}

	private void Start()
	{
		if (!this._lightStyle)
		{
			this._lightStyle = LightStyleDefault.Singleton;
		}
		this.simulationIdle = this._lightStyle.CreateSimulation(LightStyle.time, this);
	}

	protected sealed class Clip : IComparable<LightStylist.Clip>
	{
		public float weight;

		public LightStyle.Simulation simulation;

		public Clip()
		{
		}

		public int CompareTo(LightStylist.Clip other)
		{
			return this.weight.CompareTo(other.weight);
		}
	}
}