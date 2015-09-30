using System;

[Flags]
public enum ContextStatusFlags
{
	ObjectBusy = 1,
	ObjectBroken = 2,
	ObjectEmpty = 4,
	ObjectOccupied = 8,
	SpriteFlag0 = 536870912,
	SpriteFlag1 = 1073741824
}