using System;

namespace Facepunch.Build
{
	public interface ConnectionHandler : IDisposable
	{
		string address
		{
			get;
		}

		int? port
		{
			get;
		}
	}
}