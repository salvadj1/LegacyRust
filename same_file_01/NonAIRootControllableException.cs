using System;
using System.Runtime.Serialization;

[Serializable]
public class NonAIRootControllableException : InstantiateControllableException
{
	public NonAIRootControllableException()
	{
	}

	public NonAIRootControllableException(string message) : base(message)
	{
	}

	public NonAIRootControllableException(string message, Exception inner) : base(message, inner)
	{
	}

	protected NonAIRootControllableException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}