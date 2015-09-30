using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(uLinkNetworkView))]
public class Interpolator : IDLocal, IIDLocalInterpolator
{
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
	private bool _running;

	[NonSerialized]
	private bool _destroying;

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

	IDLocal IIDLocalInterpolator.self
	{
		get
		{
			return this;
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
					this._running = !Interpolator.Interpolators.SetDisabled(this);
				}
				else
				{
					this._running = Interpolator.Interpolators.SetEnabled(this);
				}
			}
		}
	}

	public Interpolator()
	{
	}

	// privatescope
	internal virtual IDMain IIDLocalInterpolator.get_idMain()
	{
		return base.idMain;
	}

	protected void OnDestroy()
	{
		this._destroying = true;
		if (this._running)
		{
			Interpolator.Interpolators.SetDisabled(this);
			this._running = false;
		}
	}

	public virtual void SetGoals(Vector3 pos, Quaternion rot, double timestamp)
	{
		if (!this.initialized)
		{
			base.transform.position = pos;
			if (!(this.idMain is Character))
			{
				base.transform.rotation = rot;
			}
			else
			{
				Angle2 angle2 = Angle2.LookDirection(rot * Vector3.forward);
				angle2.pitch = Mathf.DeltaAngle(0f, angle2.pitch);
				((Character)this.idMain).eyesAngles = angle2;
			}
			this.initialized = true;
		}
		this.targetPos = pos;
		this.targetRot = rot;
		this.fromPos = base.transform.position;
		if (!(this.idMain is Character))
		{
			this.fromRot = base.transform.rotation;
		}
		else
		{
			this.fromRot = ((Character)this.idMain).eyesAngles.quat;
		}
		this.lerpStartTime = Time.realtimeSinceStartup;
	}

	protected virtual void Syncronize()
	{
		float single = (Time.realtimeSinceStartup - this.lerpStartTime) / Interpolation.@struct.totalDelaySecondsF;
		Vector3 vector3 = Vector3.Lerp(this.fromPos, this.targetPos, single);
		Quaternion quaternion = Quaternion.Slerp(this.fromRot, this.targetRot, single);
		if (!(this.idMain is Character))
		{
			base.transform.position = vector3;
			base.transform.rotation = quaternion;
		}
		else
		{
			Character character = (Character)this.idMain;
			character.origin = vector3;
			Angle2 angle2 = Angle2.LookDirection(quaternion * Vector3.forward);
			angle2.pitch = Mathf.DeltaAngle(0f, angle2.pitch);
			character.eyesAngles = angle2;
		}
	}

	internal static void SyncronizeAll()
	{
		Interpolator.Interpolators.UpdateAll();
	}

	private static class Interpolators
	{
		private readonly static HashSet<Interpolator> hashset1;

		private readonly static HashSet<Interpolator> hashset2;

		private static bool swapped;

		private static bool iterating;

		private static bool caughtIterating;

		static Interpolators()
		{
			Interpolator.Interpolators.hashset1 = new HashSet<Interpolator>();
			Interpolator.Interpolators.hashset2 = new HashSet<Interpolator>();
		}

		public static bool SetDisabled(Interpolator interpolator)
		{
			HashSet<Interpolator> interpolators;
			HashSet<Interpolator> interpolators1;
			if (!Interpolator.Interpolators.iterating)
			{
				return ((!Interpolator.Interpolators.swapped ? Interpolator.Interpolators.hashset1 : Interpolator.Interpolators.hashset2)).Remove(interpolator);
			}
			if (Interpolator.Interpolators.caughtIterating)
			{
				return ((!Interpolator.Interpolators.swapped ? Interpolator.Interpolators.hashset2 : Interpolator.Interpolators.hashset1)).Remove(interpolator);
			}
			if (!Interpolator.Interpolators.swapped)
			{
				interpolators = Interpolator.Interpolators.hashset1;
				interpolators1 = Interpolator.Interpolators.hashset2;
			}
			else
			{
				interpolators = Interpolator.Interpolators.hashset2;
				interpolators1 = Interpolator.Interpolators.hashset1;
			}
			if (!interpolators.Contains(interpolator))
			{
				return false;
			}
			Interpolator.Interpolators.caughtIterating = true;
			interpolators1.UnionWith(interpolators);
			return interpolators1.Remove(interpolator);
		}

		public static bool SetEnabled(Interpolator interpolator)
		{
			HashSet<Interpolator> interpolators;
			HashSet<Interpolator> interpolators1;
			if (!Interpolator.Interpolators.iterating)
			{
				return ((!Interpolator.Interpolators.swapped ? Interpolator.Interpolators.hashset1 : Interpolator.Interpolators.hashset2)).Add(interpolator);
			}
			if (Interpolator.Interpolators.caughtIterating)
			{
				return ((!Interpolator.Interpolators.swapped ? Interpolator.Interpolators.hashset2 : Interpolator.Interpolators.hashset1)).Add(interpolator);
			}
			if (!Interpolator.Interpolators.swapped)
			{
				interpolators = Interpolator.Interpolators.hashset1;
				interpolators1 = Interpolator.Interpolators.hashset2;
			}
			else
			{
				interpolators = Interpolator.Interpolators.hashset2;
				interpolators1 = Interpolator.Interpolators.hashset1;
			}
			if (interpolators.Contains(interpolator))
			{
				return false;
			}
			Interpolator.Interpolators.caughtIterating = true;
			interpolators1.UnionWith(interpolators);
			return interpolators1.Add(interpolator);
		}

		public static void UpdateAll()
		{
			HashSet<Interpolator> interpolators;
			if (Interpolator.Interpolators.iterating)
			{
				return;
			}
			interpolators = (!Interpolator.Interpolators.swapped ? Interpolator.Interpolators.hashset1 : Interpolator.Interpolators.hashset2);
			try
			{
				Interpolator.Interpolators.iterating = true;
				foreach (Interpolator interpolator in interpolators)
				{
					try
					{
						interpolator.Syncronize();
					}
					catch (Exception exception)
					{
						Debug.LogError(exception);
					}
				}
			}
			finally
			{
				if (Interpolator.Interpolators.caughtIterating)
				{
					Interpolator.Interpolators.swapped = !Interpolator.Interpolators.swapped;
					if (!Interpolator.Interpolators.swapped)
					{
						Interpolator.Interpolators.hashset2.Clear();
					}
					else
					{
						Interpolator.Interpolators.hashset1.Clear();
					}
				}
				Interpolator.Interpolators.iterating = false;
			}
		}
	}
}