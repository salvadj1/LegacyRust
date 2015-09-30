using System;
using UnityEngine;

public class WaterMesh : MonoBehaviour
{
	public WaterMesher root;

	public float underFlow;

	public float minDistance = 2f;

	public int sensitivity = 256;

	public bool smooth;

	public bool reverseOrder;

	public WaterMesh()
	{
	}
}