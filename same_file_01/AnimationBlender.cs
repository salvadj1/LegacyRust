using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class AnimationBlender
{
	public static AnimationBlender.ChannelConfig Alias(this AnimationBlender.ChannelField Field, string Name)
	{
		return new AnimationBlender.ChannelConfig(Name, Field);
	}

	public static AnimationBlender.ChannelConfig[] Alias(this AnimationBlender.ChannelField Field, AnimationBlender.ChannelConfig[] Array, int Index, string Name)
	{
		Array[Index] = Field.Alias(Name);
		return Array;
	}

	public static AnimationBlender.MixerConfig Alias(this AnimationBlender.ResidualField ResidualField, UnityEngine.Animation Animation, params AnimationBlender.ChannelConfig[] ChannelAliases)
	{
		return new AnimationBlender.MixerConfig(Animation, ResidualField, ChannelAliases);
	}

	public static AnimationBlender.MixerConfig Alias(this AnimationBlender.ResidualField ResidualField, UnityEngine.Animation Animation, int ChannelCount)
	{
		return new AnimationBlender.MixerConfig(Animation, ResidualField, new AnimationBlender.ChannelConfig[ChannelCount]);
	}

	private static void ArrayResize<T>(ref T[] array, int size)
	{
		Array.Resize<T>(ref array, size);
	}

	public static AnimationBlender.Mixer Create(this AnimationBlender.MixerConfig Config)
	{
		return new AnimationBlender.Mixer(Config);
	}

	public static AnimationBlender.ChannelConfig[] Define(this AnimationBlender.ChannelConfig[] Array, int Index, string Name, AnimationBlender.ChannelField Field)
	{
		return Field.Alias(Array, Index, Name);
	}

	private static int GetClear(ref int value)
	{
		int num = value;
		value = 0;
		return num;
	}

	private static void OneWeight(ref AnimationBlender.WeightUnit weight)
	{
		float single = 1f;
		float single1 = single;
		weight.normalized = single;
		float single2 = single1;
		single1 = single2;
		weight.scaled = single2;
		weight.raw = single1;
	}

	private static void OneWeightScale(ref AnimationBlender.WeightUnit weight)
	{
		float single = 1f;
		float single1 = single;
		weight.normalized = single;
		weight.scaled = single1;
	}

	private static void SetWeight(ref AnimationBlender.WeightUnit weight)
	{
		weight.scaled = weight.raw;
		weight.normalized = 1f;
	}

	private static AnimationBlender.opt<T> to_opt<T>(Nullable<T> nullable)
	where T : struct
	{
		AnimationBlender.opt<T> value;
		if (nullable.HasValue)
		{
			value = nullable.Value;
		}
		else
		{
			value = AnimationBlender.opt<T>.none;
		}
		return value;
	}

	private static AnimationBlender.Weighted<T>[] WeightArray<T>(int size)
	{
		return new AnimationBlender.Weighted<T>[size];
	}

	private static AnimationBlender.Weighted<T>[] WeightArray<T>(T[] source)
	{
		if (object.ReferenceEquals(source, null))
		{
			return null;
		}
		int length = (int)source.Length;
		AnimationBlender.Weighted<T>[] weightedArray = AnimationBlender.WeightArray<T>(length);
		for (int i = 0; i < length; i++)
		{
			weightedArray[i].@value = source[i];
		}
		return weightedArray;
	}

	private static bool WeightOf<T>(AnimationBlender.Weighted<T>[] items, int[] index, out AnimationBlender.WeightResult result)
	{
		result = new AnimationBlender.WeightResult();
		float single;
		float single1;
		float single2;
		bool flag;
		float single3 = 0f;
		float single4 = 0f;
		int num = -1;
		int num1 = 0;
		int num2 = 0;
		int length = (int)index.Length - 1;
		while (num2 <= length)
		{
			float single5 = items[index[num2]].weight.raw;
			float single6 = single5;
			if (single5 > 0f)
			{
				num1++;
				if (single6 < 1f)
				{
					AnimationBlender.SetWeight(ref items[index[num2]].weight);
				}
				else
				{
					single6 = 1f;
					AnimationBlender.OneWeight(ref items[index[num2]].weight);
				}
				single3 = single3 + single6;
				if (single6 > single4)
				{
					single4 = single6;
					num = num2;
				}
				num2++;
			}
			else
			{
				AnimationBlender.ZeroWeight(ref items[index[num2]].weight);
				int num3 = index[length];
				int num4 = length;
				length = num4 - 1;
				index[num4] = index[num2];
				index[num2] = num3;
			}
		}
		if (num != -1)
		{
			flag = true;
			if (num1 != 1)
			{
				if (single4 >= 1f)
				{
					single = single3;
				}
				else
				{
					single = 0f;
					single2 = 1f / single4;
					for (int i = 0; i < num1; i++)
					{
						single = single + items[index[i]].weight.SetScaledRecip(single2);
					}
				}
				if (single != 1f)
				{
					single1 = 0f;
					single2 = 1f / single;
					for (int j = 0; j < num1; j++)
					{
						single1 = single1 + items[index[j]].weight.SetNormalizedRecip(single2);
					}
				}
				else
				{
					single1 = 1f;
				}
			}
			else
			{
				single = 1f;
				single1 = 1f;
				AnimationBlender.OneWeightScale(ref items[index[0]].weight);
			}
		}
		else
		{
			single = 0f;
			single1 = 0f;
			flag = false;
		}
		result.count = num1;
		result.winner = num;
		result.sum.raw = single3;
		result.sum.scaled = single;
		result.sum.normalized = single1;
		return flag;
	}

	private static void ZeroWeight(ref AnimationBlender.WeightUnit weight)
	{
		float single = 0f;
		float single1 = single;
		weight.normalized = single;
		float single2 = single1;
		single1 = single2;
		weight.scaled = single2;
		weight.raw = single1;
	}

	private struct Channel
	{
		[NonSerialized]
		public AnimationBlender.ChannelField field;

		[NonSerialized]
		public string name;

		[NonSerialized]
		public bool active;

		[NonSerialized]
		public bool valid;

		[NonSerialized]
		public bool wasActive;

		[NonSerialized]
		public bool startedTransition;

		[NonSerialized]
		public AnimationBlender.ChannelCurve induce;

		[NonSerialized]
		public AnimationBlender.ChannelCurve reduce;

		[NonSerialized]
		public int index;

		[NonSerialized]
		public int animationIndex;

		[NonSerialized]
		public float maxBlend;

		[NonSerialized]
		public float playbackRate;

		[NonSerialized]
		public float startTime;

		public Channel(int index, int animationIndex, string name, AnimationBlender.ChannelField field)
		{
			float single;
			this.index = index;
			this.animationIndex = animationIndex;
			this.name = name;
			this.field = field;
			AnimationBlender.CurveInfo curveInfo = field.inCurveInfo;
			AnimationBlender.State state = new AnimationBlender.State();
			AnimationBlender.Influence influence = new AnimationBlender.Influence();
			this.induce = new AnimationBlender.ChannelCurve(curveInfo, state, influence, field, true);
			AnimationBlender.CurveInfo curveInfo1 = field.outCurveInfo;
			AnimationBlender.State state1 = new AnimationBlender.State();
			AnimationBlender.Influence influence1 = new AnimationBlender.Influence();
			this.reduce = new AnimationBlender.ChannelCurve(curveInfo1, state1, influence1, field, false);
			this.active = false;
			this.wasActive = false;
			this.startedTransition = false;
			this.valid = animationIndex != -1;
			if (field.residualBlend > 0f)
			{
				single = (field.residualBlend < 1f ? 1f - field.residualBlend : 0f);
			}
			else
			{
				single = 1f;
			}
			this.maxBlend = single;
			this.startTime = field.startFrame;
			this.playbackRate = field.playbackRate;
		}

		private bool StartTransition(ref AnimationBlender.ChannelCurve from, ref AnimationBlender.ChannelCurve to, ref float dt, bool startNow)
		{
			if (to.state.delay == 0f && startNow)
			{
				to.state.delay = to.delayDuration;
			}
			if (to.state.delay > dt)
			{
				to.state.delay = to.state.delay - dt;
				return false;
			}
			dt = dt - to.state.delay;
			to.state.delay = 0f;
			to.influence.percent = 0f;
			to.influence.duration = from.state.percent * to.info.duration;
			to.influence.@value = from.state.@value;
			to.influence.active = from.state.percent > 0f;
			to.influence.timeleft = to.influence.duration;
			from.state.delay = to.delayDuration;
			from.state.active = false;
			to.state.active = true;
			if (to.induces)
			{
				from.state.time = from.info.start;
				from.state.percent = 0f;
			}
			return true;
		}

		private float Step(bool transitioning, ref AnimationBlender.ChannelCurve from, ref AnimationBlender.ChannelCurve to, ref float dt)
		{
			float single;
			if (transitioning && to.state.delay > 0f)
			{
				return from.state.@value;
			}
			float single1 = dt;
			float single2 = dt;
			float single3 = to.state.time;
			if (to.induces)
			{
				to.state.time = to.state.time + dt;
				if (to.state.time < to.info.end)
				{
					single1 = 0f;
					to.state.percent = to.info.TimeToPercent(to.state.time);
				}
				else
				{
					single1 = to.state.time - to.info.end;
					to.state.time = to.info.end;
					to.state.percent = 1f;
					from.state.delay = from.delayDuration;
				}
			}
			else if (to.influence.duration != 0f)
			{
				float single4 = from.info.duration / to.influence.duration;
				to.state.time = to.state.time + dt * single4;
				if (to.state.time < to.info.end)
				{
					single1 = 0f;
					to.state.percent = to.info.TimeToPercent(to.state.time);
				}
				else
				{
					single1 = (to.state.time - to.info.end) / single4;
					to.state.percent = 1f;
					to.state.time = to.info.end;
				}
			}
			else
			{
				single1 = dt;
				to.state.percent = 1f;
				to.state.time = to.info.end;
			}
			float single5 = to.info.SampleTime(to.state.time);
			if (to.influence.active)
			{
				if (to.induces)
				{
					if (to.influence.timeleft <= dt)
					{
						single2 = dt - to.influence.timeleft;
						to.influence.timeleft = 0f;
						to.influence.percent = 0f;
						to.influence.active = false;
					}
					else
					{
						to.influence.timeleft = to.influence.timeleft - dt;
						single2 = 0f;
						to.influence.percent = to.influence.timeleft / to.influence.duration;
					}
				}
				else if (to.state.percent >= 1f && to.influence.active)
				{
					to.influence.active = false;
					from.state = new AnimationBlender.State();
				}
			}
			if (!to.induces)
			{
				to.state.@value = to.influence.@value * single5;
			}
			else
			{
				single = (!to.influence.active ? single5 : single5 + (to.influence.@value - single5) * to.influence.percent);
				to.state.@value = single;
			}
			if (single2 >= single1)
			{
				dt = single1;
			}
			else
			{
				dt = single2;
			}
			return to.state.@value;
		}

		public float Update(float dt)
		{
			bool flag;
			bool flag1 = this.active != this.wasActive;
			if (flag1)
			{
				bool flag2 = this.startedTransition != this.active;
				this.startedTransition = this.active;
				flag = (!this.active ? this.StartTransition(ref this.induce, ref this.reduce, ref dt, flag2) : this.StartTransition(ref this.reduce, ref this.induce, ref dt, flag2));
				if (flag)
				{
					flag1 = false;
					this.wasActive = this.active;
				}
			}
			if (this.wasActive)
			{
				return this.Step(flag1, ref this.reduce, ref this.induce, ref dt);
			}
			return this.Step(flag1, ref this.induce, ref this.reduce, ref dt);
		}
	}

	public struct ChannelConfig
	{
		[NonSerialized]
		public readonly string name;

		[NonSerialized]
		public readonly AnimationBlender.ChannelField field;

		public ChannelConfig(string name, AnimationBlender.ChannelField field)
		{
			this.name = name;
			this.field = field;
		}
	}

	private struct ChannelCurve
	{
		[NonSerialized]
		public AnimationBlender.CurveInfo info;

		[NonSerialized]
		public AnimationBlender.State state;

		[NonSerialized]
		public AnimationBlender.Influence influence;

		[NonSerialized]
		public float delayDuration;

		[NonSerialized]
		public bool induces;

		public ChannelCurve(AnimationBlender.CurveInfo info, AnimationBlender.State state, AnimationBlender.Influence influence, AnimationBlender.ChannelField field, bool induces)
		{
			this.info = info;
			this.state = state;
			this.influence = influence;
			this.induces = induces;
			this.delayDuration = (!induces ? field.outDelay : field.inDelay);
		}
	}

	[Serializable]
	public sealed class ChannelField : AnimationBlender.Field
	{
		[SerializeField]
		public float inDelay;

		[SerializeField]
		public float outDelay;

		[SerializeField]
		public float residualBlend;

		[SerializeField]
		public bool blockedByAnimation;

		[SerializeField]
		public AnimationCurve inCurve;

		[SerializeField]
		public AnimationCurve outCurve;

		public AnimationBlender.CurveInfo inCurveInfo
		{
			get
			{
				return new AnimationBlender.CurveInfo(this.inCurve);
			}
		}

		public AnimationBlender.CurveInfo outCurveInfo
		{
			get
			{
				return new AnimationBlender.CurveInfo(this.outCurve);
			}
		}

		public ChannelField()
		{
			this.clipName = string.Empty;
			this.playbackRate = 1f;
			this.inCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			this.outCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);
		}
	}

	public struct CurveInfo
	{
		[NonSerialized]
		public AnimationCurve curve;

		[NonSerialized]
		public int length;

		[NonSerialized]
		public float start;

		[NonSerialized]
		public float firstTime;

		[NonSerialized]
		public float end;

		[NonSerialized]
		public float lastTime;

		[NonSerialized]
		public float duration;

		[NonSerialized]
		public float percentRate;

		public CurveInfo(AnimationCurve curve)
		{
			float single;
			float single1;
			this.curve = curve;
			int num = curve.length;
			int num1 = num;
			this.length = num;
			if (num1 != 0)
			{
				this.firstTime = curve[0].time;
				single1 = (this.length != 1 ? curve[this.length - 1].time : this.firstTime);
				single = single1;
				this.lastTime = single1;
				this.end = single;
				this.start = (this.firstTime >= 0f ? 0f : this.firstTime);
				if (this.end < this.start)
				{
					this.end = this.start;
					this.start = this.lastTime;
				}
				this.duration = this.end - this.start;
				this.percentRate = 1f / this.duration;
			}
			else
			{
				float single2 = 0f;
				single = single2;
				this.duration = single2;
				float single3 = single;
				single = single3;
				this.lastTime = single3;
				float single4 = single;
				single = single4;
				this.end = single4;
				float single5 = single;
				single = single5;
				this.firstTime = single5;
				this.start = single;
				this.percentRate = Single.PositiveInfinity;
			}
		}

		public float PercentClamp(float percent)
		{
			float single;
			if (percent > 0f)
			{
				single = (percent < 1f ? percent : 1f);
			}
			else
			{
				single = 0f;
			}
			return single;
		}

		public float PercentToTime(float percent)
		{
			return this.start + this.duration * percent;
		}

		public float PercentToTimeClamped(float percent)
		{
			float single;
			if (percent > 0f)
			{
				single = (percent < 1f ? this.start + this.duration * percent : this.end);
			}
			else
			{
				single = this.start;
			}
			return single;
		}

		public float SamplePercent(float percent)
		{
			return this.SampleTime(this.PercentToTime(percent));
		}

		public float SamplePercentClamped(float percent)
		{
			return this.SamplePercent(this.PercentToTimeClamped(percent));
		}

		public float SampleTime(float time)
		{
			return this.curve.Evaluate(time);
		}

		public float SampleTimeClamped(float time)
		{
			return this.SampleTime(this.TimeClamp(time));
		}

		public float TimeClamp(float time)
		{
			float single;
			if (time < this.end)
			{
				single = (time > this.start ? time : this.start);
			}
			else
			{
				single = this.end;
			}
			return single;
		}

		public float TimeToPercent(float time)
		{
			return (time - this.start) / this.duration;
		}

		public float TimeToPercentClamped(float time)
		{
			float single;
			if (time < this.end)
			{
				single = (time > this.start ? (time - this.start) / this.duration : 1f);
			}
			else
			{
				single = 1f;
			}
			return single;
		}
	}

	[Serializable]
	public class Field
	{
		[SerializeField]
		public string clipName;

		[SerializeField]
		public float startFrame;

		[SerializeField]
		public float playbackRate;

		public bool defined
		{
			get
			{
				return !string.IsNullOrEmpty(this.clipName);
			}
		}

		public Field()
		{
		}
	}

	private struct Influence
	{
		[NonSerialized]
		public bool active;

		[NonSerialized]
		public float @value;

		[NonSerialized]
		public float percent;

		[NonSerialized]
		public float timeleft;

		[NonSerialized]
		public float duration;
	}

	public sealed class Mixer
	{
		[NonSerialized]
		private Animation animation;

		[NonSerialized]
		private AnimationBlender.ResidualField residualField;

		[NonSerialized]
		private AnimationState residualState;

		[NonSerialized]
		private AnimationBlendMode residualBlendMode;

		[NonSerialized]
		private AnimationBlendMode channelBlendMode;

		[NonSerialized]
		private AnimationState oneShotAnimation;

		[NonSerialized]
		private bool playingOneShot;

		[NonSerialized]
		private bool animationBlocking;

		[NonSerialized]
		private int trackerCount;

		[NonSerialized]
		private int channelCount;

		[NonSerialized]
		private int definedChannelCount;

		[NonSerialized]
		private int[] definedChannels;

		[NonSerialized]
		private AnimationBlender.TrackerBlender blender;

		[NonSerialized]
		private AnimationBlender.Weighted<AnimationBlender.Channel>[] channels;

		[NonSerialized]
		private AnimationBlender.Weighted<AnimationBlender.Tracker>[] trackers;

		[NonSerialized]
		private Dictionary<string, int> nameToChannel;

		[NonSerialized]
		private AnimationBlender.CurveInfo oneShotBlendIn;

		[NonSerialized]
		private bool hasOneShotBlendIn;

		[NonSerialized]
		private Queue<string> queuedAnimations;

		[NonSerialized]
		private float oneShotBlendInTime;

		[NonSerialized]
		private float sumWeight;

		[NonSerialized]
		private bool hasResidual;

		public bool isPlayingManualAnimation
		{
			get
			{
				return this.ManualAnimationsPlaying(false);
			}
		}

		public Mixer(AnimationBlender.MixerConfig config)
		{
			bool clip;
			AnimationState item;
			if (!config.animation)
			{
				throw new ArgumentException("null or missing", "config.animation");
			}
			this.animation = config.animation;
			this.residualField = config.residual;
			if (object.ReferenceEquals(config.residual, null) || !config.residual.defined)
			{
				clip = false;
			}
			else
			{
				clip = this.animation.GetClip(config.residual.clipName);
			}
			this.hasResidual = clip;
			this.oneShotBlendIn = (!this.hasResidual || object.ReferenceEquals(config.residual.introCurve, null) ? new AnimationBlender.CurveInfo() : config.residual.introCurveInfo);
			this.hasOneShotBlendIn = this.oneShotBlendIn.duration > 0f;
			if (!this.hasResidual)
			{
				item = null;
			}
			else
			{
				item = this.animation[config.residual.clipName];
			}
			this.residualState = item;
			this.channelCount = (int)config.channels.Length;
			this.channels = new AnimationBlender.Weighted<AnimationBlender.Channel>[this.channelCount];
			this.trackers = new AnimationBlender.Weighted<AnimationBlender.Tracker>[this.channelCount];
			this.trackerCount = 0;
			this.nameToChannel = new Dictionary<string, int>(this.channelCount);
			for (int i = 0; i < this.channelCount; i++)
			{
				AnimationBlender.ChannelField channelField = config.channels[i].field;
				string str = config.channels[i].name;
				this.nameToChannel.Add(str, i);
				int num = -1;
				if (channelField.defined)
				{
					AnimationClip animationClip = this.animation.GetClip(channelField.clipName);
					AnimationClip animationClip1 = animationClip;
					if (animationClip)
					{
						bool flag = false;
						while (!flag)
						{
							int num1 = num + 1;
							num = num1;
							bool flag1 = num1 == this.trackerCount;
							flag = flag1;
							if (!flag1)
							{
								bool flag2 = this.trackers[num].@value.clip == animationClip1;
								flag = flag2;
								if (!flag2)
								{
									continue;
								}
								this.trackers[num].@value.channelCount = this.trackers[num].@value.channelCount + 1;
							}
							else
							{
								this.trackers[num].@value.clip = animationClip1;
								this.trackers[num].@value.state = this.animation[channelField.clipName];
								this.trackers[num].@value.channelCount = 1;
								AnimationBlender.Mixer mixer = this;
								mixer.trackerCount = mixer.trackerCount + 1;
							}
						}
						AnimationBlender.Mixer mixer1 = this;
						mixer1.definedChannelCount = mixer1.definedChannelCount + 1;
					}
				}
				this.channels[i].@value = new AnimationBlender.Channel(i, num, str, channelField);
			}
			for (int j = 0; j < this.trackerCount; j++)
			{
				this.trackers[j].@value.channels = new int[AnimationBlender.GetClear(ref this.trackers[j].@value.channelCount)];
			}
			this.definedChannels = new int[AnimationBlender.GetClear(ref this.definedChannelCount)];
			for (int k = 0; k < this.channelCount; k++)
			{
				if (this.channels[k].@value.animationIndex != -1)
				{
					int[] numArray = this.trackers[this.channels[k].@value.animationIndex].@value.channels;
					int num2 = this.trackers[this.channels[k].@value.animationIndex].@value.channelCount;
					int num3 = num2;
					this.trackers[this.channels[k].@value.animationIndex].@value.channelCount = num2 + 1;
					int num4 = num3;
					int[] numArray1 = this.definedChannels;
					AnimationBlender.Mixer mixer2 = this;
					int num5 = mixer2.definedChannelCount;
					num3 = num5;
					mixer2.definedChannelCount = num5 + 1;
					int num6 = num3;
					int num7 = k;
					num3 = num7;
					numArray1[num6] = num7;
					numArray[num4] = num3;
				}
			}
			AnimationBlender.ArrayResize<AnimationBlender.Weighted<AnimationBlender.Tracker>>(ref this.trackers, this.trackerCount);
			AnimationBlender.ArrayResize<AnimationBlender.Weighted<AnimationBlender.Channel>>(ref this.channels, this.channelCount);
			AnimationBlender.ArrayResize<int>(ref this.definedChannels, this.definedChannelCount);
			for (int l = 0; l < this.trackerCount; l++)
			{
				AnimationBlender.ArrayResize<int>(ref this.trackers[l].@value.channels, this.trackers[l].@value.channelCount);
			}
			this.blender = new AnimationBlender.TrackerBlender(this.trackerCount);
			if (this.hasResidual)
			{
				if (this.residualField.changeAnimLayer)
				{
					this.residualState.layer = this.residualField.animLayer;
					for (int m = 0; m < this.trackerCount; m++)
					{
						this.trackers[m].@value.state.layer = this.residualField.animLayer;
					}
				}
				AnimationState animationState = this.residualState;
				AnimationBlendMode animationBlendMode = this.residualField.residualBlend;
				AnimationBlendMode animationBlendMode1 = animationBlendMode;
				this.residualBlendMode = animationBlendMode;
				animationState.blendMode = animationBlendMode1;
				this.channelBlendMode = this.residualField.channelBlend;
				for (int n = 0; n < this.trackerCount; n++)
				{
					this.trackers[n].@value.state.blendMode = this.channelBlendMode;
				}
			}
		}

		private float BindTracker(ref AnimationBlender.Weighted<AnimationBlender.Tracker> tracker, float externalBlend)
		{
			float single = tracker.weight.normalized;
			if (this.hasResidual)
			{
				single = single * tracker.@value.blendFraction;
			}
			if (this.blender.trackerWeight.sum.raw < 1f)
			{
				single = single * this.blender.trackerWeight.sum.raw;
			}
			if (single > 0f)
			{
				if (!tracker.@value.wasEnabled)
				{
					int num = 1;
					bool flag = (bool)num;
					tracker.@value.wasEnabled = (bool)num;
					tracker.@value.state.enabled = flag;
					tracker.@value.state.time = tracker.@value.startTime;
				}
				tracker.@value.state.weight = single * externalBlend;
				tracker.@value.state.speed = tracker.@value.playbackRate;
			}
			else if (tracker.@value.wasEnabled)
			{
				this.DisableTracker(ref tracker.@value);
			}
			return single;
		}

		public bool CrossFade(string animationName)
		{
			return this.CrossFadeOpt(animationName, AnimationBlender.opt<float>.none, AnimationBlender.opt<PlayMode>.none, AnimationBlender.opt<float>.none, AnimationBlender.opt<float>.none);
		}

		public bool CrossFade(string animationName, float fadeLen)
		{
			return this.CrossFadeOpt(animationName, fadeLen, AnimationBlender.opt<PlayMode>.none, AnimationBlender.opt<float>.none, AnimationBlender.opt<float>.none);
		}

		public bool CrossFade(string animationName, float fadeLen, PlayMode playMode)
		{
			return this.CrossFadeOpt(animationName, fadeLen, playMode, AnimationBlender.opt<float>.none, AnimationBlender.opt<float>.none);
		}

		public bool CrossFade(string animationName, float fadeLen, PlayMode playMode, float speed)
		{
			return this.CrossFadeOpt(animationName, fadeLen, playMode, speed, AnimationBlender.opt<float>.none);
		}

		public bool CrossFade(string animationName, float fadeLen, PlayMode playMode, float speed, float startTime)
		{
			return this.CrossFadeOpt(animationName, fadeLen, playMode, speed, startTime);
		}

		public bool CrossFade(string animationName, float fadeLen, PlayMode playMode, float? speed, float startTime)
		{
			return this.CrossFadeOpt(animationName, fadeLen, playMode, AnimationBlender.to_opt<float>(speed), startTime);
		}

		private void CrossFadeDirect(string animationName, AnimationBlender.opt<float> fadeLength, AnimationBlender.opt<PlayMode> playMode)
		{
			if (playMode.defined)
			{
				this.animation.CrossFade(animationName, fadeLength[0.3f], (PlayMode)playMode.@value);
			}
			else if (!fadeLength.defined)
			{
				this.animation.CrossFade(animationName);
			}
			else
			{
				this.animation.CrossFade(animationName, fadeLength.@value);
			}
		}

		private bool CrossFadeOpt(string animationName, AnimationBlender.opt<float> fadeLength, AnimationBlender.opt<PlayMode> playMode, AnimationBlender.opt<float> speed, AnimationBlender.opt<float> startTime)
		{
			if (!string.IsNullOrEmpty(animationName))
			{
				AnimationState item = this.animation[animationName];
				AnimationState animationState = item;
				if (item != null)
				{
					if (speed.defined)
					{
						animationState.speed = speed.@value;
					}
					this.CrossFadeDirect(animationName, fadeLength, playMode);
					this.queuedAnimations.Clear();
					this.playingOneShot = true;
					this.oneShotAnimation = animationState;
					if (startTime.defined)
					{
						animationState.time = startTime.@value;
					}
					return true;
				}
			}
			return false;
		}

		public void Debug(Rect rect, string name)
		{
			AnimationBlender.Mixer.DbgGUI.TableStart(rect);
			for (int i = 0; i < (int)this.channels.Length; i++)
			{
				if (this.channels[i].weight.any)
				{
					AnimationBlender.Mixer.DbgGUI.Label(this.channels[i].@value.name);
				}
			}
			for (int j = 0; j < (int)this.trackers.Length; j++)
			{
				if (this.trackers[j].@value.enabled)
				{
					AnimationBlender.Mixer.DbgGUI.Label(this.trackers[j].@value.state.name);
				}
			}
			if (this.hasResidual)
			{
				AnimationBlender.Mixer.DbgGUI.Label(this.residualState.name);
			}
			AnimationBlender.Mixer.DbgGUI.ColumnNext();
			for (int k = 0; k < (int)this.channels.Length; k++)
			{
				if (this.channels[k].weight.any)
				{
					AnimationBlender.Mixer.DbgGUI.Fract(this.channels[k].weight.normalized);
				}
			}
			for (int l = 0; l < (int)this.trackers.Length; l++)
			{
				if (this.trackers[l].@value.enabled)
				{
					AnimationBlender.Mixer.DbgGUI.Fract(this.trackers[l].weight.normalized);
				}
			}
			if (this.hasResidual)
			{
				AnimationBlender.Mixer.DbgGUI.Fract(this.residualState.weight);
			}
			AnimationBlender.Mixer.DbgGUI.TableEnd();
		}

		private void DisableTracker(ref AnimationBlender.Tracker tracker)
		{
			if (!tracker.wasEnabled)
			{
				return;
			}
			int num = 0;
			bool flag = (bool)num;
			tracker.wasEnabled = (bool)num;
			tracker.state.enabled = flag;
			tracker.state.weight = 0f;
		}

		private bool ManualAnimationsPlaying(bool ClearWhenNone)
		{
			if (!this.playingOneShot)
			{
				return false;
			}
			while (object.ReferenceEquals(this.oneShotAnimation, null) || !this.oneShotAnimation.enabled)
			{
				if (this.queuedAnimations.Count == 0)
				{
					if (ClearWhenNone)
					{
						this.oneShotAnimation = null;
						this.playingOneShot = false;
					}
					return false;
				}
				this.SetOneShotAnimation(this.queuedAnimations.Dequeue());
			}
			return true;
		}

		public bool Play(string animationName)
		{
			return this.PlayOpt(animationName, AnimationBlender.opt<PlayMode>.none, AnimationBlender.opt<float>.none, AnimationBlender.opt<float>.none);
		}

		public bool Play(string animationName, PlayMode playMode)
		{
			return this.PlayOpt(animationName, playMode, AnimationBlender.opt<float>.none, AnimationBlender.opt<float>.none);
		}

		public bool Play(string animationName, PlayMode playMode, float speed)
		{
			return this.PlayOpt(animationName, playMode, speed, AnimationBlender.opt<float>.none);
		}

		public bool Play(string animationName, PlayMode playMode, float speed, float startTime)
		{
			return this.PlayOpt(animationName, playMode, speed, startTime);
		}

		public bool Play(string animationName, PlayMode playMode, float? speed, float startTime)
		{
			return this.PlayOpt(animationName, playMode, AnimationBlender.to_opt<float>(speed), startTime);
		}

		public bool Play(string animationName, float speed)
		{
			return this.PlayOpt(animationName, AnimationBlender.opt<PlayMode>.none, speed, AnimationBlender.opt<float>.none);
		}

		public bool Play(string animationName, float speed, float startTime)
		{
			return this.PlayOpt(animationName, AnimationBlender.opt<PlayMode>.none, speed, startTime);
		}

		public bool Play(string animationName, float? speed, float startTime)
		{
			return this.PlayOpt(animationName, AnimationBlender.opt<PlayMode>.none, AnimationBlender.to_opt<float>(speed), startTime);
		}

		private bool PlayDirect(string animationName, AnimationBlender.opt<PlayMode> playMode)
		{
			PlayMode playMode1;
			if (!playMode.check(out playMode1))
			{
				return this.animation.Play(animationName);
			}
			return this.animation.Play(animationName, playMode1);
		}

		private bool PlayOpt(string animationName, AnimationBlender.opt<PlayMode> playMode, AnimationBlender.opt<float> speed, AnimationBlender.opt<float> startTime)
		{
			float single;
			if (!string.IsNullOrEmpty(animationName))
			{
				AnimationState item = this.animation[animationName];
				AnimationState animationState = item;
				if (item != null)
				{
					if (!playMode.defined)
					{
						this.animation.Stop();
					}
					if (!speed.defined)
					{
						single = 0f;
					}
					else
					{
						single = animationState.speed;
						animationState.speed = speed.@value;
					}
					if (!this.PlayDirect(animationName, playMode))
					{
						if (speed.defined)
						{
							animationState.speed = single;
						}
						return false;
					}
					this.queuedAnimations.Clear();
					this.playingOneShot = true;
					this.oneShotAnimation = animationState;
					if (startTime.defined)
					{
						animationState.time = startTime.@value;
					}
					return true;
				}
			}
			return false;
		}

		public bool PlayQueued(string animationName)
		{
			return this.PlayQueuedOpt(animationName, AnimationBlender.opt<QueueMode>.none, AnimationBlender.opt<PlayMode>.none);
		}

		public bool PlayQueued(string animationName, QueueMode queueMode)
		{
			return this.PlayQueuedOpt(animationName, queueMode, AnimationBlender.opt<PlayMode>.none);
		}

		public bool PlayQueued(string animationName, QueueMode queueMode, PlayMode playMode)
		{
			return this.PlayQueuedOpt(animationName, queueMode, playMode);
		}

		private bool PlayQueuedDirect(string animationName, AnimationBlender.opt<QueueMode> queueMode, AnimationBlender.opt<PlayMode> playMode)
		{
			QueueMode queueMode1;
			PlayMode playMode1;
			if (playMode.check(out playMode1))
			{
				return this.animation.PlayQueued(animationName, queueMode[QueueMode.CompleteOthers], playMode1);
			}
			if (!queueMode.check(out queueMode1))
			{
				return this.animation.PlayQueued(animationName);
			}
			return this.animation.PlayQueued(animationName, queueMode1);
		}

		private bool PlayQueuedOpt(string animationName, AnimationBlender.opt<QueueMode> queueMode, AnimationBlender.opt<PlayMode> playMode)
		{
			if (string.IsNullOrEmpty(animationName) || !this.PlayQueuedDirect(animationName, queueMode, playMode))
			{
				return false;
			}
			this.StopBlendingNow();
			if (queueMode.defined && queueMode.@value == 2)
			{
				this.queuedAnimations.Clear();
				this.playingOneShot = false;
				this.oneShotAnimation = null;
				this.SetOneShotAnimation(animationName);
			}
			else if (!this.playingOneShot)
			{
				this.SetOneShotAnimation(animationName);
			}
			else
			{
				this.queuedAnimations.Enqueue(animationName);
			}
			return true;
		}

		public void SetActive(int channel, bool value)
		{
			this.channels[channel].@value.active = value;
		}

		public void SetActive(string channel, bool value)
		{
			this.SetActive(this.nameToChannel[channel], value);
		}

		public bool SetOneShotAnimation(AnimationState animationState)
		{
			if (animationState == null)
			{
				return false;
			}
			this.oneShotAnimation = animationState;
			int num = 1;
			bool flag = (bool)num;
			this.playingOneShot = (bool)num;
			return flag;
		}

		public bool SetOneShotAnimation(string animationName)
		{
			return (string.IsNullOrEmpty(animationName) ? false : this.SetOneShotAnimation(this.animation[animationName]));
		}

		public void SetSolo(int channel)
		{
			for (int i = 0; i < this.channelCount; i++)
			{
				this.SetActive(i, i == channel);
			}
		}

		public void SetSolo(string channel)
		{
			this.SetSolo(this.nameToChannel[channel]);
		}

		public void SetSolo(int channel, bool muteall)
		{
			if (!muteall)
			{
				this.SetSolo(channel);
			}
			else
			{
				for (int i = 0; i < this.channelCount; i++)
				{
					this.SetActive(i, false);
				}
			}
		}

		public void SetSolo(string channel, bool muteall)
		{
			this.SetSolo(this.nameToChannel[channel], muteall);
		}

		private void StopBlendingNow()
		{
			for (int i = 0; i < this.trackerCount; i++)
			{
				this.trackers[i].@value.state.enabled = false;
			}
		}

		public void Update(float blend, float dt)
		{
			float single;
			if (!this.playingOneShot)
			{
				this.animationBlocking = false;
				if (this.oneShotBlendInTime < this.oneShotBlendIn.end)
				{
					float single1 = this.oneShotBlendInTime + dt;
					float single2 = single1;
					this.oneShotBlendInTime = single1;
					if (single2 > this.oneShotBlendIn.end)
					{
						this.oneShotBlendInTime = this.oneShotBlendIn.end;
					}
				}
			}
			else if (this.ManualAnimationsPlaying(true))
			{
				this.oneShotBlendInTime = this.oneShotBlendIn.start;
				if (!this.hasOneShotBlendIn)
				{
					blend = 0f;
				}
				this.animationBlocking = true;
			}
			else
			{
				this.oneShotBlendInTime = this.oneShotBlendIn.start + dt;
				if (this.oneShotBlendInTime > this.oneShotBlendIn.end)
				{
					this.oneShotBlendInTime = this.oneShotBlendIn.end;
				}
				this.animationBlocking = false;
				for (int i = 0; i < this.trackerCount; i++)
				{
					this.trackers[i].@value.wasEnabled = false;
				}
			}
			if (this.hasOneShotBlendIn)
			{
				blend = blend * this.oneShotBlendIn.SampleTime(this.oneShotBlendInTime);
			}
			if (blend > 1f)
			{
				blend = 1f;
			}
			else if (blend < 0f)
			{
				blend = 0f;
			}
			if (this.UpdateBlender(ref this.blender, dt, blend, out single))
			{
				if (this.hasResidual)
				{
					bool flag = !this.residualState.enabled;
					this.residualState.enabled = true;
					this.residualState.weight = single;
					if (flag)
					{
						this.residualState.time = this.residualField.startFrame;
						this.residualState.speed = this.residualField.playbackRate;
					}
				}
			}
			else if (this.hasResidual && this.residualState.enabled)
			{
				this.residualState.enabled = false;
				this.residualState.weight = 0f;
			}
		}

		private bool UpdateBlender(ref AnimationBlender.TrackerBlender blender, float dt, float externalBlend, out float residualBlend)
		{
			for (int i = 0; i < this.trackerCount; i++)
			{
				this.UpdateTracker(ref this.trackers[blender.trackers[i]], dt);
			}
			AnimationBlender.WeightOf<AnimationBlender.Tracker>(this.trackers, blender.trackers, out blender.trackerWeight);
			for (int j = blender.trackerWeight.count; j < blender.trackerCount; j++)
			{
				this.DisableTracker(ref this.trackers[blender.trackers[j]].@value);
			}
			float single = 0f;
			for (int k = 0; k < blender.trackerWeight.count; k++)
			{
				single = single + this.BindTracker(ref this.trackers[blender.trackers[k]], externalBlend);
			}
			float single1 = (1f - single) * externalBlend;
			float single2 = single1;
			residualBlend = single1;
			return single2 > 0f;
		}

		private void UpdateChannel(ref AnimationBlender.Weighted<AnimationBlender.Channel> channel, float dt)
		{
			bool flag;
			flag = (!channel.@value.field.blockedByAnimation || !this.animationBlocking ? false : channel.@value.active);
			bool flag1 = flag;
			if (flag)
			{
				channel.@value.active = false;
			}
			channel.weight.raw = channel.@value.Update(dt);
			if (flag1)
			{
				channel.@value.active = true;
			}
		}

		private void UpdateTracker(ref AnimationBlender.Weighted<AnimationBlender.Tracker> tracker, float dt)
		{
			for (int i = 0; i < tracker.@value.channelCount; i++)
			{
				this.UpdateChannel(ref this.channels[tracker.@value.channels[i]], dt);
			}
			bool flag = AnimationBlender.WeightOf<AnimationBlender.Channel>(this.channels, tracker.@value.channels, out tracker.@value.channelWeight);
			bool flag1 = flag;
			tracker.@value.enabled = flag;
			if (flag1)
			{
				tracker.@value.startTime = this.channels[tracker.@value.channels[tracker.@value.channelWeight.winner]].@value.startTime;
				float single = 0f;
				float single1 = 0f;
				for (int j = 0; j < tracker.@value.channelWeight.count; j++)
				{
					float single2 = this.channels[tracker.@value.channels[j]].@value.playbackRate;
					float single3 = this.channels[tracker.@value.channels[j]].weight.normalized;
					float single4 = single3;
					single = single + single2 * single3;
					single1 = single1 + this.channels[tracker.@value.channels[j]].@value.maxBlend * single4;
				}
				tracker.@value.playbackRate = single;
				tracker.@value.blendFraction = single1;
			}
			tracker.weight.raw = tracker.@value.channelWeight.sum.raw;
		}

		private static class DbgGUI
		{
			private readonly static GUILayoutOption[] Cell;

			private readonly static GUILayoutOption[] FirstColumn;

			private readonly static GUILayoutOption[] OtherColumn;

			static DbgGUI()
			{
				AnimationBlender.Mixer.DbgGUI.Cell = new GUILayoutOption[] { GUILayout.Height(18f) };
				AnimationBlender.Mixer.DbgGUI.FirstColumn = new GUILayoutOption[] { GUILayout.Width(128f) };
				AnimationBlender.Mixer.DbgGUI.OtherColumn = new GUILayoutOption[] { GUILayout.ExpandWidth(true) };
			}

			public static void ColumnNext()
			{
				GUILayout.EndVertical();
				GUILayout.BeginVertical(AnimationBlender.Mixer.DbgGUI.OtherColumn);
			}

			public static void Fract(float frac)
			{
				GUILayout.HorizontalSlider(frac, 0f, 1f, AnimationBlender.Mixer.DbgGUI.Cell);
			}

			public static void Label(string str)
			{
				GUILayout.Label(str, AnimationBlender.Mixer.DbgGUI.Cell);
			}

			public static void TableEnd()
			{
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}

			public static void TableStart(Rect rect)
			{
				GUILayout.BeginArea(rect);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.BeginVertical(AnimationBlender.Mixer.DbgGUI.FirstColumn);
			}
		}
	}

	public struct MixerConfig
	{
		[NonSerialized]
		public readonly Animation animation;

		[NonSerialized]
		public readonly AnimationBlender.ResidualField residual;

		[NonSerialized]
		public readonly AnimationBlender.ChannelConfig[] channels;

		public MixerConfig(Animation animation, AnimationBlender.ResidualField residual, params AnimationBlender.ChannelConfig[] channels)
		{
			this.animation = animation;
			this.residual = residual;
			this.channels = channels;
		}
	}

	private struct opt<T>
	{
		[NonSerialized]
		public readonly T @value;

		[NonSerialized]
		public readonly bool defined;

		public readonly static AnimationBlender.opt<T> none;

		public T this[T fallback]
		{
			get
			{
				return (!this.defined ? fallback : this.@value);
			}
		}

		static opt()
		{
			AnimationBlender.opt<T>.none = new AnimationBlender.opt<T>();
		}

		private opt(T value, bool defined)
		{
			this.@value = value;
			this.defined = defined;
		}

		public bool check(out T value)
		{
			value = this.@value;
			return this.defined;
		}

		public static implicit operator opt<T>(T value)
		{
			return new AnimationBlender.opt<T>(value, true);
		}
	}

	[Serializable]
	public sealed class ResidualField : AnimationBlender.Field
	{
		[SerializeField]
		public AnimationCurve introCurve;

		[SerializeField]
		public AnimationBlendMode residualBlend;

		[SerializeField]
		public AnimationBlendMode channelBlend;

		[SerializeField]
		public int animLayer;

		[SerializeField]
		public bool changeAnimLayer;

		public AnimationBlender.CurveInfo introCurveInfo
		{
			get
			{
				return new AnimationBlender.CurveInfo(this.introCurve);
			}
		}

		public ResidualField()
		{
			this.clipName = string.Empty;
			this.playbackRate = 1f;
			this.changeAnimLayer = false;
			int num = 0;
			AnimationBlendMode animationBlendMode = (AnimationBlendMode)num;
			this.residualBlend = (AnimationBlendMode)num;
			this.channelBlend = animationBlendMode;
		}
	}

	private struct State
	{
		[NonSerialized]
		public bool active;

		[NonSerialized]
		public float time;

		[NonSerialized]
		public float percent;

		[NonSerialized]
		public float delay;

		[NonSerialized]
		public float @value;
	}

	private struct Tracker
	{
		[NonSerialized]
		public AnimationClip clip;

		[NonSerialized]
		public AnimationState state;

		[NonSerialized]
		public int[] channels;

		[NonSerialized]
		public int channelCount;

		[NonSerialized]
		public AnimationBlender.WeightResult channelWeight;

		[NonSerialized]
		public float playbackRate;

		[NonSerialized]
		public float blendFraction;

		[NonSerialized]
		public float startTime;

		[NonSerialized]
		public bool enabled;

		[NonSerialized]
		public bool wasEnabled;
	}

	private struct TrackerBlender
	{
		[NonSerialized]
		public int[] trackers;

		[NonSerialized]
		public int trackerCount;

		[NonSerialized]
		public AnimationBlender.WeightResult trackerWeight;

		public TrackerBlender(int count)
		{
			this.trackers = new int[count];
			this.trackerCount = count;
			this.trackerWeight = new AnimationBlender.WeightResult();
			for (int i = 0; i < count; i++)
			{
				this.trackers[i] = i;
			}
		}
	}

	private struct Weighted<T>
	{
		[NonSerialized]
		public AnimationBlender.WeightUnit weight;

		[NonSerialized]
		public T @value;
	}

	private struct WeightResult
	{
		[NonSerialized]
		public int count;

		[NonSerialized]
		public int winner;

		public AnimationBlender.WeightUnit sum;
	}

	private struct WeightUnit
	{
		[NonSerialized]
		public float raw;

		[NonSerialized]
		public float scaled;

		[NonSerialized]
		public float normalized;

		public bool any
		{
			get
			{
				return this.raw > 0f;
			}
		}

		public float SetNormalizedRecip(float recip)
		{
			float single = this.scaled * recip;
			float single1 = single;
			this.normalized = single;
			return single1;
		}

		public float SetScaledRecip(float recip)
		{
			float single = this.raw * recip;
			float single1 = single;
			this.scaled = single;
			float single2 = single1;
			single1 = single2;
			this.normalized = single2;
			return single1;
		}
	}
}