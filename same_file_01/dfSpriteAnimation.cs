using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

[AddComponentMenu("Daikon Forge/Tweens/Sprite Animator")]
[RequireComponent(typeof(BoxCollider))]
[Serializable]
public class dfSpriteAnimation : dfTweenPlayableBase
{
	[SerializeField]
	private string animationName = "ANIMATION";

	[SerializeField]
	private dfAnimationClip clip;

	[SerializeField]
	private dfComponentMemberInfo memberInfo = new dfComponentMemberInfo();

	[SerializeField]
	private dfTweenLoopType loopType = dfTweenLoopType.Loop;

	[SerializeField]
	private float length = 1f;

	[SerializeField]
	private bool autoStart;

	[SerializeField]
	private bool skipToEndOnStop;

	[SerializeField]
	private dfSpriteAnimation.PlayDirection playDirection;

	private bool autoRunStarted;

	private bool isRunning;

	private bool isPaused;

	private dfObservableProperty target;

	private TweenNotification AnimationStarted;

	private TweenNotification AnimationStopped;

	private TweenNotification AnimationPaused;

	private TweenNotification AnimationResumed;

	private TweenNotification AnimationReset;

	private TweenNotification AnimationCompleted;

	public bool AutoRun
	{
		get
		{
			return this.autoStart;
		}
		set
		{
			this.autoStart = value;
		}
	}

	public dfAnimationClip Clip
	{
		get
		{
			return this.clip;
		}
		set
		{
			this.clip = value;
		}
	}

	public dfSpriteAnimation.PlayDirection Direction
	{
		get
		{
			return this.playDirection;
		}
		set
		{
			this.playDirection = value;
			if (this.IsPlaying)
			{
				this.Play();
			}
		}
	}

	public bool IsPaused
	{
		get
		{
			return (!this.isRunning ? false : this.isPaused);
		}
		set
		{
			if (value != this.IsPaused)
			{
				if (!value)
				{
					this.Resume();
				}
				else
				{
					this.Pause();
				}
			}
		}
	}

	public override bool IsPlaying
	{
		get
		{
			return this.isRunning;
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
			this.length = Mathf.Max(value, 0.03f);
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
		}
	}

	public dfComponentMemberInfo Target
	{
		get
		{
			return this.memberInfo;
		}
		set
		{
			this.memberInfo = value;
		}
	}

	public override string TweenName
	{
		get
		{
			return this.animationName;
		}
		set
		{
			this.animationName = value;
		}
	}

	public dfSpriteAnimation()
	{
	}

	public void Awake()
	{
	}

	[DebuggerHidden]
	private IEnumerator Execute()
	{
		dfSpriteAnimation.<Execute>c__Iterator44 variable = null;
		return variable;
	}

	private string getPath(Transform obj)
	{
		StringBuilder stringBuilder = new StringBuilder();
		while (obj != null)
		{
			if (stringBuilder.Length <= 0)
			{
				stringBuilder.Append(obj.name);
			}
			else
			{
				stringBuilder.Insert(0, "\\");
				stringBuilder.Insert(0, obj.name);
			}
			obj = obj.parent;
		}
		return stringBuilder.ToString();
	}

	public void LateUpdate()
	{
		if (this.AutoRun && !this.IsPlaying && !this.autoRunStarted)
		{
			this.autoRunStarted = true;
			this.Play();
		}
	}

	protected void onCompleted()
	{
		base.SendMessage("AnimationCompleted", this, SendMessageOptions.DontRequireReceiver);
		if (this.AnimationCompleted != null)
		{
			this.AnimationCompleted();
		}
	}

	protected void onPaused()
	{
		base.SendMessage("AnimationPaused", this, SendMessageOptions.DontRequireReceiver);
		if (this.AnimationPaused != null)
		{
			this.AnimationPaused();
		}
	}

	protected void onReset()
	{
		base.SendMessage("AnimationReset", this, SendMessageOptions.DontRequireReceiver);
		if (this.AnimationReset != null)
		{
			this.AnimationReset();
		}
	}

	protected void onResumed()
	{
		base.SendMessage("AnimationResumed", this, SendMessageOptions.DontRequireReceiver);
		if (this.AnimationResumed != null)
		{
			this.AnimationResumed();
		}
	}

