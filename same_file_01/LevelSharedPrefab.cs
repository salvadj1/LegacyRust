using System;
using UnityEngine;

public class LevelSharedPrefab : MonoBehaviour
{
	public LevelSharedPrefab()
	{
	}

	private void Awake()
	{
		base.transform.DetachChildren();
		UnityEngine.Object.Destroy(this);
	}
}