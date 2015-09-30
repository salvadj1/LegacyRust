using System;
using UnityEngine;

public interface IStateInterpolatorWithLinearVelocity
{
	bool SampleWorldVelocity(double timeStamp, out Vector3 linear);

	bool SampleWorldVelocity(out Vector3 linear);
}