using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class VectorExtensions
{
	public static Vector3 CeilToInt(this Vector3 vector)
	{
		return new Vector3((float)Mathf.CeilToInt(vector.x), (float)Mathf.CeilToInt(vector.y), (float)Mathf.CeilToInt(vector.z));
	}

	public static Vector2 CeilToInt(this Vector2 vector)
	{
		return new Vector2((float)Mathf.CeilToInt(vector.x), (float)Mathf.CeilToInt(vector.y));
	}

	public static Vector3 FloorToInt(this Vector3 vector)
	{
		return new Vector3((float)Mathf.FloorToInt(vector.x), (float)Mathf.FloorToInt(vector.y), (float)Mathf.FloorToInt(vector.z));
	}

	public static Vector2 FloorToInt(this Vector2 vector)
	{
		return new Vector2((float)Mathf.FloorToInt(vector.x), (float)Mathf.FloorToInt(vector.y));
	}

	public static Vector2 Quantize(this Vector2 vector, float discreteValue)
	{
		vector.x = (float)Mathf.RoundToInt(vector.x / discreteValue) * discreteValue;
		vector.y = (float)Mathf.RoundToInt(vector.y / discreteValue) * discreteValue;
		return vector;
	}

	public static Vector3 Quantize(this Vector3 vector, float discreteValue)
	{
		vector.x = (float)Mathf.RoundToInt(vector.x / discreteValue) * discreteValue;
		vector.y = (float)Mathf.RoundToInt(vector.y / discreteValue) * discreteValue;
		vector.z = (float)Mathf.RoundToInt(vector.z / discreteValue) * discreteValue;
		return vector;
	}

	public static Vector3 RoundToInt(this Vector3 vector)
	{
		return new Vector3((float)Mathf.RoundToInt(vector.x), (float)Mathf.RoundToInt(vector.y), (float)Mathf.RoundToInt(vector.z));
	}

	public static Vector2 RoundToInt(this Vector2 vector)
	{
		return new Vector2((float)Mathf.RoundToInt(vector.x), (float)Mathf.RoundToInt(vector.y));
	}

	public static Vector2 Scale(this Vector2 vector, float x, float y)
	{
		return new Vector2(vector.x * x, vector.y * y);
	}

	public static Vector3 Scale(this Vector3 vector, float x, float y, float z)
	{
		return new Vector3(vector.x * x, vector.y * y, vector.z * z);
	}
}