using System;
using System.Runtime.Serialization;

[Serializable]
public class NonAIVesselControllableException : InstantiateControllableException
{
	public NonAIVesselControllableException()
	{
	}

	public NonAIVesselControllableException(string message) : base(message)
	{
	}

	public NonAIVesselControllableException(string message, Exception inner) : base(message, inner)
	{
	}

	protected NonAIVesselControllableException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}