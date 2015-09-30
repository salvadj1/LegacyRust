using System;
using System.IO;
using System.Reflection;
using UnityEngine;

public class BrandingVersion : MonoBehaviour
{
	public dfRichTextLabel textVersion;

	public BrandingVersion()
	{
	}

	private DateTime RetrieveLinkerTimestamp()
	{
		string location = Assembly.GetCallingAssembly().Location;
		byte[] numArray = new byte[2048];
		Stream fileStream = null;
		try
		{
			fileStream = new FileStream(location, FileMode.Open, FileAccess.Read);
			fileStream.Read(numArray, 0, 2048);
		}
		finally
		{
			if (fileStream != null)
			{
				fileStream.Close();
			}
		}
		int num = BitConverter.ToInt32(numArray, 60);
		int num1 = BitConverter.ToInt32(numArray, num + 8);
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);
		dateTime = dateTime.AddSeconds((double)num1);
		TimeSpan utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
		dateTime = dateTime.AddHours((double)utcOffset.Hours);
		return dateTime;
	}

	private void Start()
	{
		DateTime dateTime = this.RetrieveLinkerTimestamp();
		this.textVersion.Text = dateTime.ToString("d MMM yyyy\\, h:mmtt");
	}
}