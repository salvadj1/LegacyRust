using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class UseableUtility
{
	public const UseResponse kMinSuccess = UseResponse.Pass_Unchecked;

	public const UseResponse kMinException = UseResponse.Fail_Checked_OutOfOrder | UseResponse.Fail_CheckException;

	public const UseResponse kMaxException = UseResponse.Fail_Checked_OutOfOrder | UseResponse.Fail_Checked_BadConfiguration | UseResponse.Fail_CheckException | UseResponse.Fail_Vacancy;

	public const UseResponse kMinSucessChecked = UseResponse.Pass_Checked;

	public const UseResponse kMaxFailedChecked = UseResponse.Pass_Checked | UseResponse.Fail_Checked_OutOfOrder | UseResponse.Fail_Checked_UserIncompatible | UseResponse.Fail_Checked_BadConfiguration | UseResponse.Fail_Checked_BadResult;

	private static bool log_enabled;

	static UseableUtility()
	{
	}

	public static bool Checked(this UseResponse response)
	{
		return ((int)response < -16 ? true : (int)response > 0);
	}

	public static void Log<T>(T a, UnityEngine.Object b)
	{
		if (UseableUtility.log_enabled)
		{
			Debug.Log(a, b);
		}
	}

	public static void Log<T>(T a)
	{
		if (UseableUtility.log_enabled)
		{
			Debug.Log(a);
		}
	}

	public static void LogError<T>(T a, UnityEngine.Object b)
	{
		if (UseableUtility.log_enabled)
		{
			Debug.LogError(a, b);
		}
	}

	public static void LogError<T>(T a)
	{
		if (UseableUtility.log_enabled)
		{
			Debug.LogError(a);
		}
	}

	public static void LogWarning<T>(T a, UnityEngine.Object b)
	{
		if (UseableUtility.log_enabled)
		{
			Debug.LogWarning(a, b);
		}
	}

	public static void LogWarning<T>(T a)
	{
		if (UseableUtility.log_enabled)
		{
			Debug.LogWarning(a);
		}
	}

	public static void OnDestroy(IUseable self, Useable useable)
	{
		if (useable && useable.occupied)
		{
			useable.Eject();
		}
	}

	public static void OnDestroy(IUseable self)
	{
		MonoBehaviour monoBehaviour = self as MonoBehaviour;
		if (monoBehaviour)
		{
			UseableUtility.OnDestroy(self, monoBehaviour.GetComponent<Useable>());
		}
	}

	public static bool Succeeded(this UseResponse response)
	{
		bool flag = (int)response >= 0;
		if (!flag)
		{
			UseableUtility.LogWarning<string>(string.Concat("Did not succeed ", response));
		}
		return flag;
	}

	public static bool ThrewException<E>(this UseResponse response, out E e, bool doNotClear)
	where E : Exception
	{
		if ((int)response >= -16 && (int)response <= -10)
		{
			return Useable.GetLastException<E>(out e, doNotClear);
		}
		e = (E)null;
		return false;
	}

	public static bool ThrewException(this UseResponse response, out Exception e, bool doNotClear)
	{
		if ((int)response >= -16 && (int)response <= -10)
		{
			return Useable.GetLastException(out e, doNotClear);
		}
		e = null;
		return false;
	}

	public static bool ThrewException(this UseResponse response, out Exception e)
	{
		return response.ThrewException(out e, false);
	}
}