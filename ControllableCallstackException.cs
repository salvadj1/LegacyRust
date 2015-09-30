using System;
using System.Runtime.Serialization;

[Serializable]
public class ControllableCallstackException : InvalidOperationException
{
	public ControllableCallstackException()
	{
	}

	public ControllableCallstackException(string message) : base(message)
	{
	}

	public ControllableCallstackException(string message, Exception inner) : base(message, inner)
	{
	}

	protected ControllableCallstackException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}