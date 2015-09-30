using System;
using System.Runtime.Serialization;

[Serializable]
public class NonBindlessVesselControllableException : InstantiateControllableException
{
	public NonBindlessVesselControllableException()
	{
	}

	public NonBindlessVesselControllableException(string message) : base(message)
	{
	}

	public NonBindlessVesselControllableException(string message, Exception inner) : base(message, inner)
	{
	}

	protected NonBindlessVesselControllableException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}