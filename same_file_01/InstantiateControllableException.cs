using System;
using System.Runtime.Serialization;

[Serializable]
public abstract class InstantiateControllableException : ArgumentException
{
	public InstantiateControllableException()
	{
	}

	public InstantiateControllableException(string message) : base(message)
	{
	}

	public InstantiateControllableException(string message, Exception inner) : base(message, inner)
	{
	}

	protected InstantiateControllableException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}