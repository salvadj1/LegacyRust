using System;

public enum UseCheck : sbyte
{
	OutOfOrder = -128,
	BadUser = -127,
	BadConfiguration = -126,
	Success = 1
}