using System;
using System.Collections.Generic;
using UnityEngine;

public class ServerSave : MonoBehaviour
{
	private static Dictionary<int, string> StructureDictionary;

	[SerializeField]
	private bool autoNetSerialize = true;

	[NonSerialized]
	private ServerSave.Reged registered;

	internal ServerSave.Reged REGED
	{
		get
		{
			return this.registered;
		}
	}

	public ServerSave()
	{
	}

	internal enum Reged : sbyte
	{
		None,
		ToNet,
		ToNGC
	}
}