using System;
using UnityEngine;

[AddComponentMenu("")]
public class ServerSaveManager : MonoBehaviour
{
	[HideInInspector]
	[SerializeField]
	private int nextID = 1;

	[HideInInspector]
	[SerializeField]
	private int[] keys;

	[HideInInspector]
	[SerializeField]
	private ServerSave[] values;

	public ServerSaveManager()
	{
	}
}