	protected void onStarted()
	{
		base.SendMessage("AnimationStarted", this, SendMessageOptions.DontRequireReceiver);
		if (this.AnimationStarted != null)
		{
			this.AnimationStarted();
		}
	}

	protected void onStopped()
	{
		base.SendMessage("AnimationStopped", this, SendMessageOptions.DontRequireReceiver);
		if (this.AnimationStopped != null)
		{
			this.AnimationStopped();
		}
	}

	public void Pause()
	{
		if (this.isRunning)
		{
			this.isPaused = true;
			this.onPaused();
		}
	}

	public override void Play()
	{
		if (this.IsPlaying)
		{
			this.Stop();
		}
		if (!base.enabled || !base.gameObject.activeSelf || !base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.memberInfo == null)
		{
			throw new NullReferenceException("Animation target is NULL");
		}
		if (!this.memberInfo.IsValid)
		{
			throw new InvalidOperationException(string.Concat(new object[] { "Invalid property binding configuration on ", this.getPath(base.gameObject.transform), " - ", this.target }));
		}
		this.target = this.memberInfo.GetProperty();
		base.StartCoroutine(this.Execute());
	}

	public void PlayForward()
	{
		this.playDirection = dfSpriteAnimation.PlayDirection.Forward;
		this.Play();
	}

	public void PlayReverse()
	{
		this.playDirection = dfSpriteAnimation.PlayDirection.Reverse;
		this.Play();
	}

	public override void Reset()
	{
		List<string> sprites;
		if (this.clip == null)
		{
			sprites = null;
		}
		else
		{
			sprites = this.clip.Sprites;
		}
		List<string> strs = sprites;
		if (this.memberInfo.IsValid && strs != null && strs.Count > 0)
		{
			this.memberInfo.Component.SetProperty(this.memberInfo.MemberName, strs[0]);
		}
		if (!this.isRunning)
		{
			return;
		}
		base.StopAllCoroutines();
		this.isRunning = false;
		this.isPaused = false;
		this.onReset();
		this.target = null;
	}

	public void Resume()
	{
		if (this.isRunning && this.isPaused)
		{
			this.isPaused = false;
			this.onResumed();
		}
	}

	private void setFrame(int frameIndex)
	{
		List<string> sprites = this.clip.Sprites;
		if (sprites.Count == 0)
		{
			return;
		}
		frameIndex = Mathf.Max(0, Mathf.Min(frameIndex, sprites.Count - 1));
		if (this.target != null)
		{
			this.target.Value = sprites[frameIndex];
		}
	}

	public void Start()
	{
	}

	public override void Stop()
	{
		List<string> sprites;
		if (!this.isRunning)
		{
			return;
		}
		if (this.clip == null)
		{
			sprites = null;
		}
		else
		{
			sprites = this.clip.Sprites;
		}
		List<string> strs = sprites;
		if (this.skipToEndOnStop && strs != null)
		{
			this.setFrame(Mathf.Max(strs.Count - 1, 0));
		}
		base.StopAllCoroutines();
		this.isRunning = false;
		this.isPaused = false;
		this.onStopped();
		this.target = null;
	}

	public event TweenNotification AnimationCompleted
	{
		add
		{
			this.AnimationCompleted += value;
		}
		remove
		{
			this.AnimationCompleted -= value;
		}
	}

	public event TweenNotification AnimationPaused
	{
		add
		{
			this.AnimationPaused += value;
		}
		remove
		{
			this.AnimationPaused -= value;
		}
	}

	public event TweenNotification AnimationReset
	{
		add
		{
			this.AnimationReset += value;
		}
		remove
		{
			this.AnimationReset -= value;
		}
	}

	public event TweenNotification AnimationResumed
	{
		add
		{
			this.AnimationResumed += value;
		}
		remove
		{
			this.AnimationResumed -= value;
		}
	}

	public event TweenNotification AnimationStarted
	{
		add
		{
			this.AnimationStarted += value;
		}
		remove
		{
			this.AnimationStarted -= value;
		}
	}

	public event TweenNotification AnimationStopped
	{
		add
		{
			this.AnimationStopped += value;
		}
		remove
		{
			this.AnimationStopped -= value;
		}
	}

	public enum PlayDirection
	{
		Forward,
		Reverse
	}
}