using System;

namespace Facepunch.Attributes
{
	public enum CustomLookupResult
	{
		FailConfirmException = -7,
		FailConfirm = -6,
		FailCustomException = -5,
		FailCustom = -4,
		FailInterface = -3,
		FailCast = -2,
		FailNull = -1,
		Fallback = 0,
		Accept = 1,
		AcceptConfirmed = 2
	}
}