using System;
using System.Runtime.Serialization;

namespace Facepunch.Abstract
{
	[Serializable]
	internal class KeyArgumentIsKeyTypeException : ArgumentOutOfRangeException
	{
		public KeyArgumentIsKeyTypeException()
		{
		}

		public KeyArgumentIsKeyTypeException(string parameterName) : base(parameterName)
		{
		}

		public KeyArgumentIsKeyTypeException(string parameterName, string message) : base(parameterName, message)
		{
		}

		public KeyArgumentIsKeyTypeException(string message, Exception inner) : base(message, inner)
		{
		}

		protected KeyArgumentIsKeyTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}