using System;

public abstract class IDLocalCharacterIdleControl : IDLocalCharacter
{
	[NonSerialized]
	internal bool _idle = true;

	[NonSerialized]
	internal bool _setIdle;

	[NonSerialized]
	internal bool _changingIdle;

	public new bool? idle
	{
		get
		{
			if (this._setIdle)
			{
				return new bool?(this._idle);
			}
			return null;
		}
	}

	public new IDLocalCharacterIdleControl idleControl
	{
		get
		{
			return this;
		}
	}

	protected IDLocalCharacterIdleControl()
	{
	}

	protected abstract void OnIdleEnter();

	protected abstract void OnIdleExit();

	internal bool SetIdle(bool value)
	{
		if (this._setIdle)
		{
			if (this._idle == value)
			{
				return false;
			}
			if (this._changingIdle)
			{
				throw new InvalidOperationException("check callstack. idleControl.set was invoked multiple times. avoid it");
			}
		}
		else
		{
			this._setIdle = true;
		}
		this._changingIdle = true;
		this._idle = value;
		if (!value)
		{
			try
			{
				this.OnIdleExit();
			}
			finally
			{
				this._changingIdle = false;
			}
		}
		else
		{
			try
			{
				this.OnIdleEnter();
			}
			finally
			{
				this._changingIdle = false;
			}
		}
		return true;
	}
}