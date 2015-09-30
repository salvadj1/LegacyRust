using System;

public class RepairReceiver : IDLocal
{
	public ItemDataBlock repairAmmo;

	public int ResForMaxHealth = 10;

	public RepairReceiver()
	{
	}

	public ItemDataBlock GetRepairAmmo()
	{
		return this.repairAmmo;
	}
}