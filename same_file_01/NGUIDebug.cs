using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Debug")]
public class NGUIDebug : MonoBehaviour
{
	private static List<string> mLines;

	private static NGUIDebug mInstance;

	static NGUIDebug()
	{
		NGUIDebug.mLines = new List<string>();
		NGUIDebug.mInstance = null;
	}

	public NGUIDebug()
	{
	}

	public static void DrawBounds(Bounds b)
	{
		Vector3 vector3 = b.center;
		Vector3 vector31 = b.center - b.extents;
		Vector3 vector32 = b.center + b.extents;
		Debug.DrawLine(new Vector3(vector31.x, vector31.y, vector3.z), new Vector3(vector32.x, vector31.y, vector3.z), Color.red);
		Debug.DrawLine(new Vector3(vector31.x, vector31.y, vector3.z), new Vector3(vector31.x, vector32.y, vector3.z), Color.red);
		Debug.DrawLine(new Vector3(vector32.x, vector31.y, vector3.z), new Vector3(vector32.x, vector32.y, vector3.z), Color.red);
		Debug.DrawLine(new Vector3(vector31.x, vector32.y, vector3.z), new Vector3(vector32.x, vector32.y, vector3.z), Color.red);
	}

	public static void Log(string text)
	{
		if (!Application.isPlaying)
		{
			Debug.Log(text);
		}
		else
		{
			if (NGUIDebug.mLines.Count > 20)
			{
				NGUIDebug.mLines.RemoveAt(0);
			}
			NGUIDebug.mLines.Add(text);
			if (NGUIDebug.mInstance == null)
			{
				GameObject gameObject = new GameObject("_NGUI Debug");
				NGUIDebug.mInstance = gameObject.AddComponent<NGUIDebug>();
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
		}
	}

	private void OnGUI()
	{
		int num = 0;
		int count = NGUIDebug.mLines.Count;
		while (num < count)
		{
			GUILayout.Label(NGUIDebug.mLines[num], new GUILayoutOption[0]);
			num++;
		}
	}
}