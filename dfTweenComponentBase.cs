using System;
using UnityEngine;

[Serializable]
public abstract class dfTweenComponentBase : dfTweenPlayableBase
{
	[SerializeField]
	protected string tweenName = string.Empty;

	[SerializeField]
	protected dfComponentMemberInfo target;

	[SerializeField]
	protected dfEasingType easingType;

	[SerializeField]
	protected UnityEngine.AnimationCurve animCurve = new UnityEngine.AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f) });

	[SerializeField]
	protected float length = 1f;

	[SerializeField]
	protected bool syncStartWhenRun;

	[SerializeField]
	protected bool startValueIsOffset;

	[SerializeField]
	protected bool syncEndWhenRun;

	[SerializeField]
	protected bool endValueIsOffset;

	[SerializeField]
	protected dfTweenLoopType loopType;

	[SerializeField]
	protected bool autoRun;

	[SerializeField]
	protected bool skipToEndOnStop;

	protected bool isRunning;

	protected bool isPaused;

	protected dfEasingFunctions.EasingFunction easingFunction;

	protected dfObservableProperty boundProperty;

	protected bool wasAutoStarted;

	public UnityEngine.AnimationCurve AnimationCurve
	{
		get
		{
			return this.animCurve;
		}
		set
		{
			this.animCurve = value;
		}
	}

	public bool AutoRun
	{
		get
		{
			return this.autoRun;
		}
		set
		{
			this.autoRun = value;
		}
	}

	public bool EndValueIsOffset
	{
		get
		{
			return this.endValueIsOffset;
		}
		set
		{
			this.endValueIsOffset = value;
		}
	}

	public dfEasingType Function
	{
		get
		{
			return this.easingType;
		}
		set
		{
			this.easingType = value;
			if (this.isRunning)
			{
				this.Stop();
				this.Play();
			}
		}
	}

	public bool IsPaused
	{
		get
		{
			return this.isPaused;
		}
		set
		{
			if (value != this.isPaused)
			{
				if (value && !this.isRunning)
				{
					this.isPaused = false;
					return;
				}
				this.isPaused = value;
				if (!value)
				{
					this.onResumed();
				}
				else
				{
					this.onPaused();
				}
			}
		}
	}

	public override bool IsPlaying
	{
		get
		{
			return (!base.enabled ? false : this.isRunning);
		}
	}

	public float Length
	{
		get
		{
			return this.length;
		}
		set
		{
			this.length = Mathf.Max(0f, value);
		}
	}

	public dfTweenLoopType LoopType
	{
		get
		{
			return this.loopType;
		}
		set
		{
			this.loopType = value;
			if (this.isRunning)
			{
				this.Stop();
				this.Play();
			}
		}
	}

	public bool StartValueIsOffset
	{
		get
		{
			return this.startValueIsOffset;
		}
		set
		{
			this.startValueIsOffset = value;
		}
	}

	public bool SyncEndValueWhenRun
	{
		get
		{
			return this.syncEndWhenRun;
		}
		set
		{
			this.syncEndWhenRun = value;
		}
	}

	public bool SyncStartValueWhenRun
	{
		get
		{
			return this.syncStartWhenRun;
		}
		set
		{
			this.syncStartWhenRun = value;
		}
	}

	public dfComponentMemberInfo Target
	{
		get
		{
			return this.target;
		}
		set
		{
			this.target = value;
		}
	}

	public override string TweenName
	{
		get
		{
			if (this.tweenName == null)
			{
				this.tweenName = base.ToString();
			}
			return this.tweenName;
		}
		set
		{
			this.tweenName = value;
		}
	}

	protected dfTweenComponentBase()
	{
	}

	public void LateUpdate()
	{
		if (this.autoRun && !this.wasAutoStarted)
		{
			this.wasAutoStarted = true;
			this.Play();
		}
	}

	protected internal abstract void onCompleted();

	protected internal abstract void onPaused();

	protected internal abstract void onReset();

	protected internal abstract void onResumed();

	protected internal abstract void onStarted();

	protected internal abstract void onStopped();

	public override string ToString()
	{
		if (this.Target == null || !this.Target.IsValid)
		{
			return this.TweenName;
		}
		string component = this.target.Component.name;
		return string.Format("{0} ({1}.{2})", this.TweenName, component, this.target.MemberName);
	}
}