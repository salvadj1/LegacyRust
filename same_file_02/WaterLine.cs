using System;
using UnityEngine;

public class WaterLine : MonoBehaviour
{
	public static float Height;

	static WaterLine()
	{
	}

	public WaterLine()
	{
	}

	public void OnDestroy()
	{
		WaterLine.Height = 0f;
	}

	public void Start()
	{
	}

	public void Update()
	{
		WaterLine.Height = base.transform.position.y;
	}
}