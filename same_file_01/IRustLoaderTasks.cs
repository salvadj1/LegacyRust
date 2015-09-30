using Facepunch.Load;
using System;
using System.Collections.Generic;

public interface IRustLoaderTasks
{
	bool Active
	{
		get;
	}

	IDownloadTask ActiveGroup
	{
		get;
	}

	IEnumerable<IDownloadTask> ActiveJobs
	{
		get;
	}

	IEnumerable<IDownloadTask> Groups
	{
		get;
	}

	IEnumerable<IDownloadTask> Jobs
	{
		get;
	}

	IDownloadTask Overall
	{
		get;
	}
}