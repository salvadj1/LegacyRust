using System;
using UnityEngine;

public class RustLoaderInstantiateOnComplete : MonoBehaviour
{
	public GameObject[] prefabs;

	public RustLoaderInstantiateOnComplete()
	{
	}

	private void InstantiatePrefab(GameObject prefab)
	{
		try
		{
			UnityEngine.Object.Instantiate(prefab).name = prefab.name;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	private void OnRustReady()
	{
		if (this.prefabs != null)
		{
			GameObject[] gameObjectArray = this.prefabs;
			for (int i = 0; i < (int)gameObjectArray.Length; i++)
			{
				GameObject gameObject = gameObjectArray[i];
				if (gameObject)
				{
					this.InstantiatePrefab(gameObject);
				}
			}
		}
	}

	private void Reset()
	{
		UnityEngine.Object[] objArray = UnityEngine.Object.FindObjectsOfType(typeof(RustLoader));
		if ((int)objArray.Length > 0)
		{
			((RustLoader)objArray[0]).AddMessageReceiver(base.gameObject);
		}
	}
}