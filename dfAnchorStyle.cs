using System;

[Flags]
public enum dfAnchorStyle
{
	None = 0,
	Top = 1,
	Bottom = 2,
	Left = 4,
	Right = 8,
	All = 15,
	CenterHorizontal = 64,
	CenterVertical = 128,
	Proportional = 256
}