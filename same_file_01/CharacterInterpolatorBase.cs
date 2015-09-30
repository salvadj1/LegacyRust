using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterInterpolatorBase : IDLocalCharacterAddon, IIDLocalInterpolator
{
	protected const int kDefaultBufferCapacity = 32;

	private const IDLocalCharacterAddon.AddonFlags kRequiredAddonFlags = IDLocalCharacterAddon.AddonFlags.FireOnAddonAwake;

	[NonSerialized]
	private Vector3 targetPos;

	[NonSerialized]
	private Vector3 fromPos;

	[NonSerialized]
	private Quaternion targetRot;

	[NonSerialized]
	private Quaternion fromRot;

	[NonSerialized]
	private float lerpStartTime;

	[NonSerialized]
	private bool initialized;

	[NonSerialized]
	protected int _bufferCapacity = 32;

	[NonSerialized]
	protected bool extrapolate;

	[NonSerialized]
	protected float allowableTimeSpan = 0.1f;

	[NonSerialized]
	protected int len;

	[NonSerialized]
	private bool _running;

	[NonSerialized]
	private bool _destroying;

	protected abstract double __newestTimeStamp
	{
		get;
	}

	protected abstract double __oldestTimeStamp
	{
		get;
	}

	protected abstract double __storedDuration
	{
		get;
	}

	[Obsolete("Use .running for interpolators", true)]
	public new bool enabled
	{
		get
		{
			return this.running;
		}
		set
		{
			this.running = value;
		}
	}

	IDMain IIDLocalInterpolator.idMain
	{
		get
		{
			return this.idMain;
		}
	}

	IDLocal IIDLocalInterpolator.self
	{
		get
		{
			return this;
		}
	}

	public double newestTimeStamp
	{
		get
		{
			return this.__newestTimeStamp;
		}
	}

	public double oldestTimeStamp
	{
		get
		{
			return this.__oldestTimeStamp;
		}
	}

	public bool running
	{
		get
		{
			return this._running;
		}
		set
		{
			if (this._destroying)
			{
				value = false;
			}
			if (this._running != value)
			{
				if (!value)
				{
					this._running = !CharacterInterpolatorBase.Interpolators.SetDisabled(this);
				}
				else
				{
					this._running = CharacterInterpolatorBase.Interpolators.SetEnabled(this);
				}
			}
		}
	}

	public double storedDuration
	{
		get
		{
			return this.__storedDuration;
		}
	}

	internal CharacterInterpolatorBase(IDLocalCharacterAddon.AddonFlags addonFlags) : base((IDLocalCharacterAddon.AddonFlags)((byte)(addonFlags | IDLocalCharacterAddon.AddonFlags.FireOnAddonAwake)))
	{
	}

	protected abstract void __Clear();

	public void Clear()
	{
		this.__Clear();
	}

	protected override void OnAddonAwake()
	{
		CharacterInterpolatorTrait trait = base.idMain.GetTrait<CharacterInterpolatorTrait>();
		if (trait)
		{
			if (trait.bufferCapacity > 0)
			{
				this._bufferCapacity = trait.bufferCapacity;
			}
			this.extrapolate = trait.allowExtrapolation;
			this.allowableTimeSpan = trait.allowableTimeSpan;
		}
	}

	protected void OnDestroy()
	{
		this._destroying = true;
		if (this._running)
		{
			CharacterInterpolatorBase.Interpolators.SetDisabled(this);
			this._running = false;
		}
	}

	public virtual void SetGoals(Vector3 pos, Quaternion rot, double timestamp)
	{
		if (!this.initialized)
		{
			base.transform.position = pos;
			if (!(base.idMain is Character))
			{
				base.transform.rotation = rot;
			}
			else
			{
				Angle2 angle2 = Angle2.LookDirection(rot * Vector3.forward);
				angle2.pitch = Mathf.DeltaAngle(0f, angle2.pitch);
				base.idMain.eyesAngles = angle2;
			}
			this.initialized = true;
		}
		this.targetPos = pos;
		this.targetRot = rot;
		this.fromPos = base.transform.position;
		if (!(base.idMain is Character))
		{
			this.fromRot = base.transform.rotation;
		}
		else
		{
			this.fromRot = base.idMain.eyesAngles.quat;
		}
		this.lerpStartTime = Time.realtimeSinceStartup;
	}

	protected virtual void Syncronize()
	{
		float single = (Time.realtimeSinceStartup - this.lerpStartTime) / Interpolation.@struct.totalDelaySecondsF;
		Vector3 vector3 = Vector3.Lerp(this.fromPos, this.targetPos, single);
		Quaternion quaternion = Quaternion.Slerp(this.fromRot, this.targetRot, single);
		if (!(base.idMain is Character))
		{
			base.transform.position = vector3;
			base.transform.rotation = quaternion;
		}
		else
		{
			Character character = base.idMain;
			character.origin = vector3;
			Angle2 angle2 = Angle2.LookDirection(quaternion * Vector3.forward);
			angle2.pitch = Mathf.DeltaAngle(0f, angle2.pitch);
			character.eyesAngles = angle2;
		}
	}

	internal static void SyncronizeAll()
	{
		CharacterInterpolatorBase.Interpolators.UpdateAll();
	}

	private static class Interpolators
	{
		private readonly static HashSet<CharacterInterpolatorBase> hashset1;

		private readonly static HashSet<CharacterInterpolatorBase> hashset2;

		private static bool swapped;

		private static bool iterating;

		private static bool caughtIterating;

		static Interpolators()
		{
			CharacterInterpolatorBase.Interpolators.hashset1 = new HashSet<CharacterInterpolatorBase>();
			CharacterInterpolatorBase.Interpolators.hashset2 = new HashSet<CharacterInterpolatorBase>();
		}

		public static bool SetDisabled(CharacterInterpolatorBase interpolator)
		{
			HashSet<CharacterInterpolatorBase> characterInterpolatorBases;
			HashSet<CharacterInterpolatorBase> characterInterpolatorBases1;
			if (!CharacterInterpolatorBase.Interpolators.iterating)
			{
				return ((!CharacterInterpolatorBase.Interpolators.swapped ? CharacterInterpolatorBase.Interpolators.hashset1 : CharacterInterpolatorBase.Interpolators.hashset2)).Remove(interpolator);
			}
			if (CharacterInterpolatorBase.Interpolators.caughtIterating)
			{
				return ((!CharacterInterpolatorBase.Interpolators.swapped ? CharacterInterpolatorBase.Interpolators.hashset2 : CharacterInterpolatorBase.Interpolators.hashset1)).Remove(interpolator);
			}
			if (!CharacterInterpolatorBase.Interpolators.swapped)
			{
				characterInterpolatorBases = CharacterInterpolatorBase.Interpolators.hashset1;
				characterInterpolatorBases1 = CharacterInterpolatorBase.Interpolators.hashset2;
			}
			else
			{
				characterInterpolatorBases = CharacterInterpolatorBase.Interpolators.hashset2;
				characterInterpolatorBases1 = CharacterInterpolatorBase.Interpolators.hashset1;
			}
			if (!characterInterpolatorBases.Contains(interpolator))
			{
				return false;
			}
			CharacterInterpolatorBase.Interpolators.caughtIterating = true;
			characterInterpolatorBases1.UnionWith(characterInterpolatorBases);
			return characterInterpolatorBases1.Remove(interpolator);
		}

		public static bool SetEnabled(CharacterInterpolatorBase interpolator)
		{
			HashSet<CharacterInterpolatorBase> characterInterpolatorBases;
			HashSet<CharacterInterpolatorBase> characterInterpolatorBases1;
			if (!CharacterInterpolatorBase.Interpolators.iterating)
			{
				return ((!CharacterInterpolatorBase.Interpolators.swapped ? CharacterInterpolatorBase.Interpolators.hashset1 : CharacterInterpolatorBase.Interpolators.hashset2)).Add(interpolator);
			}
			if (CharacterInterpolatorBase.Interpolators.caughtIterating)
			{
				return ((!CharacterInterpolatorBase.Interpolators.swapped ? CharacterInterpolatorBase.Interpolators.hashset2 : CharacterInterpolatorBase.Interpolators.hashset1)).Add(interpolator);
			}
			if (!CharacterInterpolatorBase.Interpolators.swapped)
			{
				characterInterpolatorBases = CharacterInterpolatorBase.Interpolators.hashset1;
				characterInterpolatorBases1 = CharacterInterpolatorBase.Interpolators.hashset2;
			}
			else
			{
				characterInterpolatorBases = CharacterInterpolatorBase.Interpolators.hashset2;
				characterInterpolatorBases1 = CharacterInterpolatorBase.Interpolators.hashset1;
			}
			if (characterInterpolatorBases.Contains(interpolator))
			{
				return false;
			}
			CharacterInterpolatorBase.Interpolators.caughtIterating = true;
			characterInterpolatorBases1.UnionWith(characterInterpolatorBases);
			return characterInterpolatorBases1.Add(interpolator);
		}

		public static void UpdateAll()
		{
			HashSet<CharacterInterpolatorBase> characterInterpolatorBases;
			if (CharacterInterpolatorBase.Interpolators.iterating)
			{
				return;
			}
			characterInterpolatorBases = (!CharacterInterpolatorBase.Interpolators.swapped ? CharacterInterpolatorBase.Interpolators.hashset1 : CharacterInterpolatorBase.Interpolators.hashset2);
			try
			{
				CharacterInterpolatorBase.Interpolators.iterating = true;
				foreach (CharacterInterpolatorBase characterInterpolatorBase in characterInterpolatorBases)
				{
					try
					{
						characterInterpolatorBase.Syncronize();
					}
					catch (Exception exception)
					{
						Debug.LogError(exception);
					}
				}
			}
			finally
			{
				if (CharacterInterpolatorBase.Interpolators.caughtIterating)
				{
					CharacterInterpolatorBase.Interpolators.swapped = !CharacterInterpolatorBase.Interpolators.swapped;
					if (!CharacterInterpolatorBase.Interpolators.swapped)
					{
						CharacterInterpolatorBase.Interpolators.hashset2.Clear();
					}
					else
					{
						CharacterInterpolatorBase.Interpolators.hashset1.Clear();
					}
				}
				CharacterInterpolatorBase.Interpolators.iterating = false;
			}
		}
	}
}