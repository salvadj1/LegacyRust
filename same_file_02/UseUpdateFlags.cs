using System;

[Flags]
public enum UseUpdateFlags
{
	None,
	UpdateWithUser,
	UpdateWithoutUser,
	UpdateAlways
}