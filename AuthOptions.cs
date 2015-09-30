using System;

[Flags]
public enum AuthOptions
{
	SearchDown = 1,
	SearchUp = 2,
	SearchInclusive = 8,
	SearchReverse = 16
}