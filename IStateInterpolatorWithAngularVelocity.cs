using System;

public interface IStateInterpolatorWithAngularVelocity
{
	bool SampleWorldVelocity(double timeStamp, out Angle2 angular);

	bool SampleWorldVelocity(out Angle2 angular);
}