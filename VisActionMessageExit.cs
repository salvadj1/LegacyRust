using System;

public class VisActionMessageExit : VisActionMessageEnter
{
	public VisActionMessageExit()
	{
	}

	public override void Accomplish(IDMain self, IDMain instigator)
	{
	}

	public override void UnAcomplish(IDMain self, IDMain instigator)
	{
		base.Accomplish(self, instigator);
	}
}