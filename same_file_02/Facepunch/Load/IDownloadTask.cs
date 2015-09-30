using System;

namespace Facepunch.Load
{
	public interface IDownloadTask
	{
		int ByteLength
		{
			get;
		}

		int ByteLengthDownloaded
		{
			get;
		}

		string ContextualDescription
		{
			get;
		}

		int Count
		{
			get;
		}

		int Done
		{
			get;
		}

		string Name
		{
			get;
		}

		float PercentDone
		{
			get;
		}

		Facepunch.Load.TaskStatus TaskStatus
		{
			get;
		}

		Facepunch.Load.TaskStatusCount TaskStatusCount
		{
			get;
		}
	}
}