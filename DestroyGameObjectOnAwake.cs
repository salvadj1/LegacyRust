using System;
using UnityEngine;

public class DestroyGameObjectOnAwake : MonoBehaviour
{
	public DestroyGameObjectOnAwake()
	{
	}

	private void Awake()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}