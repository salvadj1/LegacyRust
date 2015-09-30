using System;

public enum UseResponse : sbyte
{
	Fail_Checked_OutOfOrder = -128,
	Fail_Checked_UserIncompatible = -127,
	Fail_Checked_BadConfiguration = -126,
	Fail_Checked_BadResult = -125,
	Fail_CheckException = -16,
	Fail_EnterException = -15,
	Fail_Vacancy = -10,
	Fail_Redundant = -9,
	Fail_UserDead = -8,
	Fail_Destroyed = -7,
	Fail_NotIUseable = -6,
	Fail_InvalidOperation = -5,
	Fail_NullOrMissingUser = -4,
	Pass_Unchecked = 0,
	Pass_Checked = 1
}