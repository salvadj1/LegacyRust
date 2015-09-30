using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Daikon Forge/Tweens/Group")]
[Serializable]
public class dfTweenGroup : dfTweenPlayableBase
{
	[SerializeField]
	protected string groupName = string.Empty;

	public List<dfTweenPlayableBase> Tweens = new List<dfTweenPlayableBase>();

	public dfTweenGroup.TweenGroupMode Mode;

	private TweenNotification TweenStarted;

	private TweenNotification TweenStopped;

	private TweenNotification TweenReset;

	private TweenNotification TweenCompleted;

	public override bool IsPlaying
	{
		get
		{
			for (int i = 0; i < this.Tweens.Count; i++)
			{
				if (!(this.Tweens[i] == null) && this.Tweens[i].enabled)
				{
					if (this.Tweens[i].IsPlaying)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public override string TweenName
	{
		get
		{
			return this.groupName;
		}
		set
		{
			this.groupName = value;
		}
	}

	public dfTweenGroup()
	{
	}

	public void DisableTween(string TweenName)
	{
		for (int i = 0; i < this.Tweens.Count; i++)
		{
			if (this.Tweens[i] != null)
			{
				if (this.Tweens[i].name == TweenName)
				{
					this.Tweens[i].enabled = false;
					break;
				}
			}
		}
	}

	public void EnableTween(string TweenName)
	{
		for (int i = 0; i < this.Tweens.Count; i++)
		{
			if (this.Tweens[i] != null)
			{
				if (this.Tweens[i].TweenName == TweenName)
				{
					this.Tweens[i].enabled = true;
					break;
				}
			}
		}
	}

	protected internal void onCompleted()
	{
		base.SendMessage("TweenCompleted", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenCompleted != null)
		{
			this.TweenCompleted();
		}
	}

	protected internal void onReset()
	{
		base.SendMessage("TweenReset", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenReset != null)
		{
			this.TweenReset();
		}
	}

	protected internal void onStarted()
	{
		base.SendMessage("TweenStarted", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenStarted != null)
		{
			this.TweenStarted();
		}
	}

	protected internal void onStopped()
	{
		base.SendMessage("TweenStopped", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenStopped != null)
		{
			this.TweenStopped();
		}
	}

	public override void Play()
	{
		if (this.IsPlaying)
		{
			this.Stop();
		}
		this.onStarted();
		if (this.Mode != dfTweenGroup.TweenGroupMode.Concurrent)
		{
			base.StartCoroutine(this.runSequence());
		}
		else
		{
			base.StartCoroutine(this.runConcurrent());
		}
	}

	public override void Reset()
	{
		if (!this.IsPlaying)
		{
			return;
		}
		base.StopAllCoroutines();
		for (int i = 0; i < this.Tweens.Count; i++)
		{
			if (this.Tweens[i] != null)
			{
				this.Tweens[i].Reset();
			}
		}
		this.onReset();
	}

	[DebuggerHidden]
	[HideInInspector]
	private IEnumerator runConcurrent()
	{
		dfTweenGroup.<runConcurrent>c__Iterator47 variable = null;
		return variable;
	}

	[DebuggerHidden]
	[HideInInspector]
	private IEnumerator runSequence()
	{
		dfTweenGroup.<runSequence>c__Iterator46 variable = null;
		return variable;
	}

	public override void Stop()
	{
		if (!this.IsPlaying)
		{
			return;
		}
		base.StopAllCoroutines();
		for (int i = 0; i < this.Tweens.Count; i++)
		{
			if (this.Tweens[i] != null)
			{
				this.Tweens[i].Stop();
			}
		}
		this.onStopped();
	}

	private void Update()
	{
	}

	public event TweenNotification TweenCompleted
	{
		add
		{
			this.TweenCompleted += value;
		}
		remove
		{
			this.TweenCompleted -= value;
		}
	}

	public event TweenNotification TweenReset
	{
		add
		{
			this.TweenReset += value;
		}
		remove
		{
			this.TweenReset -= value;
		}
	}

	public event TweenNotification TweenStarted
	{
		add
		{
			this.TweenStarted += value;
		}
		remove
		{
			this.TweenStarted -= value;
		}
	}

	public event TweenNotification TweenStopped
	{
		add
		{
			this.TweenStopped += value;
		}
		remove
		{
			this.TweenStopped -= value;
		}
	}

	public enum TweenGroupMode
	{
		Concurrent,
		Sequence
	}
}