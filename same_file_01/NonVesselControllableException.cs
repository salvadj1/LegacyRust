using System;
using System.Runtime.Serialization;

[Serializable]
public class NonVesselControllableException : InstantiateControllableException
{
	public NonVesselControllableException()
	{
	}

	public NonVesselControllableException(string message) : base(message)
	{
	}

	public NonVesselControllableException(string message, Exception inner) : base(message, inner)
	{
	}

	protected NonVesselControllableException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}