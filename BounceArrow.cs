using System;
using UnityEngine;

public class BounceArrow : MonoBehaviour
{
	public BounceArrow()
	{
	}

	private void Update()
	{
		float single = 0f + Mathf.Abs(Mathf.Sin(Time.time * 5f)) * 0.15f;
		base.transform.localPosition = new Vector3(0f, single, 0f);
	}
}