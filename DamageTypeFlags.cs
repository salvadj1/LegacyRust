using System;

[Flags]
public enum DamageTypeFlags
{
	damage_generic = 1,
	damage_bullet = 2,
	damage_melee = 4,
	damage_explosion = 8,
	damage_radiation = 16,
	damage_cold = 32
}