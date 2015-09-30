using System;
using UnityEngine;

public abstract class CCTotem<TTotemObject, CCTotemScript> : CCTotem<TTotemObject>
where TTotemObject : CCTotem.TotemicObject<CCTotemScript, TTotemObject>, new()
where CCTotemScript : CCTotem<TTotemObject, CCTotemScript>, new()
{
	[NonSerialized]
	private bool destroyed;

	internal CCTotem()
	{
	}

	internal void AssignTotemicObject(TTotemObject totemObject)
	{
		if (!object.ReferenceEquals(this.totemicObject, null))
		{
			if (object.ReferenceEquals(this.totemicObject, totemObject))
			{
				return;
			}
			this.ClearTotemicObject();
		}
		this.totemicObject = totemObject;
		if (!object.ReferenceEquals(this.totemicObject, null))
		{
			if (this.destroyed)
			{
				this.totemicObject = (TTotemObject)null;
				throw new InvalidOperationException("Cannot assign non-null script during destruction");
			}
			this.totemicObject.AssignedToScript((CCTotemScript)this);
		}
	}

	protected void ClearTotemicObject()
	{
		TTotemObject tTotemObject = this.totemicObject;
		try
		{
			try
			{
				this.totemicObject.OnScriptDestroy((CCTotemScript)this);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, this);
			}
		}
		finally
		{
			if (object.ReferenceEquals(tTotemObject, this.totemicObject))
			{
				this.totemicObject = (TTotemObject)null;
			}
		}
	}

	protected void OnDestroy()
	{
		if (!this.destroyed)
		{
			this.destroyed = true;
			if (!object.ReferenceEquals(this.totemicObject, null))
			{
				this.ClearTotemicObject();
			}
		}
	}
}