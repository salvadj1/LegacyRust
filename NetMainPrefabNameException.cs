using System;
using System.Runtime.Serialization;

[Serializable]
public class NetMainPrefabNameException : ArgumentOutOfRangeException
{
	public NetMainPrefabNameException()
	{
	}

	public NetMainPrefabNameException(string parameter) : base(parameter)
	{
	}

	public NetMainPrefabNameException(string parameter, string message) : base(parameter, message)
	{
	}

	public NetMainPrefabNameException(string parameter, string value, string message) : base(parameter, value, message)
	{
	}

	public NetMainPrefabNameException(string message, Exception inner) : base(message, inner)
	{
	}

	protected NetMainPrefabNameException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}