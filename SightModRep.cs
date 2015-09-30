using System;

public class SightModRep : WeaponModRep
{
	protected SightModRep(ItemModRepresentation.Caps caps, bool defaultOn) : base(caps, defaultOn)
	{
	}

	public SightModRep() : this(0, true)
	{
	}

	protected SightModRep(ItemModRepresentation.Caps caps) : this(caps, true)
	{
	}

	protected override void DisableMod(ItemModRepresentation.Reason reason)
	{
	}

	protected override void EnableMod(ItemModRepresentation.Reason reason)
	{
	}
}