using System;

public enum ActivationResult
{
	Success,
	Fail_Busy,
	Fail_Broken,
	Fail_Access,
	Fail_Redundant,
	Fail_BadToggle,
	Fail_RequiresInstigator,
	Error_Implementation,
	Error_Destroyed
}