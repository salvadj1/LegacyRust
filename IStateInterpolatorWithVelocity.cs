using System;
using UnityEngine;

public interface IStateInterpolatorWithVelocity : IStateInterpolatorWithLinearVelocity, IStateInterpolatorWithAngularVelocity
{
	bool SampleWorldVelocity(double timeStamp, out Vector3 linear, out Angle2 angular);

	bool SampleWorldVelocity(out Vector3 linear, out Angle2 angular);
}