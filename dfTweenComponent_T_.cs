using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

[Serializable]
public abstract class dfTweenComponent<T> : dfTweenComponentBase
{
	[SerializeField]
	protected T startValue;

	[SerializeField]
	protected T endValue;

	private T actualStartValue;

	private T actualEndValue;

	private TweenNotification TweenStarted;

	private TweenNotification TweenStopped;

	private TweenNotification TweenPaused;

	private TweenNotification TweenResumed;

	private TweenNotification TweenReset;

	private TweenNotification TweenCompleted;

	public T EndValue
	{
		get
		{
			return this.endValue;
		}
		set
		{
			this.endValue = value;
			if (this.isRunning)
			{
				this.Stop();
				this.Play();
			}
		}
	}

	public T StartValue
	{
		get
		{
			return this.startValue;
		}
		set
		{
			this.startValue = value;
			if (this.isRunning)
			{
				this.Stop();
				this.Play();
			}
		}
	}

	protected dfTweenComponent()
	{
	}

	public abstract T evaluate(T startValue, T endValue, float time);

	[DebuggerHidden]
	protected internal IEnumerator Execute(dfObservableProperty property)
	{
		dfTweenComponent<T>.<Execute>c__Iterator45 variable = null;
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

	protected internal static float Lerp(float startValue, float endValue, float time)
	{
		return startValue + (endValue - startValue) * time;
	}

	public abstract T offset(T value, T offset);

	protected internal override void onCompleted()
	{
		base.SendMessage("TweenCompleted", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenCompleted != null)
		{
			this.TweenCompleted();
		}
	}

	protected internal override void onPaused()
	{
		base.SendMessage("TweenPaused", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenPaused != null)
		{
			this.TweenPaused();
		}
	}

	protected internal override void onReset()
	{
		base.SendMessage("TweenReset", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenReset != null)
		{
			this.TweenReset();
		}
	}

	protected internal override void onResumed()
	{
		base.SendMessage("TweenResumed", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenResumed != null)
		{
			this.TweenResumed();
		}
	}

	protected internal override void onStarted()
	{
		base.SendMessage("TweenStarted", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenStarted != null)
		{
			this.TweenStarted();
		}
	}

	protected internal override void onStopped()
	{
		base.SendMessage("TweenStopped", this, SendMessageOptions.DontRequireReceiver);
		if (this.TweenStopped != null)
		{
			this.TweenStopped();
		}
	}

	public void Pause()
	{
		base.IsPaused = true;
	}

	public override void Play()
	{
		if (this.isRunning)
		{
			this.Stop();
		}
		if (!base.enabled || !base.gameObject.activeSelf || !base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.target == null)
		{
			throw new NullReferenceException("Tween target is NULL");
		}
		if (!this.target.IsValid)
		{
			throw new InvalidOperationException(string.Concat(new object[] { "Invalid property binding configuration on ", this.getPath(base.gameObject.transform), " - ", this.target }));
		}
		dfObservableProperty property = this.target.GetProperty();
		base.StartCoroutine(this.Execute(property));
	}

	public override void Reset()
	{
		if (!this.isRunning)
		{
			return;
		}
		this.boundProperty.Value = this.actualStartValue;
		base.StopAllCoroutines();
		this.isRunning = false;
		this.onReset();
		this.easingFunction = null;
		this.boundProperty = null;
	}

	public void Resume()
	{
		base.IsPaused = false;
	}

	public override void Stop()
	{
		if (!this.isRunning)
		{
			return;
		}
		if (this.skipToEndOnStop)
		{
			this.boundProperty.Value = this.actualEndValue;
		}
		base.StopAllCoroutines();
		this.isRunning = false;
		this.onStopped();
		this.easingFunction = null;
		this.boundProperty = null;
	}

	public override string ToString()
	{
		if (base.Target == null || !base.Target.IsValid)
		{
			return this.TweenName;
		}
		string component = this.target.Component.name;
		return string.Format("{0} ({1}.{2})", this.TweenName, component, this.target.MemberName);
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

	public event TweenNotification TweenPaused
	{
		add
		{
			this.TweenPaused += value;
		}
		remove
		{
			this.TweenPaused -= value;
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

	public event TweenNotification TweenResumed
	{
		add
		{
			this.TweenResumed += value;
		}
		remove
		{
			this.TweenResumed -= value;
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
}