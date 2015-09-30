using System;

public abstract class HandGrenadeItem<T> : ThrowableItem<T>
where T : HandGrenadeDataBlock
{
	protected HandGrenadeItem(T db) : base(db)
	{
	}
}