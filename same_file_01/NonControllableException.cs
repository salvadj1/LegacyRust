using System;
using System.Runtime.Serialization;

[Serializable]
public class NonControllableException : InstantiateControllableException
{
	public NonControllableException()
	{
	}

	public NonControllableException(string message) : base(message)
	{
	}

	public NonControllableException(string message, Exception inner) : base(message, inner)
	{
	}

	protected NonControllableException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}