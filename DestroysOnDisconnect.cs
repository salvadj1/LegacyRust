using System;
using System.Collections.Generic;
using uLink;
using UnityEngine;

public sealed class DestroysOnDisconnect : UnityEngine.MonoBehaviour
{
	private static bool ListClassInitialized;

	private bool inList;

	public DestroysOnDisconnect()
	{
	}

	public static void ApplyToGameObject(GameObject gameObject)
	{
		if (!gameObject.GetComponent<DestroysOnDisconnect>())
		{
			gameObject.AddComponent<DestroysOnDisconnect>();
		}
	}

	private void Awake()
	{
		if (!this.inList)
		{
			this.inList = true;
			try
			{
				DestroysOnDisconnect.List.all.Add(this);
			}
			catch
			{
				this.inList = false;
				throw;
			}
		}
	}

	private void DestroyManually()
	{
		if (this.inList)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		if (this.inList)
		{
			try
			{
				if (!DestroysOnDisconnect.List.all.Remove(this))
				{
					Debug.LogWarning("serious problem, script reload?", this);
				}
			}
			finally
			{
				this.inList = false;
			}
		}
	}

	public static void OnDisconnectedFromServer()
	{
		if (DestroysOnDisconnect.ListClassInitialized && DestroysOnDisconnect.List.all.Count > 0)
		{
			DestroysOnDisconnect[] array = DestroysOnDisconnect.List.all.ToArray();
			for (int i = 0; i < (int)array.Length; i++)
			{
				DestroysOnDisconnect destroysOnDisconnect = array[i];
				if (destroysOnDisconnect)
				{
					UnityEngine.Object.Destroy(destroysOnDisconnect.gameObject);
				}
			}
		}
	}

	private void uLink_OnDisconnectedFromServer(uLink.NetworkDisconnection blowme)
	{
		this.DestroyManually();
	}

	private static class List
	{
		public readonly static List<DestroysOnDisconnect> all;

		static List()
		{
			DestroysOnDisconnect.List.all = new List<DestroysOnDisconnect>();
			DestroysOnDisconnect.ListClassInitialized = true;
		}
	}
}