using System;
using UnityEngine;

public abstract class dfTweenPlayableBase : MonoBehaviour
{
	public abstract bool IsPlaying
	{
		get;
	}

	public abstract string TweenName
	{
		get;
		set;
	}

	protected dfTweenPlayableBase()
	{
	}

	public void Disable()
	{
		base.enabled = false;
	}

	public void Enable()
	{
		base.enabled = true;
	}

	public abstract void Play();

	public abstract void Reset();

	public abstract void Stop();

	public override string ToString()
	{
		return string.Concat(this.TweenName, " - ", base.ToString());
	}
}