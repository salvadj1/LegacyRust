using System;

public abstract class CCTotem<TTotemObject> : CCTotem
where TTotemObject : CCTotem.TotemicObject
{
	protected internal TTotemObject totemicObject;

	internal sealed override CCTotem.TotemicObject _Object
	{
		get
		{
			return (object)this.totemicObject;
		}
	}

	internal CCTotem()
	{
	}
}