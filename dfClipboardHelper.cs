using System;
using System.Reflection;
using UnityEngine;

public class dfClipboardHelper
{
	private static PropertyInfo m_systemCopyBufferProperty;

	public static string clipBoard
	{
		get
		{
			string value;
			try
			{
				value = (string)dfClipboardHelper.GetSystemCopyBufferProperty().GetValue(null, null);
			}
			catch
			{
				value = string.Empty;
			}
			return value;
		}
		set
		{
			try
			{
				dfClipboardHelper.GetSystemCopyBufferProperty().SetValue(null, value, null);
			}
			catch
			{
			}
		}
	}

	static dfClipboardHelper()
	{
	}

	public dfClipboardHelper()
	{
	}

	private static PropertyInfo GetSystemCopyBufferProperty()
	{
		if (dfClipboardHelper.m_systemCopyBufferProperty == null)
		{
			Type type = typeof(GUIUtility);
			dfClipboardHelper.m_systemCopyBufferProperty = type.GetProperty("systemCopyBuffer", BindingFlags.Static | BindingFlags.NonPublic);
			if (dfClipboardHelper.m_systemCopyBufferProperty == null)
			{
				throw new Exception("Can'time access internal member 'GUIUtility.systemCopyBuffer' it may have been removed / renamed");
			}
		}
		return dfClipboardHelper.m_systemCopyBufferProperty;
	}
}