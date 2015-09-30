using System;
using System.Runtime.CompilerServices;

public static class ContextStatus
{
	public const ContextStatusFlags ObjectBusy = ContextStatusFlags.ObjectBusy;

	public const ContextStatusFlags ObjectBroken = ContextStatusFlags.ObjectBroken;

	public const ContextStatusFlags ObjectEmpty = ContextStatusFlags.ObjectEmpty;

	public const ContextStatusFlags ObjectOccupied = ContextStatusFlags.ObjectOccupied;

	public const ContextStatusFlags SPRITE_DEFAULT = 0;

	public const ContextStatusFlags SPRITE_FRACTION = ContextStatusFlags.SpriteFlag0;

	public const ContextStatusFlags SPRITE_NEVER = ContextStatusFlags.SpriteFlag1;

	public const ContextStatusFlags SPRITE_ALWAYS = ContextStatusFlags.SpriteFlag0 | ContextStatusFlags.SpriteFlag1;

	public const ContextStatusFlags MASK_SPRITE = ContextStatusFlags.SpriteFlag0 | ContextStatusFlags.SpriteFlag1;

	public static ContextStatusFlags CopyWithSpriteSetting(this ContextStatusFlags statusFlags, ContextStatusFlags SPRITE_SETTING)
	{
		return statusFlags & (ContextStatusFlags.ObjectBusy | ContextStatusFlags.ObjectBroken | ContextStatusFlags.ObjectEmpty | ContextStatusFlags.ObjectOccupied) | SPRITE_SETTING & (ContextStatusFlags.SpriteFlag0 | ContextStatusFlags.SpriteFlag1);
	}

	public static ContextStatusFlags GetSpriteFlags(this ContextStatusFlags statusFlags)
	{
		return statusFlags & (ContextStatusFlags.SpriteFlag0 | ContextStatusFlags.SpriteFlag1);
	}
}