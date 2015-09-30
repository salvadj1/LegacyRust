using System;

public interface IStateInterpolator<TSampleType> : IStateInterpolatorSampler<TSampleType>
{
	void SetGoals(ref TSampleType sample, ref double timeStamp);

	void SetGoals(ref TimeStamped<TSampleType> sample);
}