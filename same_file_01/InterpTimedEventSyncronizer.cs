using System;
using UnityEngine;

public class InterpTimedEventSyncronizer : MonoBehaviour
{
	private static InterpTimedEventSyncronizer singleton;

	private static bool syncronizationPaused;

	private static bool exists;

	internal static bool available
	{
		get
		{
			return InterpTimedEventSyncronizer.exists;
		}
	}

	internal static bool paused
	{
		get
		{
			return InterpTimedEventSyncronizer.syncronizationPaused;
		}
		set
		{
			InterpTimedEventSyncronizer.syncronizationPaused = value;
			if (InterpTimedEventSyncronizer.singleton)
			{
				InterpTimedEventSyncronizer.singleton.enabled = !InterpTimedEventSyncronizer.syncronizationPaused;
			}
		}
	}

	public InterpTimedEventSyncronizer()
	{
	}

	private void Awake()
	{
		if (InterpTimedEventSyncronizer.singleton)
		{
			Debug.LogWarning("Destroying old singleton!", InterpTimedEventSyncronizer.singleton.gameObject);
			UnityEngine.Object.Destroy(InterpTimedEventSyncronizer.singleton);
		}
		InterpTimedEventSyncronizer.singleton = this;
		InterpTimedEventSyncronizer.exists = true;
	}

	private void OnDestroy()
	{
		if (InterpTimedEventSyncronizer.singleton == this)
		{
			try
			{
				InterpTimedEvent.Clear(false);
			}
			finally
			{
				InterpTimedEventSyncronizer.singleton = null;
				InterpTimedEventSyncronizer.exists = false;
			}
		}
	}

	private void Update()
	{
		InterpTimedEvent.Catchup();
	}
}