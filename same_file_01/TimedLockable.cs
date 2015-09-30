using System;

public class TimedLockable : LockableObject
{
	private ulong ownerID;

	private float lockTime;

	public TimedLockable()
	{
	}
}