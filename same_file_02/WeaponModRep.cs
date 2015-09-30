using System;
using UnityEngine;

public abstract class WeaponModRep : ItemModRepresentation
{
	private GameObject _attached;

	protected readonly bool defaultsOn;

	private bool _on;

	public GameObject attached
	{
		get
		{
			return this._attached;
		}
		protected set
		{
			if (value != this._attached)
			{
				if (!value)
				{
					if (this._attached)
					{
						this.OnRemoveAttached();
					}
					this._attached = null;
				}
				else
				{
					if (!this.VerifyCompatible(value))
					{
						throw new ArgumentOutOfRangeException("value", "incompatible");
					}
					if (this._attached)
					{
						this.OnRemoveAttached();
					}
					this._attached = value;
					this.OnAddAttached();
					if (!this._on)
					{
						this.DisableMod(ItemModRepresentation.Reason.Implicit);
					}
					else
					{
						this.EnableMod(ItemModRepresentation.Reason.Implicit);
					}
				}
			}
			this._attached = value;
		}
	}

	public bool on
	{
		get
		{
			return this._on;
		}
		protected set
		{
			this.SetOn(value, ItemModRepresentation.Reason.Explicit);
		}
	}

	protected WeaponModRep(ItemModRepresentation.Caps caps, bool defaultsOn) : base(caps)
	{
		this.defaultsOn = defaultsOn;
		this._on = defaultsOn;
	}

	protected WeaponModRep(ItemModRepresentation.Caps caps) : this(caps, false)
	{
	}

	protected abstract void DisableMod(ItemModRepresentation.Reason reason);

	protected abstract void EnableMod(ItemModRepresentation.Reason reason);

	protected virtual void OnAddAttached()
	{
	}

	protected virtual void OnRemoveAttached()
	{
	}

	public virtual void SetAttached(GameObject attached, bool vm)
	{
		this.attached = attached;
	}

	protected void SetOn(bool on, ItemModRepresentation.Reason reason)
	{
		if (this._on != on)
		{
			this._on = on;
			if (this._attached)
			{
				if (!on)
				{
					this.DisableMod(reason);
				}
				else
				{
					this.EnableMod(reason);
				}
			}
		}
	}

	protected virtual bool VerifyCompatible(GameObject attachment)
	{
		return true;
	}
}