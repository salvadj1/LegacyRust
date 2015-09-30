using System;
using UnityEngine;

internal interface IIDLocalInterpolator
{
	IDMain idMain
	{
		get;
	}

	IDLocal self
	{
		get;
	}

	void SetGoals(Vector3 pos, Quaternion rot, double timestamp);
}