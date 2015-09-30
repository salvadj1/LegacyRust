using System;

public class StructureComponentTakeDamage : ProtectionTakeDamage
{
	public StructureComponentTakeDamage()
	{
	}

	protected override LifeStatus Hurt(ref DamageEvent damage)
	{
		return base.Hurt(ref damage);
	}
}