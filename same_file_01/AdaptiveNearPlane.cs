using System;
using UnityEngine;

public class AdaptiveNearPlane : MonoBehaviour
{
	public float maxNear = 0.65f;

	public float minNear = 0.22f;

	public float threshold = 0.05f;

	public LayerMask ignoreLayers = 0;

	public LayerMask forceLayers = 0;

	public AdaptiveNearPlane()
	{
	}
}