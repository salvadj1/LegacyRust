using System;

public interface IObservableValue
{
	bool HasChanged
	{
		get;
	}

	object Value
	{
		get;
	}
}