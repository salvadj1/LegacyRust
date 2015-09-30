using System;

public struct RepairEvent
{
	public IDBase doner;

	public TakeDamage receiver;

	public float givenAmount;

	public float usedAmount;

	public RepairStatus status;

	public IDMain beneficiary
	{
		get
		{
			IDMain dMain;
			if (!this.receiver)
			{
				dMain = null;
			}
			else
			{
				dMain = this.receiver.idMain;
			}
			return dMain;
		}
	}

	public override string ToString()
	{
		return string.Format("[RepairEvent: beneficiary={0} givenAmount={1} usedAmount={5} status={2} doner={3} receiver={4}]", new object[] { this.beneficiary, this.givenAmount, this.status, this.doner, this.receiver, this.usedAmount });
	}
}