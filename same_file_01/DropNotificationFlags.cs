using System;

[Flags]
public enum DropNotificationFlags
{
	DragDrop = 1,
	DragLand = 2,
	DragReverse = 4,
	AltDrop = 8,
	AltLand = 16,
	AltReverse = 32,
	MidDrop = 64,
	MidLand = 128,
	MidReverse = 256,
	DragHover = 512,
	LandHover = 1024,
	ReverseHover = 2048,
	RegularHover = 4096,
	DragLandOutside = 8192
}