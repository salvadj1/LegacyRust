using System;

public class SilencerModRep : WeaponModRep
{
	protected SilencerModRep(ItemModRepresentation.Caps caps, bool defaultOn) : base(caps, defaultOn)
	{
	}

	public SilencerModRep() : this(0, true)
	{
	}

	protected SilencerModRep(ItemModRepresentation.Caps caps) : this(caps, true)
	{
	}

	protected override void DisableMod(ItemModRepresentation.Reason reason)
	{
	}

	protected override void EnableMod(ItemModRepresentation.Reason reason)
	{
	}
}