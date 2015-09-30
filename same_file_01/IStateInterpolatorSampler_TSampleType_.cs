using System;

public interface IStateInterpolatorSampler<TSampleType>
{
	bool Sample(ref double timeStamp, out TSampleType sample);
}